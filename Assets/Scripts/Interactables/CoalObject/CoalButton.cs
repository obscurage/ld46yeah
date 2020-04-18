using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalButton : MonoBehaviour
{
    public void PressButton()
    {
        ThrowCoal();
    }

    void ThrowCoal()
    {
        GameManager gm = GameManager.instance;
        if (gm.coalLeft > 0)
        {
            print("hiiltä lapattu");
            float coalThrew;
            if (gm.coalLeft >= gm.coalPerThrow)
            {
                gm.coalLeft -= gm.coalPerThrow;
                coalThrew = gm.coalPerThrow;
            }
            else
            {
                coalThrew = gm.coalLeft;
                gm.coalLeft = 0;
            }

            if (coalThrew + gm.coalInMachine < gm.maxCoalInMachine)
            {
                gm.coalInMachine += coalThrew;
            }
            else
            {
                gm.coalInMachine = gm.maxCoalInMachine;
            }
        }
        print("ei hiiltä");
    }
}
