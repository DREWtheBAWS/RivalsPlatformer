using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class grabScript : NetworkBehaviour
{
    public GameObject BaseballBatPref;
    public bool grabbed;
    public float distance = 1f;
    RaycastHit2D hit;
    public Transform holdPoint;
    public float throwForce;


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdGrabAndThrow();
        }

        //CmdIsGrabbed();

    }

    //[Command]
    //void CmdIsGrabbed()
    //{
    //    if (grabbed)
    //    {
            
    //        //hit.collider.gameObject.transform.position = holdPoint.position;

    //    }
    //}
    [Command]
    void CmdGrabAndThrow()
    {

        if (!grabbed)
        {

            Physics2D.queriesStartInColliders = false;
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.right * transform.localScale.x, distance);
            
            if (hit.collider != null && hit.collider.gameObject.CompareTag("BaseBallBat"))
            {
                
                
                grabbed = true;
                NetworkServer.Destroy(hit.collider.gameObject);
                Debug.Log(hit.collider.gameObject.tag);
            }

        }
        else
        {

            grabbed = false;
            GameObject instance = Instantiate(BaseballBatPref, holdPoint.position, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1);
            NetworkServer.Spawn(instance);
            


        }

    }

}