using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleCombat : NetworkBehaviour {
    public Collider2D attackCollider;
    bool isCollided;
    const float velocityX=10;
    const float velocityY = 10;

    // Use this for initialization
    void Start () {
        isCollided = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.K) && isCollided)
        {
            Attack();
            Debug.Log("Attack");
            return;
        }
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            isCollided = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            isCollided = false;
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.tag == ("Player"))
    //    {
    //        isCollided = true;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.collider.tag == ("Player"))
    //    {
    //        isCollided = false;
    //    }
    //}

    void Attack()
    {
        Debug.Log("J'attack foreal");
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Attack to the right");
            GetComponent<Collider>().gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(transform.localScale.x*velocityX, 0);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Attack under");
            Vector2 vect = new Vector2(0,velocityY);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Attack to the left");
            Vector2 vect = new Vector2(velocityX*-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Attack over");
            Vector2 vect = new Vector2(velocityX * -1, 0);
        }
    }



}
