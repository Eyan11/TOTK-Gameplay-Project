using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header ("References")]
    private PlayerInput inputScript;
    private Camera cam;

    [Header ("Settings")]
    [SerializeField] private float walkRotSpeed;
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
        curRotSpeed = walkRotSpeed;
    }

    private void Update() {
        RotatePlayer();

    }

    private void RotatePlayer() {
        //camera direction
        camForward = cam.transform.forward;
        camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        moveInput = inputScript.MoveInput;

        //direction of input relative to camera
        if(moveInput != Vector2.zero)
            rotateDir = (camForward * moveInput.y) + (camRight * moveInput.x);

        //smoothly rotate player to new direction
        transform.forward = Vector3.Slerp(transform.forward, rotateDir, curRotSpeed * Time.deltaTime);
    }

}
