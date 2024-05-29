using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovement : MonoBehaviour
{
    [Header ("References")]
    private Rigidbody body;

    private void Awake() {
        body = GetComponent<Rigidbody>();

    }


}
