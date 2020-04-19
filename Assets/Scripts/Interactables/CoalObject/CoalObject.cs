using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalObject : MonoBehaviour
{
    [SerializeField]
    GameObject coalPopup;
    public float startVolume = 0.2f;
    [SerializeField]
    AudioSource fireSource;
    [SerializeField]
    AudioSource emptySource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            coalPopup.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            coalPopup.SetActive(false);
        }
    }

    private void Update()
    {
        fireSource.volume = startVolume * (1/Mathf.Pow(Vector2.Distance(transform.position, GameManager.instance.player.transform.position), 2));
        fireSource.volume = startVolume * fireSource.volume * (GameManager.instance.currentTemperature / GameManager.instance.temperatureMax);
        if (fireSource.volume > startVolume)
        {
            fireSource.volume = startVolume;
        }
        emptySource.volume = startVolume * (1 / Mathf.Pow(Vector2.Distance(transform.position, GameManager.instance.player.transform.position), 2));
        if (emptySource.volume > startVolume)
        {
            emptySource.volume = startVolume;
        }

    }
}
