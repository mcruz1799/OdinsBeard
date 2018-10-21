using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour {

    public int dropDistance;
    public float speed;
    public int framesActive;
    public int framesWaiting;
    public bool startsActive;
    bool movingDown;
    int count;
    Vector3 startPos;

	// Use this for initialization
	void Start () {
        count = 0;
        if (startsActive)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            startPos = transform.position;
        }
        else
        {
            startPos = transform.position + new Vector3(0, dropDistance, 0);
            GetComponent<BoxCollider2D>().enabled = false;
            movingDown = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (startsActive)
        {
            if (count < framesActive)
            {
                count++;
            }
            else
            {
                startsActive = false;
                movingDown = true;
                GetComponent<BoxCollider2D>().enabled = false;
                count = 0;
            }
        }
        else
        {
            //Moving down code
            if (movingDown)
            {
                transform.position += new Vector3(0, -speed, 0);
                Vector3 dist = transform.position - startPos;
                if (dist.magnitude > dropDistance)
                {
                    movingDown = false;
                }
            }
            else
            {
                if (count < framesWaiting)
                {
                    //Holding for undergound delay
                    count++;
                }
                else
                {
                    //Moving back up!
                    transform.position += new Vector3(0, speed, 0);
                    if (transform.position.y >= startPos.y)
                    {
                        startsActive = true;
                        GetComponent<BoxCollider2D>().enabled = true;
                        count = 0;
                    }
                }
            }
        }
		
	}





}
