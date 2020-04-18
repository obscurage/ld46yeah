﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public float speed = 8f;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        
        //float dTime = 
        float temp = transform.position.x - ((1 - parallaxEffect) * Time.deltaTime * speed);
        float dist = transform.position.x - (parallaxEffect * Time.deltaTime * speed);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) transform.position = new Vector3(startpos, transform.position.y, transform.position.z);
        else if (temp < startpos - length) transform.position = new Vector3(-startpos, transform.position.y, transform.position.z); ;
    }
}