using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header ("References")]
    private PlayerManager managerScript;
    private GroundedMovement groundedScript;
    private AirMovement airScript;
    private PlayerStamina staminaScript;
    private Animator animator;
    private Rigidbody body;
    private Vector2 velocity;
    private float maxSpeed;
    private int isGroundedStateHash;
    private int isAirStateHash;
    private int isJumpingHash;
    private int isLandingHash;
    private int isExhaustedHash;
    private int velocityHash;

    private void Awake() {
        animator = GetComponent<Animator>();
        managerScript = GetComponent<PlayerManager>();
        groundedScript = GetComponent<GroundedMovement>();
        airScript = GetComponent<AirMovement>();
        staminaScript = GetComponent<PlayerStamina>();
        body = GetComponent<Rigidbody>();

        // converts string to int
        isGroundedStateHash = Animator.StringToHash("isGroundedState");
        isAirStateHash = Animator.StringToHash("isAirState");
        isJumpingHash = Animator.StringToHash("isJumping");
        isLandingHash = Animator.StringToHash("isLanding");
        isExhaustedHash = Animator.StringToHash("isExhausted");
        velocityHash = Animator.StringToHash("velocity");
    }

    private void Start() {
        // get max speed variable
        if(groundedScript.enabled)
            maxSpeed = groundedScript.GetMaxSpeed();
        else
            Debug.Log("GroundedScript is disabled upon start, can't get max speed");
    }

    private void Update() {
        SetStateAnimations();

        if(groundedScript.enabled)
            SetGroundedAnimations();
        else if(airScript.enabled)
            SetAirAnimations();
    }

    private void SetStateAnimations() {
        // from PlayerManager
        animator.SetBool(isGroundedStateHash, managerScript.PlayerState == PlayerManager.State.Grounded);
        animator.SetBool(isAirStateHash, managerScript.PlayerState == PlayerManager.State.Air);
        animator.SetBool(isLandingHash, managerScript.IsLanding);

        // from Rigidbody
        velocity.x = body.velocity.x;
        velocity.y = body.velocity.z;
        // float from 0 - 1 where 1 is max speed
        animator.SetFloat(velocityHash, Mathf.Abs(velocity.magnitude) / maxSpeed);
    }

    private void SetGroundedAnimations() {
        // from PlayerStamina
        animator.SetBool(isExhaustedHash, staminaScript.IsExhausted);

        // from GroundedMovement
        animator.SetBool(isJumpingHash, groundedScript.HasJumped);
    }

    private void SetAirAnimations() {


    }





}
