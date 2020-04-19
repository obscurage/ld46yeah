using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerButton : MonoBehaviour
{
    [SerializeField]
    Customer customer;
    public void PressButton() {
        SellTicket();
    }

    void SellTicket()
    {
        if(GameManager.instance.player.GetComponent<Player>().inAction)
        {
            return;
        }
        customer.BuyTicket();
    }
}
