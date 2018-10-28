using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementScript : MonoBehaviour
{
    private Vector2 velocity;

    private Rigidbody2D rb;
    private Collider2D collider;
    private SpriteRenderer sp;

    private Animator animator;

    public GameObject mainCam;

    public int hp = 3;
    public float invulnTime = 2.0f;

    // Set basic speed variables
    public float playerSpeed = 7.0f;
    public float swordSpeed = 200.0f;

    // Set time sword is held after being swung
    public float holdTime = 1.5f;

    // Set jump variables
    public float maxJumpTime = 0.25f;
    private float currentJumpTime;
    public float gracePeriod = 0.5f;
    private bool isJumping;
    private bool isFalling;

    private float baseGrav;

    // Create distance, time, direction, attacking, and sword transform variables
    private float distance;
    private float swordTimer;
    private bool isFacingRight;
    private bool attacking;
    private bool holding;
    private Transform swordBox;

    // Create variable to hold the rotation direction
    private Vector3 attackDir;

    // Create a variable to hold the collider of the sword
    Collider2D swordCollider;

    // Create variables to track the current sword and target angles
    private float swordAngle;
    private int targetAngle;

    // Create variables to hold the original position and rotation of the sword
    private Vector3 originalPos;
    private Quaternion originalRot;

    // (0, 0, 0)
    Vector2 curVel = Vector2.zero;

    // Use this for initialization
    void Start()
    {

        // These *MIGHT* be helpful later
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sp = GetComponent <SpriteRenderer>();

        animator = GetComponent<Animator>();

        mainCam = GameObject.Find("Main Camera");

        // Grab sword's hitbox (empty GameObject)
        swordBox = transform.Find("Sword Box");
        originalPos = swordBox.transform.localPosition;
        originalRot = swordBox.transform.rotation;

        // Initialize the sword box's collider and disable it
        swordCollider = swordBox.GetComponent<Collider2D>();
        swordCollider.enabled = false;

        // Reset swordTimer and variables
        swordTimer = 0.0f;
        currentJumpTime = 0.0f;
        isFacingRight = true;
        attacking = false;

        // Initialize jump variables
        currentJumpTime = 0.0f;
        isJumping = false;

        rb.gravityScale = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float pX = transform.position.x;
        float pY = transform.position.y;

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(pX, -16.6f, Mathf.Infinity), 
            pY + 1.48f, -10.0f);

        // If the player presses Enter, start attacking
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Set the collider to be enabled if it isn't already
            if (!attacking && !holding)
            {
                swordCollider.enabled = true;
                attacking = true;
            }
        }

        // If the sword collider is enabled, rotate the sprite
        if (attacking)
        {
            // Get the sword angle and check
            swordAngle = swordBox.rotation.eulerAngles.z;

            // Set target angle and attack direction depending on player position
            if (isFacingRight)
            {
                targetAngle = 285;
                attackDir = -Vector3.forward;
                // This works fairly well and the game doesn't work without it for some reason
                if (swordAngle <= targetAngle)
                {
                    swordAngle = 360.0f - swordAngle;
                }
            }
            else
            {
                targetAngle = 75;
                attackDir = Vector3.forward;
            }

            // Rotate the sword and increase the base speed (giving "weight")
            swordBox.RotateAround(transform.position, attackDir,
                                   swordSpeed * Time.deltaTime);
            swordSpeed += 0.05f * swordSpeed;

            // Say the player is now holding the sword straight
            if ((isFacingRight && (int)swordAngle <= targetAngle) ||
                (!isFacingRight && (int)swordAngle >= targetAngle))
            {
                attacking = false;
                holding = true;
            }
        }

        // Hold the sword as long as directed
        if (holding)
        {
            // If the player has held the sword for long enough, reset the sword attributes
            if (swordTimer >= holdTime)
            {
                ResetSword();
            }
            // Increase the swordTimer
            swordTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Grab the axis value
        float hAxis = Input.GetAxis("Horizontal");

        if (hAxis != 0)
        {
            // Flip sprites and direction
            if (hAxis < 0 && !attacking)
            {
                isFacingRight = false;
                sp.flipX = false;
            }
            if (hAxis > 0 && !attacking)
            {
                isFacingRight = true;
                sp.flipX = true;
            }
            animator.SetTrigger("PlayerWalk");
        } else if (animator.GetComponent<Animator>().GetBool("PlayerWalk")) {
            // End animation
            animator.Rebind();
        }

        float lift = 0.0f;

        if (Input.GetKey("space") && currentJumpTime < maxJumpTime) {
            rb.gravityScale = 1.0f;
            lift = 10.0f;
            isJumping = true;
            currentJumpTime += Time.deltaTime;
            animator.SetTrigger("PlayerJump");
        } else if (isJumping && !isFalling) {
            animator.Rebind();
            rb.gravityScale = 0.0f;
            isFalling = true;
            animator.SetTrigger("PlayerFall");
        }
        else if (isFalling) {
            rb.gravityScale = 20.0f;
            lift = 0.0f;
        }

        /*if (Input.GetKeyUp("space")) {
            isFalling = true;
        }*/

        Vector2 newVelocity = new Vector2(hAxis * playerSpeed, lift);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref curVel, 0.05f);
    }

    // Resets the sword's properties
    private void ResetSword(){
        swordCollider.enabled = false;
        swordBox.transform.localPosition = originalPos;
        swordBox.transform.rotation = originalRot;
        swordSpeed = 200.0f;
        holding = false;
        swordTimer = 0.0f;
    }

    /*
    private bool isGrounded() {
        return Physics2D.Raycast(, Vector3.down, collider.bounds.extents.y);
    }
    */

    private void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.name == "Tilemap")
        {
            isJumping = false;
            isFalling = false;
        }

    }

    private float DistanceToGround() {
        return 0;
    }
}
