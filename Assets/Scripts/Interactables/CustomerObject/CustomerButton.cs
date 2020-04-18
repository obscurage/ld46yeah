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
        customer.BuyTicket();
    }
}
