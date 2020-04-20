using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //You can use GameManagers variables etc in any script with GameManager.instance.myVariable/myFunction() 
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
    public List<GameObject> customerPrefabs = new List<GameObject>();
    public List<GameObject> customerSpots = new List<GameObject>();
    public int minCustomers = 10;
    public int maxCustomers = 20;
    private float startTime;

    [Tooltip("Distance in km")]
    public float totalDistance = 5;
    [HideInInspector]
    public float distanceTravelled = 0;
    public float backgroundSpeedMultiplier = 2;

    [SerializeField]
    AudioSource audioSource;
    [HideInInspector]
    public AudioClip[] maleVoice;
    [HideInInspector]
    public AudioClip[] femaleVoice;

    [SerializeField]
    AudioMixerGroup pitchBendGroup;

    public float playTime;
    public List<Animator> animators = new List<Animator>();

    private GameState gameState = GameState.NOT_STARTED;
    private bool gameInitiallyStarted = false;

    [SerializeField] BackgroundMusicPlayer backgroundMusicPlayer;
    [SerializeField] GameObject startMenu;

    public TMP_Text cashText;
    public TMP_Text buttonText;
    public TMP_Text uiTitle;
    public TMP_Text coalText;

    private Fader fader;
    [SerializeField]
    TMP_Text scoreText;
    float timeMultiplier;
    public int soldTickets;
    int totalCustomers;

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

    private IEnumerator Start()
    {
        fader = FindObjectOfType<Fader>();
        fader.FadeInImmediate();

        startMenu.SetActive(true);
        backgroundMusicPlayer.requestClipChange(1);

        maleVoice = Resources.LoadAll<AudioClip>("CharactersVoices/Male");
        femaleVoice = Resources.LoadAll<AudioClip>("CharactersVoices/Female");
        coalLeft = startCoal;
        audioSource.outputAudioMixerGroup = pitchBendGroup;
        CalculateTempo();

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
        scoreText.gameObject.SetActive(false);

        yield return StartCoroutine(fader.FadeOut());
    }

    private void Update()
    {
        // Stuff that will be run regardless of game state
        HandleMusicChanges();
        HandleInputs();

        if (gameState == GameState.PAUSE || gameState == GameState.NOT_STARTED) return;
        if (gameState == GameState.WON)
        {
            gameInitiallyStarted = false;
            SetWonScreen();
        }
        else if (gameState == GameState.LOST)
        {
            gameInitiallyStarted = false;
            SetLostScreen();
        }
        else
        {
            // Stuff that should be run only if game is not on pause, won or lost.
            BurnCoal();
            playTime = Time.time - startTime;
            CalculateTempo();
            CalculateDistance();
            CalculateAnimationSpeed();
        }
        cashText.text = money.ToString();
        coalText.text = $"{Mathf.Round(coalInMachine)}/{maxCoalInMachine}";
    }

    public void TogglePause()
    {
        if (gameState == GameState.LOST || gameState == GameState.WON || gameState == GameState.NOT_STARTED) return;
        if (gameState == GameState.PAUSE)
        {
            gameState = GameState.RUNNING;
            startMenu.SetActive(false);
        }
        else {
            gameState = GameState.PAUSE;
            startMenu.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (!gameInitiallyStarted)
        {
            // May seem little unclear what this does but basically this prevents resetting
            // game state when first time starting the game.
            if (gameState != GameState.NOT_STARTED)
            {
                StartCoroutine(ResetGameState());
            }
            uiTitle.text = "TRAIN GAEM (PAUSED)";
            buttonText.text = "Continue";

            gameInitiallyStarted = true;
            gameState = GameState.RUNNING;
        } 
        else
        {
            TogglePause();
        }
    }

    public GameState GetGameState()
    {
        return gameState;
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
            gameState = GameState.WON;
        }
    }

    float GetDistanceTravelledAsPercentage()
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
        audioSource.volume = Mathf.Clamp(audioSource.volume, 0, 0.35f);
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
            gameState = GameState.LOST;
        }
    }

    void SpawnCustomers()
    {
        List<GameObject> chosenSpawns = new List<GameObject>();
        List<GameObject> copySpawns = customerSpots;

        int count = Random.Range(minCustomers, maxCustomers);
        totalCustomers = count;
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, copySpawns.Count);
            chosenSpawns.Add(copySpawns[index]);
            copySpawns.RemoveAt(index);
        }

        for (int i = 0; i < chosenSpawns.Count; i++)
        {
            GameObject c = Instantiate(customerPrefabs[Random.Range(0,customerPrefabs.Count)], chosenSpawns[i].transform.position, Quaternion.identity);
            if (c.name.Contains("F"))
            {
                c.GetComponent<Customer>().GetVoiceSource().clip = femaleVoice[Random.Range(0, femaleVoice.Length)];

            }
            else
            {
                c.GetComponent<Customer>().GetVoiceSource().clip = maleVoice[Random.Range(0, maleVoice.Length)];
            }
        }
    }

    void HandleMusicChanges()
    {
        float distanceTravelledAsPercentage = GetDistanceTravelledAsPercentage();
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

    void HandleInputs()
    {
        bool pausePressed = Input.GetButtonDown("Cancel");
        if (pausePressed)
        {
            TogglePause();
        }
    }

    private IEnumerator ResetGameState()
    {
        yield return StartCoroutine(fader.FadeIn());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetWonScreen()
    {
        startMenu.SetActive(true);
        uiTitle.text = "YOU WON!";
        buttonText.text = "Again";
        CalculateTimeMultiplier();
        ShowScore();
    }

    private void SetLostScreen()
    {
        startMenu.SetActive(true);
        uiTitle.text = "YOU LOST!";
        buttonText.text = "Again";
        timeMultiplier = 1;
        ShowScore();
    }

    void ShowScore()
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"Score: {money * timeMultiplier} \nTickets sold: {soldTickets}/{totalCustomers} \nDistance traveled: {distanceTravelled}km";
    }

    void CalculateTimeMultiplier()
    {
        if(totalDistance / (playTime / 3600) > speedMax / 2)
        {
            timeMultiplier = 2;
        }
        else
        {
            timeMultiplier = 1.5f;
        }
    }

}

public enum GameState
{
    NOT_STARTED, PAUSE, RUNNING, WON, LOST
}
