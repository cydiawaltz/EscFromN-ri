using UnityEngine;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public float speed;
    //以下画像（ドット絵）
    private Rigidbody2D rb;
    public GameObject Migi;
    public GameObject Hidari;
    public GameObject Ue;
    public GameObject Sita;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 2f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.position += new Vector2(speed, 0);
            Ue.SetActive(true);
            Hidari.SetActive(false);
            Migi.SetActive(false);
            Sita.SetActive(false);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            rb.position += new Vector2(-speed, 0);
            Ue.SetActive(false);
            Hidari.SetActive(false);
            Migi.SetActive(false);
            Sita.SetActive(true);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.position += new Vector2(0,speed);
            Migi.SetActive(true);
            Hidari.SetActive(false);
            Sita.SetActive(false):
            Ue.SetActive(false);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rb.position += new Vector2(0, -speed);
            Hidari.SetActive(true);
            Migi.SetActive(false);
            Sita.SetActive(false):
            Ue.SetActive(false);
        }
    }
}




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
