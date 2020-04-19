using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    bool ticketBought = false;
    [SerializeField]
    GameObject popUpObject;
    [SerializeField]
    GameObject foodPopUp;
    [SerializeField]
    GameObject ticketPopUp;
    int ticketMoneyToPay;
    int foodMoneyToPay;
    bool foodBought;
    bool wantsFood = false;
    [SerializeField]
    [Range(0, 100)]
    int foodWantChance = 20;
    [SerializeField]
    float foodWantCheckIntervalMin = 5;
    [SerializeField]
    float foodWantCheckIntervalMax = 15;
    float foodWantCheckInterval;
    [SerializeField]
    AudioSource voiceSource;

    float canFood;

    Player player;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        foodWantCheckInterval = Random.Range(foodWantCheckIntervalMin, foodWantCheckIntervalMax);
        player = GameManager.instance.player.GetComponent<Player>();
        popUpObject.SetActive(false);
        ticketMoneyToPay = Random.Range(GameManager.instance.customerMoneyToPayMin, GameManager.instance.customerMoneyToPayMax);
        foodMoneyToPay = Random.Range(GameManager.instance.foodPriceMin, GameManager.instance.foodPriceMax);
        canFood = foodWantCheckInterval;
    }

    private void Update()
    {
        if (gameManager.GetGameState() != GameState.RUNNING) return;

        if(canFood < GameManager.instance.playTime)
        {
            foodWantCheckInterval = Random.Range(foodWantCheckIntervalMin, foodWantCheckIntervalMax);
            canFood += foodWantCheckInterval;
            int rand = Random.Range(0, 100);
            if(rand < foodWantChance && foodBought == false && wantsFood == false)
            {
                wantsFood = true;
                voiceSource.volume = 1/Mathf.Pow(Vector2.Distance(transform.position, GameManager.instance.player.transform.position), 2);
                if(voiceSource.volume <= 0.15)
                {
                    voiceSource.volume = 0;
                }
                voiceSource.Play();

            }
        }
    }

    public void BuyFood()
    {
        if (gameManager.GetGameState() != GameState.RUNNING) return;
        StartCoroutine(FoodBuying());
    }

    public IEnumerator FoodBuying()
    {
        foodBought = true;
        wantsFood = false;
        GameManager.instance.money += foodMoneyToPay;
        foodPopUp.SetActive(false);
        if (ticketBought == true)
        {
            popUpObject.SetActive(false);
        }
        player.inAction = true;
        player.ticketSource.clip = player.pencilClips[Random.Range(0, player.pencilClips.Count)];
        player.ticketSource.Play();
        yield return new WaitForSeconds(player.foodSellTime);
        player.ticketSource.clip = player.ripClips[Random.Range(0, player.ripClips.Count)];
        player.ticketSource.Play();
        player.inAction = false;
        voiceSource.volume = 1;
        GetComponent<AudioSource>().Play();
        voiceSource.Play();

    }

    public void BuyTicket()
    {
        if (gameManager.GetGameState() != GameState.RUNNING) return;
        StartCoroutine(TicketBuying());
    }

    public IEnumerator TicketBuying()
    {
        ticketBought = true;
        GameManager.instance.money += ticketMoneyToPay;
        ticketPopUp.SetActive(false);
        if (wantsFood == true && foodBought == false && GameManager.instance.foodCart.GetInUse())
        {
            popUpObject.SetActive(true); 
        }
        else
        {
            popUpObject.SetActive(false);
        }
        player.inAction = true;
        player.ticketSource.clip = player.pencilClips[Random.Range(0,player.pencilClips.Count)];
        player.ticketSource.Play();
        yield return new WaitForSeconds(player.ticketSellTime);
        player.ticketSource.clip = player.ripClips[Random.Range(0,player.ripClips.Count)];
        player.ticketSource.Play();
        player.inAction = false;
        voiceSource.volume = 1;
        GetComponent<AudioSource>().Play();
        voiceSource.Play();
    }

    public bool GetTicketBought()
    {
        return ticketBought;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(ticketBought == false || (foodBought == false && wantsFood == true && GameManager.instance.foodCart.GetInUse()))
            {
                popUpObject.SetActive(true);
                ticketPopUp.SetActive(!ticketBought);
                if (GameManager.instance.foodCart.GetInUse())
                {
                    foodPopUp.SetActive(wantsFood); 
                }
                else
                {
                    foodPopUp.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            popUpObject.SetActive(false);
            ticketPopUp.SetActive(!ticketBought);
            foodPopUp.SetActive(wantsFood);
        }
    }

    public AudioSource GetVoiceSource()
    {
        return voiceSource;
    }
}
