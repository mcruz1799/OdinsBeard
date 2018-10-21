using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    public float jumpDecrease = 0.5f;

    private PlayerAttackController attackController;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
    {
        attackController = GetComponent<PlayerAttackController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    protected override void ComputeVelocity()
    {
        // base.ComputeVelocity();
        Vector2 move = Vector2.zero;

        float hAxis = Input.GetAxis("Horizontal");

        if (hAxis < 0.2 && hAxis < -0.2)
        {
            /* hAxis = 0;

            // Keyboard.
            if (Input.GetKey("a"))
            {
                hAxis = -1;
            }
            else if (Input.GetKey("d"))
            {
                hAxis = 1;
            } */

            // Reset the player's vertical axis value to 0 if it's not beyond the deadbands
            /* if (!(vAxis > 0.2 || vAxis < -0.2))
            {
                vAxis = 0;
                // If using the keyboard, the axis value is 0, so manually set the axis values
                if (Input.GetKey("w"))
                {
                    vAxis = 1;
                }
                else if (Input.GetKey("s"))
                {
                    vAxis = -1;
                }
            } */
        }

        move.x = hAxis;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            // Reduce velocity if player stops holding jump.
            if (velocity.y > 0)
                velocity.y = velocity.y * jumpDecrease;
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x < 0) : (move.x > 0));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // Control movement animation here.

        targetVelocity = move * maxSpeed;
    }
}
