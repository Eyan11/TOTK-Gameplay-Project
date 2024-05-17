using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovement : MonoBehaviour
{
    [Header ("References")]
    private Rigidbody body;
    private PlayerInput inputScript;

    [Header ("Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float moveAcceleration;
    private Vector2 moveInput;
    private bool jumpInput;
    private Vector3 velocity;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        inputScript = GetComponent<PlayerInput>();
    }

    private void FixedUpdate() {
        //get input


        MovePlayer();
    }

    private void MovePlayer() {
        //velocity is always in forward direction
        velocity = transform.forward * inputScript.MoveInput.magnitude * walkSpeed;
        velocity.y = body.velocity.y;

        body.velocity = velocity;
    }


}
