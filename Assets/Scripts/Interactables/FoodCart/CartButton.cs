using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartButton : MonoBehaviour
{
    public CartObject cartObject;
    public void PressButton()
    {
        cartObject.UseCart();
    }
}
