using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public int hp;
    public int lives;

    private SpriteRenderer sp;
    private Animator animator;

    public GameObject mainCam;

    private bool isFacingRight;
    private bool spriteCanChange;

    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    public float jumpDecrease = 0.5f;

    private float hAxis;

    private PlayerAttackController attackController;

	// Use this for initialization
	void Start ()
    {
        hp = 3;
        lives = 3;

        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        mainCam = GameObject.Find("Main Camera");

        attackController = GetComponent<PlayerAttackController>();
        
        isFacingRight = true;
        spriteCanChange = true;
	}


    // Tap Jump activates falling animation but player doesn't immediately break out of it
    // 
    // 

    protected override void ComputeVelocity()
    {
        // base.ComputeVelocity();
        Vector2 move = Vector2.zero;

        hAxis = Input.GetAxis("Horizontal");
        
        float pX = transform.position.x;
        float pY = transform.position.y;

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(pX, -16.6f, Mathf.Infinity),
            pY + 1.48f, -10.0f);

        if (hAxis != 0)
        {
            // Flip sprites and direction
            if (hAxis < 0 && !attackController.attacking && spriteCanChange)
            {
                isFacingRight = false;
            }
            if (hAxis > 0 && !attackController.attacking && spriteCanChange)
            {
                isFacingRight = true;
            }

            if (isFacingRight)
            {
                sp.flipX = true;
            }
            else
            {
                sp.flipX = false;
            }

            if (grounded) {
                animator.SetTrigger("PlayerWalk");
            }
        }
        else if (animator.GetComponent<Animator>().GetBool("PlayerWalk"))
        {
            // End animation
            animator.Rebind();
        }
        else if (grounded) {
            animator.Rebind();
        }

        if (!attackController.attacking && !attackController.holding)
        {
            spriteCanChange = true;
        }
        else
        {
            spriteCanChange = false;
        }

        if (hAxis < 0.2 && hAxis < -0.2)
        {
            // deadband logic
        }

        move.x = hAxis;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            animator.Rebind();
            animator.SetTrigger("PlayerJump");
        }
        else if (Input.GetButtonUp("Jump"))
        {
            // Reduce velocity if player stops holding jump.
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * jumpDecrease;
                animator.SetTrigger("PlayerFall");
            }
        }
        else if (Input.GetButton("Jump") && !grounded && velocity.y <= jumpTakeOffSpeed * jumpDecrease) {
            animator.SetTrigger("PlayerFall");
        }

        targetVelocity = move * maxSpeed;
    }

    public void Heal() {
        if (hp < 3) {
            hp++;
        }
    }

    // Does nothing
    public void TakeDamage() {

        hp--;
        if (hp == 0) {
            lives--;
        }

        if (lives == 0) {
            GameOver();
        }
    }

    // Does nothing
    public void GameOver() {

    }
}
