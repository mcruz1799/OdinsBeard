using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAi : MonoBehaviour {

    bool stone;
    bool alive;
    bool triggered;
    public int hp;
    int stoneCount;

    int attackCount;
    public int timeToAttack;

    public int framesMoving;
    public int framesStone;

    SpriteRenderer spriteR;
    Sprite normalSprite;
    public Sprite stoneSprite;

    public double radius;
    Vector3 initialPos;
    public float speed;
    bool headingLeft;

    GameObject player;

	// Use this for initialization
	void Start () {
        //Flag for being turned into stone
        stone = false;



        alive = true;

        //Flag the controls whether or not the golem ignores the player. If this is set to true then the golem will begin level wanting to attack the player.
        triggered = false;

        //Max HP of the golem, technically 1 lower since the golem will take 1 point of damage to get triggered

        //The two sprites the AI is using to transform
        normalSprite = GetComponent<SpriteRenderer>().sprite;

        //Radius is how far it will move from its beginning condition, speed is how fast it walks
        

        //Which direction the AI is walking initially
        headingLeft = true;

        initialPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");

        //Flag for how the ai will start its first attack. Set it positive to make the first attack a lot faster, and lower to make it delayed.
        attackCount = 0;

        //Flags for how many frames the AI will spend in each turn
        framesMoving = 9 * 24;
        framesStone = 3 * 24;
    }
	
	// Update is called once per frame
	void Update () {

        if (!triggered)
        {
            if (headingLeft)
            {
                transform.position += new Vector3(-speed, 0, 0);
            }
            else
            {
                transform.position += new Vector3(speed, 0, 0);
            }
            Vector3 v = initialPos - transform.position;
            if (v.magnitude > radius)
            {
                headingLeft = !headingLeft;
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));

            }
        }
        else
        {
            if (alive)
            {
                if ((stoneCount < framesMoving && !stone) || (stoneCount < framesStone && stone))
                {
                    stoneCount++;
                }
                else
                {
                    switchStone();
                }
            }

            //Set the Scale
            if (!stone)
            {
                if ((player.transform.position.x < transform.position.x) && (transform.localScale.x < 0))
                {
                    transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
                }
                else if ((player.transform.position.x > transform.position.x) && (transform.localScale.x > 0))
                {
                    transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
                }

                //Attacking vs. movement controls
                if (getDistanceToPlayer() < 6)
                {
                    if (attackCount < timeToAttack)
                    {
                        attackCount++;
                    }
                    else
                    {
                        if (player.transform.position.x < transform.position.x)
                        {
                            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-16, 0), ForceMode2D.Impulse);
                        }
                        else
                        {
                            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(16, 0), ForceMode2D.Impulse);
                        }
                        attackCount = 0;
                    }
                }
                else
                {
                    attackCount = 0;
                    //Controls Movement
                    if (headingLeft)
                    {
                        transform.position += new Vector3(-speed, 0, 0);
                    }
                    else
                    {
                        transform.position += new Vector3(speed, 0, 0);
                    }
                    Vector3 v = initialPos - transform.position;
                    if (v.magnitude > radius)
                    {
                        headingLeft = !headingLeft;
                    }
                }


            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Sword" && !stone)
        {
            print("Golem was attacked.");
            triggered = true;
            switchStone();
            hp--;
            if (hp == 0)
            {
                alive = false;
            }
        }
    }

    void switchStone()
    {
        print("Golem Transforms");
        if (stone)
        {
            GetComponent<SpriteRenderer>().sprite = normalSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = stoneSprite;
        }
        stone = !stone;
        stoneCount = 0;
    }

    float getDistanceToPlayer()
    {
        Vector3 dist = player.transform.position - transform.position;
        return dist.magnitude;
    }
}
