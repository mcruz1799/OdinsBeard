using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // Set time sword is held after being swung
    public float holdTime = 1.5f;
    public float swordSpeed = 200.0f;

    public bool attacking;
    public bool holding;

    // Create distance, time, direction, attacking, and sword transform variables
    private float distance;
    private float timer;
    private Transform swordBox;

    private SpriteRenderer spriteRenderer;

    // Create variable to hold the rotation direction
    private Vector3 attackDir;

    // Create a variable to hold the collider of the sword
    private BoxCollider2D swordCollider;

    // Create variables to track the current sword and target angles
    private float swordAngle;
    private int targetAngle;

    // Create variables to hold the original position and rotation of the sword
    private Vector3 originalPos;
    private Quaternion originalRot;

    // Use this for initialization
    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab sword's hitbox (empty GameObject)
        swordBox = transform.Find("Sword Box");
        originalPos = swordBox.transform.localPosition;
        originalRot = swordBox.transform.rotation;

        // Initialize the sword box's collider and disable it
        swordCollider = swordBox.GetComponent<BoxCollider2D>();
        swordCollider.enabled = false;

        // Reset timer and variables
        timer = 0.0f;
        attacking = false;
        holding = false;
    }
	
	// Update is called once per frame
	void Update ()
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

        if (attacking)
        {
            // Get the sword angle and check
            swordAngle = swordBox.rotation.eulerAngles.z;

            // Set target angle and attack direction depending on player position
            if (spriteRenderer.flipX)
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
            if ((spriteRenderer.flipX && (int)swordAngle <= targetAngle) ||
                (!spriteRenderer.flipX && (int)swordAngle >= targetAngle))
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

    // Resets the sword's properties
    private void ResetSword()
    {
        swordCollider.enabled = false;
        swordBox.transform.localPosition = originalPos;
        swordBox.transform.rotation = originalRot;
        swordSpeed = 200.0f;
        holding = false;
        timer = 0.0f;
    }

    // Logic for Ignoring collision with tilemap
}
