using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float length, startpos;
    public float parallaxEffect;
    private float speed;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        speed = GameManager.instance.currentSpeed * GameManager.instance.backgroundSpeedMultiplier / GameManager.instance.speedMax;
        //speed = 1;
        float temp = transform.position.x - ((1 - parallaxEffect) * Time.deltaTime * speed);
        float dist = transform.position.x - (parallaxEffect * Time.deltaTime * speed);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) transform.position = new Vector3(startpos, transform.position.y, transform.position.z);
        else if (temp < startpos - length) transform.position = new Vector3(-startpos, transform.position.y, transform.position.z); ;
    }
}
