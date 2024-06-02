using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header ("References")]
    private PlayerManager managerScript;

    [Header ("Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float rechargeStaminaPerSecond;
    [SerializeField] private float runStaminaPerSecond;
    [SerializeField] private float glideStaminaPerSecond;
    [SerializeField] private float jumpStaminaBurst;
    [SerializeField] private float rechargeDelayTime;
    public float CurStamina { get; private set; }
    public float CurStaminaUsage { get; private set; }
    public bool IsExhausted { get; private set;}
    public float RechargeDelayCounter { get; private set;}


    private void Awake() {
        managerScript = GetComponent<PlayerManager>();
        CurStamina = maxStamina;
        RechargeDelayCounter = 0f;
    }


    private void Update() {

        RechargeDelayCounter -= Time.deltaTime;

        // if Grounded state
        if(managerScript.PlayerState == PlayerManager.State.Grounded) {

            if(RechargeDelayCounter < 0)
                RechargeStamina();

            // reset CurStaminaUsage when not in use (for StaminaUI script)
            if(RechargeDelayCounter < 0.9)
                CurStaminaUsage = 0f;
        }


    }


    /** Recharges stamina when stamina has not been used for a short period **/
    private void RechargeStamina() {
        
        CurStamina += rechargeStaminaPerSecond * Time.deltaTime;
        LimitStamina();
    }


    /** Uses stamina per second based on based on movement type.
        (R = Run, G = Glide) **/
    public void UseStaminaPerSecond(char staminaType) {

        if(CurStamina <= 0)
            return;

        // set CurStaminaUsage
        switch(staminaType) {

            case 'R':
                CurStaminaUsage = runStaminaPerSecond;
                break;
            case 'G':
                CurStaminaUsage = glideStaminaPerSecond;
                break;
            default:
                Debug.LogError("PlayerStamina > UseStaminaPerSecond parameter is incorrect");
                break;
        }

        // calculate stamina
        CurStamina -= CurStaminaUsage * Time.deltaTime;
        RechargeDelayCounter = rechargeDelayTime;
        LimitStamina();
    }

    /** Uses stamina burst based on based on movement type.
        (J = Jump, C = Climb, S = Swim) **/
    public void UseStaminaBurst(char staminaType) {

        if(CurStamina <= 0)
            return;

        // set CurStaminaUsage
        switch(staminaType) {

            case 'J':
                CurStaminaUsage = jumpStaminaBurst;
                break;
            case 'C':
                break;
            case 'S':
                break;
            default:
                Debug.LogError("PlayerStamina > UseStaminaBurst parameter is incorrect");
                break;
        }

        // calculate stamina
        CurStamina -= CurStaminaUsage;
        RechargeDelayCounter = rechargeDelayTime;
        LimitStamina();
    }

    /** Limits stamina between 0 and max stamina and sets IsExhausted. **/
    private void LimitStamina() {
        
        if(CurStamina < 0) {
            CurStamina = 0f;
            IsExhausted = true;
            EventManager.current.TriggerExhaustedEvent();
        }
        else if(CurStamina > maxStamina) {
            CurStamina = maxStamina;
            IsExhausted = false;
            EventManager.current.TriggerReplenishedEvent();
        }
    }

}
