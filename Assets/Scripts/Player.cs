using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    [HideInInspector]
    public float currentSpeed = 0;

    public float coalThrowTime = 1;
    public float foodSellTime = 1;
    public float ticketSellTime = 1;

    public bool inAction = false;

    [SerializeField]
    GameObject coalPopUp;

    AudioSource audioSource;
    public float footStepTimer = 0.2f;
    float canFootStep = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        PlayFootStepSound();
    }

    private void CalculateMovement()
    {
        if(inAction)
        {
            return;
        }
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

    private void PlayFootStepSound()
    {
        if (inAction)
        {
            return;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            if(Time.time > canFootStep)
            {
                audioSource.Play();
                canFootStep += footStepTimer * (2 - Mathf.Abs(Input.GetAxis("Horizontal")));
            }
        }
    }

    public void ThrowCoal()
    {
        StartCoroutine(CoalThrowing());
    }

    public IEnumerator CoalThrowing()
    {
        coalPopUp.SetActive(false);
        GameManager gm = GameManager.instance;
        inAction = true;
        print(coalThrowTime);
        yield return new WaitForSeconds(coalThrowTime);
        inAction = false;
        if (gm.coalLeft > 0)
        {
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
        coalPopUp.SetActive(true);
    }
}
