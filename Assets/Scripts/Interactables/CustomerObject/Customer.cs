using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    bool ticketBought = false;
    [SerializeField]
    GameObject popUpObject;
    int moneyToPay;
    bool wantsFood = false;
    [SerializeField]
    [Range(0, 100)]
    int foodWantChance = 20;
    [SerializeField]
    AudioSource voiceSource;
    bool isMale = true;

    private void Start()
    {
        popUpObject.SetActive(false);
        moneyToPay = Random.Range(GameManager.instance.customerMoneyToPayMin, GameManager.instance.customerMoneyToPayMax);
    }

    public void BuyTicket()
    {
        ticketBought = true;
        GameManager.instance.money += moneyToPay;
        popUpObject.SetActive(!ticketBought);
        GetComponent<AudioSource>().Play();
        voiceSource.Play();
    }

    public bool GetTicketBought()
    {
        return ticketBought;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            popUpObject.SetActive(!ticketBought);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            popUpObject.SetActive(false);
        }
    }

    public AudioSource GetVoiceSource()
    {
        return voiceSource;
    }

    public bool GetGender()
    {
        return isMale;
    }

    public void SetGender()
    {
        int gender = Random.Range(0, 2);
        print(gender);
        if (gender == 0)
        {
            isMale = true;
        }
        else
        {
            isMale = false;
        }
    }
}
