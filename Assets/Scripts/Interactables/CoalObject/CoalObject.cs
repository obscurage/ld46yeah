using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalObject : MonoBehaviour
{
    [SerializeField]
    GameObject coalPopup;
    public float startVolume = 0.2f;

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
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = (1/Mathf.Pow(Vector2.Distance(transform.position, GameManager.instance.player.transform.position), 2)) - (1 - startVolume);
        if(audio.volume > startVolume)
        {
            audio.volume = startVolume;
        }
    }
}
