﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources = new List<AudioSource>();

    private int currentListIndex = 0;
    private AudioSource currentAudioSource;

    private Queue<int> clipRequestQueue = new Queue<int>();
    private int lastRequestedClipIndex = -1;

    private List<bool> initialLoopInformation = new List<bool>();

    void Start()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            initialLoopInformation.Add(audioSource.loop);
        }
        currentAudioSource = audioSources[currentListIndex];
    }

    void Update()
    {
        // Automatically move forward if current clip is not playing
        if (!currentAudioSource.isPlaying)
        {
            int nextIndex = getNextClipIndex();
            Debug.Log("BackgroundMusicPlayer: Moving to clip: " + nextIndex);

            currentListIndex = nextIndex;
            MoveTo(currentListIndex);
        }
    }

    public void reset()
    {
        currentAudioSource.Stop();

        int i = 0;
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.loop = initialLoopInformation[i];
            i++;
        }

        currentListIndex = 0;
        currentAudioSource = audioSources[currentListIndex];
        clipRequestQueue = new Queue<int>();
        lastRequestedClipIndex = -1;
    }

    public bool requestClipChange(int index)
    {        
        // Already playing requested clip
        if (currentListIndex == index) return true;
        // Requested clip already in queue
        if (lastRequestedClipIndex == index) return true;

        Debug.Log("BackgroundMusicPlayer: Clip requested succesfully: " + index);

        lastRequestedClipIndex = index;
        clipRequestQueue.Enqueue(index);
        currentAudioSource.loop = false;

        return true;
    }

    private void MoveTo(int index)
    {
        currentListIndex = index;
        currentAudioSource = audioSources[currentListIndex];
        currentAudioSource.Play();

        // If there are clips waiting in queue play this only once even if loop
        if (clipRequestQueue.Count > 0) {
            currentAudioSource.loop = false;
        }
    }

    private int getNextClipIndex()
    {
        int nextIndex = currentListIndex + 1;
        if (clipRequestQueue.Count > 0)
        {
            nextIndex = clipRequestQueue.Dequeue();
        }
        return nextIndex;
    }
}
