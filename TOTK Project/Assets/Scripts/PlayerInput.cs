using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputMap inputMap;

    //Vector inputs
    public Vector2 MoveInput { get; private set; }

    //button down inputs
    public bool JumpInput { get; private set; }

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.Player.Enable();
    }

    private void Update() {

        //gets vector2 input from input map
        MoveInput = inputMap.Player.Move.ReadValue<Vector2>();

        //gets button down inputs (true for 1 frame)
        JumpInput = inputMap.Player.Jump.triggered;
    }

}
