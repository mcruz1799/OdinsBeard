using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour {

    Sprite normal;
    public Sprite WarmupSprite;
    public GameObject flame;

    public int warmupDelay;
    public int switchCount;
    public int flameDelay;
    
    int count;
   
    bool active;
    bool flameActive;
    bool displayNormal;

	// Use this for initialization
	void Start () {
        normal = GetComponent<SpriteRenderer>().sprite;
        displayNormal = true;
        active = false;
        count = 0;
        if (warmupDelay < 5)
        {
            warmupDelay = 0;
        }
        flame.GetComponent<BoxCollider2D>().enabled = false;
        flame.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (active)
        {
            if (count < warmupDelay)
            {
                if (count % switchCount == 0)
                {
                    switchSprite();
                }
            }
            else
            {
                if (displayNormal)
                {
                    switchSprite();
                }
                if (!flameActive)
                {
                    activateFlame();
                }
                if (count < warmupDelay + flameDelay)
                {
                    
                }
                else
                {
                    //Turn things off
                    flameActive = false;
                    flame.GetComponent<SpriteRenderer>().enabled = false;
                    flame.GetComponent<BoxCollider2D>().enabled = false;

                    displayNormal = true;
                    GetComponent<SpriteRenderer>().sprite = normal;
                }
            }
            count++;
        }
	}

    public void activate()
    {
        active = true;
    }

    public void switchSprite()
    {
        if (displayNormal)
        {
            displayNormal = false;
            GetComponent<SpriteRenderer>().sprite = WarmupSprite;
        }
        else
        {
            displayNormal = true;
            GetComponent<SpriteRenderer>().sprite = normal;
        }
    }

    public void activateFlame()
    {
        flameActive = true;
        flame.GetComponent<SpriteRenderer>().enabled = true;
        flame.GetComponent<BoxCollider2D>().enabled = true;
    }
}
