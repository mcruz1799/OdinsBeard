using System.Collections;
using System;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{


    private Vector3 startPos; //position before each frame
    private Vector3 newPos; //position change during each frame
    private Vector3 center; //center of neutral phase
    private float StartVerSpeed = -0.15f;
    private float StartHoriSpeed = -0.075f;
    private float verSpeed;
    private float horiSpeed;
    private float gravity = 0.0024f;
    private float speed = 0.0f; //-0.15f;
    private float force; //accelaration to the center when neutral
    private float upperRange;
    private float lowerRange;
    private int mode; //0 for neutral, 1 for divebomb, 2 for reset
    private int time; //to calculate center for Neutral()
    private float timer;
    GameObject character;
    Vector3 characterPos;

    void Start()
    {
        startPos = transform.position;
        mode = 0;
        time = 0;
        upperRange = transform.position.y + 0.1f;
        timer = 0.0f;
        verSpeed = StartVerSpeed;
        horiSpeed = StartHoriSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            Neutral();
        }

        else if (mode == 1)
        {
            Divebomb();
        }

        else if (mode == 2)
        {
            Reset();
        }
        startPos = transform.position;
        timer += 1;
        //Debug.Log(mode);
        //Debug.Log(timer);
    }

    void Neutral()
    {
        if (Distance() < 12 && timer > 400)
        {
            mode = 1; //switch to divebomb mode
            verSpeed = StartVerSpeed;
            horiSpeed = StartHoriSpeed;
        }
        if (time == 0)
        {
            center.Set(startPos.x + 1.0f, startPos.y, startPos.z);
            time = 1;
        }
        //Debug.Log(center);
        force = (center.x - startPos.x) * 0.002f;
        speed = speed + force;
        newPos.Set(speed, 0, 0);
        transform.position = startPos + newPos;
    }

    void Divebomb()
    {
        verSpeed = verSpeed + gravity;
        newPos.Set(horiSpeed, verSpeed, 0);
        if (transform.position.y > center.y - 5.0f && transform.position.y < upperRange)
        {
            transform.position = startPos + newPos;
        }
        else
        {
            time = 0;
            timer = 0.0f;
            speed = 0;
            mode = 0;
        }
    }

    void Reset()
    {
        startPos.Set(startPos.x + 0.2f, startPos.y, startPos.z);
        transform.position = startPos;
        time = 0; //to calculate center for Neutral()
        if (Distance() >= 15)
        {
            mode = 0;
        }
    }

    float Distance()
    {
        character = GameObject.Find("Player");
        characterPos = character.transform.position;
        float distance = 0;
        distance = Math.Abs(transform.position.x - characterPos.x);
        return distance;
    }
}