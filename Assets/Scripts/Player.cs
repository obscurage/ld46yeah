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

    public AudioSource ticketSource;
    public List<AudioClip> pencilClips = new List<AudioClip>();
    public List<AudioClip> ripClips = new List<AudioClip>();
    [HideInInspector]
    public Animator anim;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == GameState.RUNNING)
        {
            CalculateMovement();
            PlayFootStepSound();
        }
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
                anim.Play("Konnari_Walk");

            }
            return;
        }
        anim.Play("Konnari");
    }

    public void ThrowCoal()
    {
        if (gameManager.GetGameState() == GameState.RUNNING)
        {
            StartCoroutine(CoalThrowing());
        }
    }

    public IEnumerator CoalThrowing()
    {
        coalPopUp.SetActive(false);
        anim.Play("Konnari_Shovel");
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
