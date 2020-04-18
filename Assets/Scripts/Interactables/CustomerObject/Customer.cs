using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    bool ticketBought = false;
    [SerializeField]
    GameObject popUpObject;
    int moneyToPay;

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
}
