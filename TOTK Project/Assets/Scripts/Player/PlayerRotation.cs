using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header ("References")]
    private PlayerInput inputScript;
    private Camera cam;

    [Header ("Settings")]
    [SerializeField] private float groundedRotSpeed;
    [SerializeField] private float airRotSpeed;
    private float curRotSpeed;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 rotateDir = Vector3.zero;
    private Vector3 camForward = Vector3.zero;
    private Vector3 camRight = Vector3.zero;

    private void Awake() {
        inputScript = GetComponent<PlayerInput>();
        cam = Camera.main;

        rotateDir = transform.forward;

        //TEMP
        curRotSpeed = groundedRotSpeed;
    }

    private void Update() {
        RotatePlayer();
    }

    /** Rotates player toward input direction relative to camera **/
    private void RotatePlayer() {

        //camera direction
        camForward = cam.transform.forward;
        camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        moveInput = inputScript.MoveInput;

        //direction of input relative to camera
        if(moveInput != Vector2.zero && curRotSpeed != 0f)
            rotateDir = (camForward * moveInput.y) + (camRight * moveInput.x);

        //smoothly rotate player to new direction
        transform.forward = Vector3.Slerp(transform.forward, rotateDir, curRotSpeed * Time.deltaTime);
    }

    /** Changes rotation speed when in different states **/
    public void ChangeRotationSpeed(char stateChar) {
        
        switch(stateChar) {
            case 'G':
                curRotSpeed = groundedRotSpeed;
                break;
            case 'A':
                curRotSpeed = airRotSpeed;
                break;
            default:
                Debug.LogError("PlayerRotation > ChangeRotationSpeed parameter incorrect");
                break;
        }
    }

}
