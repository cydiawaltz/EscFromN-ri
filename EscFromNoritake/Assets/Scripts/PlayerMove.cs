/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");

        if (x > 0)
        {
            rigidBody.AddForce(transform.right * 8.0f);
        }
        else if (x < 0)
        {
            rigidBody.AddForce(-transform.right * 8.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Meat"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
*/
using UnityEngine;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 2f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x,-speed);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(rb.velocity.y,speed);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(rb.velocity.y,-speed);
        }
    }
}

