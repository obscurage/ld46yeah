using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartObject : MonoBehaviour
{
    [SerializeField]
    GameObject popUp;
    [SerializeField]
    Transform playerPosition;
    AudioSource audioSource;

    private GameManager gameManager;

    bool inUse = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
    }

    public void UseCart()
    {
        if (gameManager.GetGameState() == GameState.RUNNING)
        {
            popUp.SetActive(false);
            inUse = true;
            GameManager.instance.player.transform.position = new Vector2(playerPosition.position.x, GameManager.instance.player.transform.position.y);
        }
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
        if (gameManager.GetGameState() == GameState.RUNNING)
        {
            if (inUse && !GameManager.instance.player.GetComponent<Player>().inAction)
            {
                MoveCart();

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    inUse = false;
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    void MoveCart()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * GameManager.instance.player.GetComponent<Player>().currentSpeed * Time.deltaTime, 0);
        transform.Translate(movement);
        if (!audioSource.isPlaying && Input.GetAxis("Horizontal") != 0)
        {
            audioSource.Play(); 
        }
        else if(Input.GetAxis("Horizontal") == 0)
        {
            audioSource.Stop();
        }
    }
}
