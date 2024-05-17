using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float rayInterval;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask floorLayer;
    private float rayTimer = 0f;
    private State playerState = State.Grounded;

    private void Awake() {
        rayTimer = rayInterval;

        //rayDistance is inspector value + distance from origin to bottom of collider
        rayDistance += GetComponent<CapsuleCollider>().height/2f;
    }

    private enum State {
        Grounded, Air, Water, Gliding, Climbing
    }

    private void Update() {
        rayTimer -= Time.deltaTime;
        if(rayTimer < 0)
            RaycastGroundCheck();

        //run code for current state
        switch(playerState) {
            case State.Grounded:
                break;
        }

    }

    private void RaycastGroundCheck() {
        rayTimer = rayInterval;

        //if a ray going down collides with floor
        if(Physics.Raycast(transform.position, -transform.up, rayDistance, floorLayer)) {
            
        }
    }

    private void ChangeStates() {
        Debug.Log("Change States");
    }

}
