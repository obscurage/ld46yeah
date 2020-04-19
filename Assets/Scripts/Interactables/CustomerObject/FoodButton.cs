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
        if (GameManager.instance.player.GetComponent<Player>().inAction)
        {
            return;
        }
        customer.BuyFood();
    }
}
