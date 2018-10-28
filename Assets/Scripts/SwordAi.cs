using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAi : MonoBehaviour {

    GameObject player;
    Vector3 startPos;
    bool headingLeft;
    bool facingLeft;
    bool attacking;
    Transform sword;
    int count;
    public int hp;
    public double radius;
    public float speed;
    public int swingDegreePerFrame;
    public int swingFrames;
    public int delayAfterAttack;

	// Use this for initialization
	void Start () {
        startPos = transform.position;

        //Requires a player object to function
        player = GameObject.FindGameObjectWithTag("Player");

        //This picks which direction it starts moving in, not important to change unless this is the 1st enemy on the screen
        headingLeft = false;

        //Which direction the original sprite is facing
        facingLeft = true;

        //How far the ai will walk from its beginning point
        //radius = 1.3;
        //How fast it will do that walk
        //speed = .03f;

        //An internal variable, don't change
        attacking = false;

        //Requires a child with the 'Sword' tag
        foreach(Transform child in transform)
        {
            if (child.tag == "Sword")
            {
                sword = child;
            }
        }
        //Don't edit, this will make wonky behavior the first time it swings.
        //If you want to make them more fair to fight, set this to a negative number. That will be added frame delay the first time the enemy attacks, but they'll act normally after 1 attack.
        count = 0;

        //The hp of the enemy
        //hp = 3;

        //This variable determines how far the sword swings per frame on the downswing
        //swingDegreePerFrame = 10;

        //This variable is how many frames will be spent swinging. The multiple of this and swingDegreePerFrame is how far the total downswing is.
        //swingFrames = 10;

        //delayAfterAttack is how many frames the ai will hold its position after attacking
        //delayAfterAttack = 125;

	}
	
	// Update is called once per frame
	void Update () {

        //Set the enemy to face the player
        if (!attacking)
        {
            if ((player.transform.position.x > transform.position.x) && (transform.localScale.x == 1))
            {
                facingLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if ((player.transform.position.x < transform.position.x) && (transform.localScale.x == -1))
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;
            }
        }

        //Big if: if alive, do regular things, else jump.

        if (hp > 0)
        {
            if (getDistanceToPlayer() < 2.5)
            {
                attacking = true;
            }

            if (attacking)
            {
                //Grabs pivot object, finds rotation, checks if its good, rotates if not, holds if so, waits, then resets.
                if (count < 25)
                {
                    count++;
                }
                else
                {

                    if (count < (25 + swingFrames))
                    {
                        //Rotate the sword
                        if (facingLeft)
                        {
                            sword.RotateAround(transform.position, new Vector3(0, 0, 1), swingDegreePerFrame);
                        }
                        else
                        {
                            sword.RotateAround(transform.position, new Vector3(0, 0, 1), -swingDegreePerFrame);
                        }
                        count++;
                    }
                    else
                    {
                        //This code executes once the slash is done
                        if (count < (delayAfterAttack + swingFrames))
                        {
                            //Hold position to create delay
                            count++;
                        }
                        else
                        {
                            //Reset!
                            if (facingLeft)
                            {
                                sword.RotateAround(transform.position, new Vector3(0, 0, 1), -(swingFrames * swingDegreePerFrame));
                            }
                            else
                            {
                                sword.RotateAround(transform.position, new Vector3(0, 0, 1), (swingFrames * swingDegreePerFrame));
                            }
                            attacking = false;
                            count = 0;
                        }
                    }
                }
            }
            else
            {
                if (headingLeft)
                {
                    transform.position += new Vector3(-speed, 0f, 0f);
                    Vector3 p = startPos - transform.position;
                    if (p.magnitude > radius)
                    {
                        headingLeft = false;
                    }
                }
                else
                {
                    transform.position += new Vector3(speed, 0f, 0f);
                    Vector3 p = startPos - transform.position;
                    if (p.magnitude > radius)
                    {
                        headingLeft = true;
                    }
                }

            }
        }
        else
        {
            //Dead!
            if (count < 25)
            {
                transform.position += new Vector3(0, .2f, 0);
            }
            else if (count < 100)
            {
                transform.position += new Vector3(0, -.6f, 0);
            }
            else
            {
                Destroy(gameObject);
            }
            count++;
        }
		
	}

    float getDistanceToPlayer()
    {
        Vector3 dist = player.transform.position - transform.position;
        return dist.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("AI Took damage");
            hp--;
            if (hp == 0)
            {
                die();
            }
        }
    }

    void die()
    {
        //Turns off sprites and colliders so that the AI is harmless
        SpriteRenderer[] all = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < all.Length; i++)
        {
            all[i].enabled = false;
        }
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        count = 0;
    }


}
