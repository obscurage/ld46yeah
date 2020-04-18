using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    [SerializeField]
    Customer customer;
    public void PressButton()
    {
        SellFood();
    }

    void SellFood()
    {
        customer.BuyFood();
    }
}
