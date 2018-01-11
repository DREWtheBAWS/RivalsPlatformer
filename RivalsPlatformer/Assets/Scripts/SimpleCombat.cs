using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCombat : MonoBehaviour {
    public Collider2D attackCollider;
    bool isCollided;
    
	// Use this for initialization
	void Start () {
        isCollided = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.K) && isCollided)
        {
            //Attack();
            Debug.Log("Attack");
            
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

    }



}
