using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAi : MonoBehaviour {

    GameObject player;
    Vector3 startPos;
    double radius;
    float speed;
    bool headingLeft;
    bool facingLeft;
    bool attacking;
    Transform pivot;
    int count;
    int hp;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        headingLeft = false;
        facingLeft = false;
        radius = 1.3;
        speed = .03f;
        attacking = false;
        foreach(Transform child in transform)
        {
            if (child.tag == "Pivot")
            {
                pivot = child;
            }
        }
        int count = 0;
        hp = 3;
	}
	
	// Update is called once per frame
	void Update () {

        //Set the enemy to face the player
        if (!attacking)
        {
            if ((player.transform.position.x < transform.position.x) && (transform.localScale.x == 1))
            {
                facingLeft = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if ((player.transform.position.x > transform.position.x) && (transform.localScale.x == -1))
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = false;
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

                    float zRotation = pivot.rotation.eulerAngles.z;
                    if (((facingLeft && zRotation < 100) || (!facingLeft && zRotation > 260)) || zRotation == 0)
                    {
                        pivot.Rotate(new Vector3(0, 0, -20));
                    }
                    else
                    {
                        //This code executes once the slash is done
                        if (count < 125)
                        {
                            count++;
                        }
                        else
                        {
                            //Reset!

                            pivot.eulerAngles = new Vector3(0, 0, 0);
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
