using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);
        transform.Translate(movement);
    }
}
