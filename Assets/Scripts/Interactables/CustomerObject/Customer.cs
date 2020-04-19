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
    float foodWantCheckInterval = 10;
    [SerializeField]
    AudioSource voiceSource;
    bool isMale = true;

    float canFood;

    Player player;

    private void Start()
    {
        player = GameManager.instance.player.GetComponent<Player>();
        popUpObject.SetActive(false);
        ticketMoneyToPay = Random.Range(GameManager.instance.customerMoneyToPayMin, GameManager.instance.customerMoneyToPayMax);
        foodMoneyToPay = Random.Range(GameManager.instance.foodPriceMin, GameManager.instance.foodPriceMax);
        canFood = foodWantCheckInterval;
    }

    private void Update()
    {
        if(canFood < GameManager.instance.playTime)
        {
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
        yield return new WaitForSeconds(player.foodSellTime);
        player.inAction = false;
        voiceSource.volume = 1;
        GetComponent<AudioSource>().Play();
        voiceSource.Play();

    }

    public void BuyTicket()
    {
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
        yield return new WaitForSeconds(player.ticketSellTime);
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

    public bool GetGender()
    {
        return isMale;
    }

    public void SetGender()
    {
        int gender = Random.Range(0, 2);
        if (gender == 0)
        {
            isMale = true;
        }
        else
        {
            isMale = false;
        }
    }
}
