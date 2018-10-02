using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    // These *MIGHT* be helpful later
    // private Rigidbody rb;
    // private Collider collider;

    // Set basic speed variables
    public float playerSpeed = 0.12f;
    public float swordSpeed = 200.0f;

    // Set time sword is held after being swung
    public float holdTime = 1.5f;

    // Create distance, time, direction, attacking, and sword transform variables
    private float distance;
    private float timer;
    private bool isFacingRight;
    private bool attacking;
    private bool holding;
    private Transform swordBox;

    // Create variable to hold the rotation direction
    private Vector3 attackDir;

    // Create a variable to hold the collider of the sword
    BoxCollider2D swordCollider;

    // Create variables to track the current sword and target angles
    private float swordAngle;
    private int targetAngle;

    // Create variables to hold the original position and rotation of the sword
    private Vector3 originalPos;
    private Quaternion originalRot;

    // Use this for initialization
    void Start()
    {

        // These *MIGHT* be helpful later
        // rb = GetComponent<Rigidbody>();
        // collider = GetComponent<Collider>();

        // Grab sword's hitbox (empty GameObject)
        swordBox = transform.Find("Sword Box");
        originalPos = swordBox.transform.localPosition;
        originalRot = swordBox.transform.rotation;

        // Initialize the sword box's collider and disable it
        swordCollider = swordBox.GetComponent<BoxCollider2D>();
        swordCollider.enabled = false;

        // Reset timer and variables
        timer = 0.0f;
        isFacingRight = true;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {

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
            if (timer >= holdTime)
            {
                ResetSword();
            }
            // Increase the timer
            timer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {

        // Grab the axis values
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // Reset the player's horizontal axis value to 0 if it's not beyond the deadbands
        if (!(hAxis > 0.2 || hAxis < -0.2))
        {
            hAxis = 0;
            // If using the keyboard, the axis value is 0, so manually set the axis values
            if (Input.GetKey("a"))
            {
                hAxis = -1;
                if (attacking == false)
                {
                    isFacingRight = false;
                }
            }
            else if (Input.GetKey("d"))
            {
                hAxis = 1;
                if (attacking == false)
                {
                    isFacingRight = true;
                }
            }
        }

        // Reset the player's vertical axis value to 0 if it's not beyond the deadbands
        if (!(vAxis > 0.2 || vAxis < -0.2))
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
        }

        // Get the player's current position
        Vector3 playerPos = transform.position;

        // Move the player's horizontal position relatively smoothly
        float xPos = playerPos.x + (hAxis * playerSpeed);
        // This part should change with Nathan's jumping logic
        float yPos = playerPos.y;

        // Create the player's new position
        playerPos = new Vector3(xPos, yPos, 0.0f);

        // Set the player's position to be the new generated position
        transform.position = playerPos;
    }

    // Resets the sword's properties
    private void ResetSword(){
        swordCollider.enabled = false;
        swordBox.transform.localPosition = originalPos;
        swordBox.transform.rotation = originalRot;
        swordSpeed = 200.0f;
        holding = false;
        timer = 0.0f;
    }
}
