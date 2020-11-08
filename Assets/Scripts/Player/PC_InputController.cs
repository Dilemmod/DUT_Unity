using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement_controller))]
public class PC_InputController : MonoBehaviour
{
    Movement_controller playerMovement;
    float move;
    bool jump;
    bool roll;
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
    }
    private void FixedUpdate()
    {
        playerMovement.Move(move, jump, roll);
        jump = false;
    }
}
