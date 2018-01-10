using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2d : NetworkBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float maxClimbAngle = 80;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider2d;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;


    void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    //Methode that makes the character move
    public void Move(Vector3 velocity)
    {
        collisions.Reset();
        UpdateRaycastOrigins();

        if (velocity.x != 0)
        {
            HorizontalCollision(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollision(ref velocity);
        }
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        transform.Translate(velocity);
    }
    //checks for collisions on the Y axis
    void VerticalCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;


        for (int rayIndex = 0; rayIndex < verticalRayCount; rayIndex++)
        {
            Vector2 rayOrigin;
            if (directionY == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.topLeft;
            }
            rayOrigin += Vector2.right * (verticalRaySpacing * rayIndex + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);//direction is set to UP because down is negative.

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (rayIndex == verticalRayCount-1 && slopeAngle < maxClimbAngle)
                {
                    float distanceToSlope = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                       
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.y -= distanceToSlope * directionY;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.y += distanceToSlope * directionY;
                }
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;


                    if (collisions.climbingSlope)
                    {
                        //velocity.x = Mathf.Abs(velocity.y) / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad);
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    //sets the collision at true depending on the direction
                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }
        }
    }
    //Checks for collisions on the X axis
    void HorizontalCollision(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;


        for (int rayIndex = 0; rayIndex < horizontalRayCount; rayIndex++)
        {
            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.bottomRight;
            }
            rayOrigin += Vector2.up * (horizontalRaySpacing * rayIndex);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);//direction is set to right because left is negative.

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (rayIndex == 0 && slopeAngle < maxClimbAngle)
                {
                    float distanceToSlope = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        Debug.Log(slopeAngle);
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.x -= distanceToSlope * directionX;
                        velocity.y -= distanceToSlope * -1;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x+= distanceToSlope * directionX;
                    velocity.y += distanceToSlope * -1;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    velocity.y = (hit.distance - skinWidth) * -1;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    //sets the collision at true depending on the direction
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY= (float)Math.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

      
        if (velocity.y <= climbVelocityY)
        {
            collisions.climbingSlope = true;
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.slopeAngle = slopeAngle;
        }
        
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxClimbAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    //Moves the rayOrigins with the player
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    //Calculates the space between the rays. NOT USED YET
    void CalculateRaySpacing()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    //Ray origins
    struct RaycastOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
    //Variables related to the collision
    public struct CollisionInfo
    {
        public bool above;
        public bool below;
        public bool left;
        public bool right;

        public bool climbingSlope, descendingSlope;
        public float slopeAngle, slopeAngleOld;
        //Method that resets the collisions
        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
    }
    }
	
	
}
