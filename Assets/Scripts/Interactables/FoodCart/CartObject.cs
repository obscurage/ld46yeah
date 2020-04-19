using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartObject : MonoBehaviour
{
    [SerializeField]
    GameObject popUp;
    [SerializeField]
    Transform playerPosition;

    bool inUse = false;

    public void UseCart()
    {
        popUp.SetActive(false);
        inUse = true;
        GameManager.instance.player.transform.position = new Vector2(playerPosition.position.x, GameManager.instance.player.transform.position.y);
    }

    public bool GetInUse()
    {
        return inUse;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            popUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popUp.SetActive(false);
        }
    }

    private void Update()
    {
        if(inUse && !GameManager.instance.player.GetComponent<Player>().inAction)
        {
            MoveCart();

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                inUse = false;
            }
        }

    }

    void MoveCart()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * GameManager.instance.player.GetComponent<Player>().currentSpeed * Time.deltaTime, 0);
        transform.Translate(movement);
    }
}
