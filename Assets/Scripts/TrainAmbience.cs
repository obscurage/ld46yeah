using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAmbience : MonoBehaviour
{
    bool followPlayer = false;
    private Vector2 bounds;
    AudioSource audioSource;
    void Start()
    {
        bounds = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer && transform.position.x <= bounds.x)
        {
            transform.Translate(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime,0));
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            followPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            followPlayer = false;
        }
    }
}
