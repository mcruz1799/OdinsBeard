using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;
    public float minGroundNormalY = 0.65f;  // Normal at which slopes are considered standable.

    // Allow child classes to modify these.
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 targetVelocity;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable ()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // Use the layer collisions defined in Unity for the current GameObject.
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
	}

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    // Function to be inherited.
    protected virtual void ComputeVelocity() { }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;  // Assume until grounded.

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); // Perpendicular vector.
        
        // Horizontal movement.
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);

        // Vertical movement.
        move = Vector2.up * deltaPosition.y;  // Physics2D.gravity is negative.
        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            // Get a list of all collisions 
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;  // Object will land on ground. 
                    if (yMovement)
                    {

                        // Prevent all movement along the x axis.
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);

                // Current speed and the wall are in collision.
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;  // Cancel velocity stopped by collision.
                }

                // Get canonical distance.
                float modifiedDistance = hitBufferList[i].distance - shellRadius;

                // Find the smallest distance to move.
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
