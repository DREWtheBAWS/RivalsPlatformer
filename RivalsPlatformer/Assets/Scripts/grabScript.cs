using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class grabScript :NetworkBehaviour  {

    public bool grabbed;
    public float distance=1f;
    RaycastHit2D hit;
    public Transform holdPoint;
    public float throwForce;
	
	
	// Update is called once per frame
	void Update () {

      
        
        CmdGrabAndThrow();
	}
    [Command]
    void CmdGrabAndThrow()
    {
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(grabbed);
           
            if (!grabbed)
            {
                
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.right * transform.localScale.x, distance);

                if (hit.collider != null)
                {
                    grabbed = true;
                }


            }
            else
            {
                grabbed = false;
                if (hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity != null)
                {
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1);
                }
            }
        }

        if (grabbed)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                grabbed = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1);
            }
            else
            {
                hit.collider.gameObject.transform.position = holdPoint.position;
            }


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }


}
