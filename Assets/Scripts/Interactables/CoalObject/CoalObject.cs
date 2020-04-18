using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalObject : MonoBehaviour
{
    [SerializeField]
    GameObject coalPopup;

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
}
