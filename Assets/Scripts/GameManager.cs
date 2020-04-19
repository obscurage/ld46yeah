using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    //You cna use GameManagers variables etc in any script with GameManager.instance.myVariable/myFunction() 
    public static GameManager instance = null;
    public GameObject player;
    public CartObject foodCart;
    public int foodPriceMax = 50;
    public int foodPriceMin = 25;
    [Tooltip("How much the cart slows the player")]
    public float cartMultiplier = 0.5f;

    public float startCoal = 100;
    public float coalLeft;
    public float coalInMachine;

    public float maxCoalInMachine = 20;
    public float currentSpeed = 141;
    public float speedMax = 141;
    public float currentTemperature = 0;
    public float temperatureMax = 100;
    public float temperatureMin = 0;

    //How much coal is used in one interaction
    public float coalPerThrow = 3;

    public float coalBurnRate = 1;

    public int money;
    public int customerMoneyToPayMax = 50;
    public int customerMoneyToPayMin = 10;

    public GameObject customerPrefab;
    public List<GameObject> customerSpots = new List<GameObject>();
    public int minCustomers = 10;
    public int maxCustomers = 20;
    private float startTime;

    [Tooltip("Distance in km")]
    public float totalDistance = 5;
    float distanceTravelled = 0;
    public float backgroundSpeedMultiplier = 2;

    [SerializeField]
    AudioSource audioSource;
    public AudioClip[] maleVoice;
    public AudioClip[] femaleVoice;

    AudioMixerGroup pitchBendGroup;

    public float playTime;
    public List<Animator> animators = new List<Animator>();

    [SerializeField] BackgroundMusicPlayer backgroundMusicPlayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // This should be called when player starts the game by pressin Start on main menu.
        backgroundMusicPlayer.requestClipChange(1);        

        maleVoice = Resources.LoadAll<AudioClip>("CharactersVoices/Male");
        femaleVoice = Resources.LoadAll<AudioClip>("CharactersVoices/Female");
        coalLeft = startCoal;

        pitchBendGroup = Resources.Load<AudioMixerGroup>("BackgroundMixer");
        audioSource.outputAudioMixerGroup = pitchBendGroup;
        startTime = Time.time;
        if(maxCustomers > customerSpots.Count)
        {
            maxCustomers = customerSpots.Count;
        }
        if(minCustomers > maxCustomers)
        {
            minCustomers = maxCustomers;
        }
        SpawnCustomers();
    }

    private void Update()
    {
        BurnCoal();
        playTime = Time.time - startTime;
        CalculateTempo();
        CalculateDistance();
        CalculateAnimationSpeed();
        HandleMusicChanges();
    }

    void CalculateDistance()
    {
        if(distanceTravelled + currentSpeed * Time.deltaTime < totalDistance * 1000)
        {
            distanceTravelled += currentSpeed * Time.deltaTime;
        }
        else
        {
            distanceTravelled = totalDistance;
            print("voitit pelin");
        }
    }

    float CalculateDistanceTravelledAsPercentage()
    {
        if (distanceTravelled <= 0) return 0f;
        return (distanceTravelled / (totalDistance * 1000)) * 100;
    }

    private void CalculateTempo()
    {
        float pitch = currentSpeed / speedMax;
        audioSource.pitch = pitch;
        pitchBendGroup.audioMixer.SetFloat("PitchBend", 1 / pitch);
        audioSource.volume = 1 / Mathf.Pow(Vector2.Distance(audioSource.gameObject.transform.position, player.transform.position), 2);
    }

    void CalculateAnimationSpeed()
    {
        foreach (Animator animator in animators)
        {
            animator.speed = currentSpeed / speedMax;
        }
    }

    void BurnCoal()
    {
        float burnedCoal = coalBurnRate * Time.deltaTime;
        if(coalInMachine - burnedCoal >= 0)
        {
            coalInMachine -= burnedCoal;
        }
        else
        {
            coalInMachine = 0;
        }
        float coalInMachineRate = coalInMachine / maxCoalInMachine * 100;
        float tempAccelerationRate = 0;
        if(coalInMachineRate >= 75)
        {
            tempAccelerationRate = 2f;
        }
        else if(coalInMachineRate >= 50 && coalInMachineRate < 75)
        {
            tempAccelerationRate = 1f;
        }
        else if (coalInMachineRate >= 25 && coalInMachineRate < 50)
        {
            tempAccelerationRate = -1f;
        }
        else if (coalInMachineRate < 25)
        {
            tempAccelerationRate = -2f;
        }
        RaiseTemperature(tempAccelerationRate);
    }
    void RaiseTemperature(float rate)
    {
        if(currentTemperature + rate * Time.deltaTime <= temperatureMax && currentTemperature + rate >= temperatureMin)
        {
            currentTemperature += rate * Time.deltaTime * 0.75f;
        }
        float tempRate = currentTemperature / temperatureMax * 100;
        float speedAccelerationRate = 0;
        if (tempRate >= 75)
        {
            speedAccelerationRate = 2f;
        }
        else if (tempRate >= 50 && tempRate < 75)
        {
            speedAccelerationRate = 1f;
        }
        else if (tempRate >= 25 && tempRate < 50)
        {
            speedAccelerationRate = -1f;
        }
        else if (tempRate < 25)
        {
            speedAccelerationRate = -2f;
        }
        CalculateSpeed(speedAccelerationRate);
    }

    void CalculateSpeed(float rate)
    {
        if (currentSpeed + rate * Time.deltaTime <= speedMax && currentSpeed + rate * Time.deltaTime >= 0)
        {
            currentSpeed += rate * Time.deltaTime * 0.5f;
        }
        else if(currentSpeed + rate * Time.deltaTime >= speedMax)
        {
            currentSpeed = speedMax;
        }
        else if (currentSpeed + rate * Time.deltaTime <= 0)
        {
            currentSpeed = 0;
        }
    }

    void SpawnCustomers()
    {
        List<GameObject> chosenSpawns = new List<GameObject>();
        List<GameObject> copySpawns = customerSpots;

        int count = Random.Range(minCustomers, maxCustomers);
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, copySpawns.Count);
            chosenSpawns.Add(copySpawns[index]);
            copySpawns.RemoveAt(index);
        }

        for (int i = 0; i < chosenSpawns.Count; i++)
        {
            GameObject c = Instantiate(customerPrefab, chosenSpawns[i].transform.position, Quaternion.identity);
            c.GetComponent<Customer>().SetGender();
            if (c.GetComponent<Customer>().GetGender())
            {
                c.GetComponent<Customer>().GetVoiceSource().clip = maleVoice[Random.Range(0, maleVoice.Length)];

            }
            else
            {
                c.GetComponent<Customer>().GetVoiceSource().clip = femaleVoice[Random.Range(0, femaleVoice.Length)];
            }
        }
    }

    void HandleMusicChanges()
    {
        float distanceTravelledAsPercentage = CalculateDistanceTravelledAsPercentage();
        if (distanceTravelled > 0 && distanceTravelledAsPercentage < 45) {
            backgroundMusicPlayer.requestClipChange(2);
        }
        else if (distanceTravelledAsPercentage >= 45 && distanceTravelledAsPercentage < 80) {
            backgroundMusicPlayer.requestClipChange(5);
        } 
        else if (distanceTravelledAsPercentage >= 80) {
            backgroundMusicPlayer.requestClipChange(6);
        }
    }
}
