using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovement : MonoBehaviour
{
    [Header ("References")]
    private Rigidbody body;
    private PlayerInput inputScript;
    private PlayerManager playerManagerScript;
    private PlayerRotation rotationScript;
    private PlayerStamina staminaScript;

    [Header ("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float exhaustedSpeed;
    [SerializeField] private float groundedDrag;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 moveVel = Vector3.zero;
    private float curSpeed;

    [Header ("Jump Settings")]
    [SerializeField] private float jumpForce;
    private bool hasJumped = false;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        inputScript = GetComponent<PlayerInput>();
        playerManagerScript = GetComponent<PlayerManager>();
        rotationScript = GetComponent<PlayerRotation>();
        staminaScript = GetComponent<PlayerStamina>();
    }

    private void OnEnable() {
        // reset variables
        moveDir = Vector3.zero;
        body.drag = groundedDrag;
        hasJumped = false;

        // set initial speed
        if(staminaScript.IsExhausted)
            curSpeed = exhaustedSpeed;
        else
            curSpeed = walkSpeed;
    }

    private void Update() {
        ManageMovement();
        LimitVelocity();
    }

    /** Applies movement calculations to rigidbody **/
    private void FixedUpdate() {
        body.AddForce(moveDir.normalized * curSpeed * 50f, ForceMode.Force);
    }


    /** Determines which movements to do (walk, run, exhausted, or jump) **/
    private void ManageMovement() {

        // stop everything when jumping (handled by air state script)
        if(hasJumped)
            return;

        if(!staminaScript.IsExhausted) {
            // run
            if(inputScript.RunInput)
                Run();
            // walk
            else
                curSpeed = walkSpeed;
        }

        MovementCalculations();

        if(inputScript.JumpInput)
            Jump();
    }


    /** Sets current speed and uses run stamina **/
    private void Run() {

        // use run stamina
        staminaScript.UseStaminaPerSecond('R');

        // set speed
        if(staminaScript.IsExhausted)
            curSpeed = exhaustedSpeed;
        else
            curSpeed = runSpeed;
    }


    /** Calculates horizontal movement direction **/
    private void MovementCalculations() {
        
        // move direction calculations
        moveDir = transform.forward * inputScript.MoveInput.magnitude;
    }


    /** Caps horizontal movement velocity **/
    private void LimitVelocity() {

        moveVel = body.velocity;
        moveVel.y = 0f;

        // limit horizontal velocity
        if(moveVel.magnitude > curSpeed) {

            moveVel = moveVel.normalized * curSpeed;
            moveVel.y = body.velocity.y;
            body.velocity = moveVel;
        }
    }


    /** Applies jump impulse and stamina **/
    private void Jump() {
    
        // reset y velocity
        body.velocity = new Vector3(body.velocity.x, 0f, body.velocity.z);

        // apply jump force
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // use stamina when running
        if(curSpeed == runSpeed)
            staminaScript.UseStaminaBurst('J');

        // jump settings
        body.drag = 0f;
        hasJumped = true;
        moveDir = Vector3.zero;
        rotationScript.ChangeRotationSpeed('A');
    }

}
