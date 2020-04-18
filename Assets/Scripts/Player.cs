using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    [HideInInspector]
    public float currentSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;   
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float direction = Input.GetAxis("Horizontal");
        if (GameManager.instance.foodCart.GetInUse())
        {
            currentSpeed = speed * GameManager.instance.cartMultiplier;
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            if (direction < 0) { 
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else if (direction > 0) { 
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }

            currentSpeed = speed;
        }
        Vector2 movement = new Vector2(direction * currentSpeed * Time.deltaTime, 0);
        transform.Translate(movement);
    }
}
