using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    bool ticketBought = false;
    [SerializeField]
    GameObject popUpObject;
    [SerializeField]
    GameObject foodPopUp;
    [SerializeField]
    GameObject ticketPopUp;
    int ticketMoneyToPay;
    int foodMoneyToPay;
    bool foodBought;
    bool wantsFood = false;
    [SerializeField]
    [Range(0, 100)]
    int foodWantChance = 20;
    [SerializeField]
    float foodWantCheckInterval = 10;
    [SerializeField]
    AudioSource voiceSource;
    bool isMale = true;

    float canFood;

    private void Start()
    {
        popUpObject.SetActive(false);
        ticketMoneyToPay = Random.Range(GameManager.instance.customerMoneyToPayMin, GameManager.instance.customerMoneyToPayMax);
        foodMoneyToPay = Random.Range(GameManager.instance.foodPriceMin, GameManager.instance.foodPriceMax);
        canFood = foodWantCheckInterval;
    }

    private void Update()
    {
        if(canFood < GameManager.instance.playTime)
        {
            canFood += foodWantCheckInterval;
            int rand = Random.Range(0, 100);
            if(rand < foodWantChance && foodBought == false)
            {
                wantsFood = true;

            }
        }
    }

    public void BuyFood()
    {
        foodBought = true;
        GameManager.instance.money += foodMoneyToPay;
        foodPopUp.SetActive(false);
        if (ticketBought == true)
        {
            popUpObject.SetActive(false); 
        }
        GetComponent<AudioSource>().Play();
        voiceSource.Play();

    }

    public void BuyTicket()
    {
        ticketBought = true;
        GameManager.instance.money += ticketMoneyToPay;
        ticketPopUp.SetActive(false);
        if (wantsFood == true && foodBought == false)
        {
            popUpObject.SetActive(false); 
        }
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
            if(ticketBought == false || foodBought == false)
            {
                popUpObject.SetActive(true);
                ticketPopUp.SetActive(!ticketBought);
                foodPopUp.SetActive(!foodBought);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            popUpObject.SetActive(false);
            ticketPopUp.SetActive(!ticketBought);
            foodPopUp.SetActive(!foodBought);
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
