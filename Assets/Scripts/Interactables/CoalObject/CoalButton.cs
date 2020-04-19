using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalButton : MonoBehaviour
{
    public void PressButton()
    {
        if(GameManager.instance.player.GetComponent<Player>().inAction)
        {
            return;
        }
        GameManager.instance.player.GetComponent<Player>().ThrowCoal();
    }
}
