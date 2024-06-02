using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    //global reference to this script
    public static EventManager current;

    private void Awake() {
        current = this; 
    }

    //list of observers for events
    public event Action onExhaustedEvent;
    public event Action onReplenishedEvent;


    // ---------------- Trigger Event Methods --------------------\\

    public void TriggerExhaustedEvent() {
        if(onExhaustedEvent != null)
            onExhaustedEvent();
    }

    public void TriggerReplenishedEvent() {
        if(onReplenishedEvent != null) 
            onReplenishedEvent();
    }


}
