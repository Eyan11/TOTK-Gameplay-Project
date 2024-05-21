using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("References")]
    private PlayerRotation rotationScript;
    private GroundedMovement groundedMoveScript;

    [Header ("Settings")]
    [SerializeField] private float rayInterval;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask floorLayer;
    public bool IsGrounded { get; private set; }
    private State playerState = State.Grounded;

    private void Awake() {
        rotationScript = GetComponent<PlayerRotation>();
        groundedMoveScript = GetComponent<GroundedMovement>();
        
        StartCoroutine(GrouncCheckCoroutine());

        //enable grounded scripts
        ChangeStates(State.Grounded);

        //rayDistance is inspector value + distance from origin to bottom of collider
        rayDistance += GetComponent<CapsuleCollider>().height/2f;
    }

    private enum State {
        Grounded, Air, Water, Gliding, Climbing
    }

    /** Checks if grounded and switches states **/
    private IEnumerator GrouncCheckCoroutine() {

        //if a ray going down collides with floor
        if(Physics.Raycast(transform.position, -transform.up, rayDistance, floorLayer)) {
            IsGrounded = true;

            if(playerState == State.Air)
                ChangeStates(State.Grounded);
        }
        else {
            IsGrounded = false;

            if(playerState == State.Grounded)
                ChangeStates(State.Air);
        }

        yield return new WaitForSeconds(rayInterval);
        StartCoroutine(GrouncCheckCoroutine());
    }

    /** Disables old state and enables new state **/
    private void ChangeStates(State newState) {

        //disable scripts from previous state
        switch(playerState) {

            case State.Grounded:
                groundedMoveScript.enabled = false;
                break;

            case State.Air:
                break;
        }

        //enable script from new state
        switch(newState) {

            case State.Grounded:
                groundedMoveScript.enabled = true;
                rotationScript.ChangeRotationSpeed('G');
                break;

            case State.Air:
                rotationScript.ChangeRotationSpeed('A');
                break;
        }

        playerState = newState;
    }

}
