using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //You cna use GameManagers variables etc in any script with GameManager.instance.myVariable/myFunction() 
    public static GameManager instance = null;

    public float startCoal = 100;
    public float coalLeft;
    public float coalInMachine;

    public float maxCoalInMachine = 20;
    public float currentSpeed;
    public float speedMax = 15;
    public float speedMin = 0;
    public float currentTemperature = 0;
    public float temperatureMax = 100;
    public float temperatureMin = 0;

    //How much coal is used in one interaction
    public float coalPerThrow = 3;

    public float coalBurnRate = 1;

    public int money;
    public int customerMoneyToPayMax = 50;
    public int customerMoneyToPayMin = 10;

    [SerializeField]
    AudioSource audioSource;

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
        coalLeft = startCoal;

        var pitchBendGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("BackgroundMixer");
        audioSource.outputAudioMixerGroup = pitchBendGroup;
        audioSource.pitch = 2f;
        pitchBendGroup.audioMixer.SetFloat("PitchBend", 1f / 2f);
    }

    private void Update()
    {
        BurnCoal();
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
            tempAccelerationRate = 1.5f;
        }
        else if(coalInMachineRate >= 50 && coalInMachineRate < 75)
        {
            tempAccelerationRate = 0.5f;
        }
        else if (coalInMachineRate >= 25 && coalInMachineRate < 50)
        {
            tempAccelerationRate = -0.5f;
        }
        else if (coalInMachineRate < 25)
        {
            tempAccelerationRate = -1.5f;
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
            speedAccelerationRate = 1.5f;
        }
        else if (tempRate >= 50 && tempRate < 75)
        {
            speedAccelerationRate = 0.5f;
        }
        else if (tempRate >= 25 && tempRate < 50)
        {
            speedAccelerationRate = -0.5f;
        }
        else if (tempRate < 25)
        {
            speedAccelerationRate = -1.5f;
        }
        CalculateSpeed(speedAccelerationRate);
    }

    void CalculateSpeed(float rate)
    {
        if (currentSpeed + rate * Time.deltaTime <= speedMax && currentSpeed + rate >= speedMin)
        {
            currentSpeed += rate * Time.deltaTime * 0.5f;
        }
    }
}
