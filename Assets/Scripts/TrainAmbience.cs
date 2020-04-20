using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAmbience : MonoBehaviour
{
    bool followPlayer = false;
    private Vector2 bounds;
    AudioSource audioSource;
    float maxAudioLevel = 0.45f;
    void Start()
    {
        bounds = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(transform.position.x <= bounds.x)
        {
            transform.position = GameManager.instance.player.transform.position;
        }
        if(transform.position.x > bounds.x)
        {
            transform.position = bounds;
        }
        audioSource.volume = 1 / Mathf.Pow(Vector2.Distance(transform.position, GameManager.instance.player.transform.position),2);
        if(audioSource.volume < 0.2)
        {
            audioSource.volume = 0;
        }
        GameManager gm = GameManager.instance;
        if(gm.currentSpeed/gm.speedMax < 0.3)
        {
            audioSource.volume = audioSource.volume * gm.currentSpeed / gm.speedMax;
        }

        audioSource.volume = Mathf.Clamp(audioSource.volume, 0, maxAudioLevel);
    }
}
