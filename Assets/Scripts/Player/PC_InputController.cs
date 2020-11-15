using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Movement_controller))]
public class PC_InputController : MonoBehaviour
{
    Movement_controller playerMovement;
    DateTime strikeClickTime;
    float move;
    bool jump;
    bool roll;
    bool canAttack;
    void Start()
    {
        playerMovement = GetComponent<Movement_controller>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButton("Jump");
        roll = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKey(KeyCode.E))
            playerMovement.StartCasting();
        if (Input.GetButtonDown("Fire1"))
        {
            strikeClickTime = DateTime.Now;
            canAttack = true;
        }    
        if (Input.GetButtonUp("Fire1"))
        {
            float holdrtime = (float)(DateTime.Now-strikeClickTime).TotalSeconds;
            if(canAttack)
                playerMovement.StartStrike(holdrtime);
            canAttack = false;
        }
        if ((DateTime.Now - strikeClickTime).TotalSeconds >= playerMovement.MaxChargeTime && canAttack) 
        {
            playerMovement.StartStrike(playerMovement.MaxChargeTime);
            canAttack = false;
        }

    }
    private void FixedUpdate()
    {
        playerMovement.Move(move, jump, roll);
        jump = false;
    }
}
