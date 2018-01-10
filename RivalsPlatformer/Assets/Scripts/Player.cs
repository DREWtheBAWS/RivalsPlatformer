using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2d))]
public class Player : MonoBehaviour {

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex=.4f;
    public float accelerationTimeAirboarne=.2f;
    public float accelerationTimeGrounded=.1f;
    public float moveSpeed = .5f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXsmoothing;

    Controller2d controller;
    
    

    void Start () {
        controller = GetComponent<Controller2d>();

        gravity = -(2*maxJumpHeight) / Mathf.Pow(timeToJumpApex,2); // D=1/2gt^2  =>  Distance= 1/2 *gravity*time^2
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        Debug.Log("velocityMin" + minJumpVelocity);
        Debug.Log("velocityMax" + maxJumpVelocity);
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0; //we set the velocity back to 0 because we dont want the gravity force to accumulate
        }
        if (isLocalPlayer)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (velocity.y > minJumpVelocity)
                {

                    velocity.y = minJumpVelocity;
                    timeToJumpApex = timeToJumpApex / (maxJumpHeight / minJumpHeight);
                    Debug.Log("velocityApres" + velocity.y);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                Debug.Log("velocityFirst" + velocity.y);


                velocity.y = maxJumpVelocity;
                Debug.Log("velocityFirst" + velocity.y);

            }



            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirboarne); //Adds some acceleration to the vector. Makes it feel smooth.

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
	
	
}
