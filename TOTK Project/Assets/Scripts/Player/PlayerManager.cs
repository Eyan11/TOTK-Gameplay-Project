using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("References")]
    private PlayerRotation rotationScript;
    private GroundedMovement groundedMoveScript;
    private AirMovement airMoveScript;

    [Header ("Settings")]
    [SerializeField] private float rayInterval;
    [SerializeField] private float groundedRayDist;
    [SerializeField] private float landingRayDist;
    [SerializeField] private LayerMask floorLayer;
    public bool IsGrounded { get; private set; }
    public bool IsLanding { get; private set; }
    private Ray ray;
    private RaycastHit hit;

    public enum State { Grounded, Air, Water, Climbing }
    public State PlayerState { get; private set; }

    private void Awake() {
        rotationScript = GetComponent<PlayerRotation>();
        groundedMoveScript = GetComponent<GroundedMovement>();
        airMoveScript = GetComponent<AirMovement>();
        
        StartCoroutine(RaycastCoroutine());

        //enable grounded scripts
        ChangeStates(State.Grounded);

        //rayDistance is inspector value + distance from origin to bottom of collider
        groundedRayDist += GetComponent<CapsuleCollider>().height/2f;
        landingRayDist += GetComponent<CapsuleCollider>().height/2f;
    }

    /** Checks if grounded and switches states **/
    private IEnumerator RaycastCoroutine() {

        // raycast
        ray = new Ray(transform.position, -transform.up);

        if(Physics.Raycast(ray, out hit, floorLayer)) {

            IsGrounded = (hit.distance <= groundedRayDist);
            IsLanding = (hit.distance <= landingRayDist) && (PlayerState == State.Air);
        }


        // change states
        if(IsGrounded && PlayerState == State.Air)
            ChangeStates(State.Grounded);
        else if(!IsGrounded && PlayerState == State.Grounded)
            ChangeStates(State.Air);

        yield return new WaitForSeconds(rayInterval);
        StartCoroutine(RaycastCoroutine());
    }

    /** Disables old state and enables new state **/
    private void ChangeStates(State newState) {

        //disable scripts from previous state
        switch(PlayerState) {

            case State.Grounded:
                groundedMoveScript.enabled = false;
                break;

            case State.Air:
                airMoveScript.enabled = false;
                break;
        }

        //enable script from new state
        switch(newState) {

            case State.Grounded:
                groundedMoveScript.enabled = true;
                rotationScript.ChangeRotationSpeed('G');
                break;

            case State.Air:
                airMoveScript.enabled = true;
                rotationScript.ChangeRotationSpeed('A');
                break;
        }

        PlayerState = newState;
    }

}
