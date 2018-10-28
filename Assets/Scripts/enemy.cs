using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public float distToPlayer;

    private Vector3 startPos; //position before each frame
    private Vector3 newPos; //position change during each frame
    private Vector3 center; //center of neutral phase
    private float StartVerSpeed = -0.15f;
    private float StartHoriSpeed = 0.075f;
    private float verSpeed;
    private float horiSpeed;
    private float gravity = 0.0024f;
    private float speed = 0.0f; //-0.15f;
    private float force; //accelaration to the center when neutral
    private float upperRange;
    private int mode; //0 for neutral, 1 for divebomb, 2 for reset
    private int attackDirection;
    private float timer;
    private bool needIni; //to initialize neutral() and divebomb()
    
    GameObject character;
    Vector3 characterPos;

    void Start()
    {
        distToPlayer = 5.0f;

        startPos = transform.position;
        mode = 0;
        upperRange = transform.position.y + 0.1f;
        timer = 0.0f;
        verSpeed = StartVerSpeed;
        horiSpeed = StartHoriSpeed;
        attackDirection = 0;
        needIni = true;
        character = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        if(mode == 0)
        {
            Neutral();
        }

        else if(mode == 1)
        {
            Divebomb();
        }

        startPos = transform.position;
    }

    void Neutral()
    {
        timer += 1;
        if(Distance() < distToPlayer && timer > 150)
        {
            mode = 1; //switch to divebomb mode
            needIni = true;
            timer = 0;
            return;
        }
        if(needIni)
        {
            center.Set(startPos.x + 1.0f, startPos.y, startPos.z);
            needIni = false;
        }
        force = (center.x - startPos.x) * 0.002f;
        speed = speed + force;
        newPos.Set(speed, 0, 0);
        transform.position = startPos + newPos;
    }

    void Divebomb()
    {
        timer += 1;
        if(needIni)
        {
            DivebombIni();
        }
        verSpeed = verSpeed + gravity;
        newPos.Set(horiSpeed, verSpeed, 0);
        if (timer <= 10 || transform.position.y <= upperRange)
        {
            transform.position = startPos + newPos;
        }
        else 
        {
            timer = 0.0f;
            speed = 0;
            mode = 0;
            needIni = true;
        }
    }

    void DivebombIni()
    {
        timer = 0;
        if (transform.position.x > character.transform.position.x)
            attackDirection = -1;
        else
            attackDirection = 1;
        verSpeed = StartVerSpeed;
        horiSpeed = StartHoriSpeed * attackDirection;
        needIni = false;
    }

    float Distance()
    {
        characterPos = character.transform.position;
        float distance = 0;
        distance = Math.Abs(transform.position.x - characterPos.x);
        return distance;
    }
}
