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
    public bool IsExhausted { get; private set;}
    private float rechargeDelayCounter = 0f;


    private void Awake() {
        managerScript = GetComponent<PlayerManager>();
        CurStamina = maxStamina;
    }


    private void Update() {

        rechargeDelayCounter -= Time.deltaTime;

        // if recharge delay finished and grounded
        if(rechargeDelayCounter < 0 && managerScript.IsGrounded)
            RechargeStamina();

        Debug.Log("Stamina: " + CurStamina);
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

        // use stamina per second for movement type
        switch(staminaType) {

            case 'R':
                CurStamina -= runStaminaPerSecond * Time.deltaTime;
                break;
            case 'G':
                CurStamina -= glideStaminaPerSecond * Time.deltaTime;
                break;
            default:
                Debug.LogError("PlayerStamina > UseStaminaPerSecond parameter is incorrect");
                break;
        }

        rechargeDelayCounter = rechargeDelayTime;
        LimitStamina();
    }

    /** Uses stamina burst based on based on movement type.
        (J = Jump, C = Climb, S = Swim) **/
    public void UseStaminaBurst(char staminaType) {

        if(CurStamina <= 0)
            return;

        switch(staminaType) {

            case 'J':
                CurStamina -= jumpStaminaBurst;
                break;
            case 'C':
                break;
            case 'S':
                break;
            default:
                Debug.LogError("PlayerStamina > UseStaminaBurst parameter is incorrect");
                break;
        }

        rechargeDelayCounter = rechargeDelayTime;
        LimitStamina();
    }

    /** Limits stamina between 0 and max stamina and sets IsExhausted. **/
    private void LimitStamina() {
        
        if(CurStamina < 0) {
            CurStamina = 0f;
            IsExhausted = true;
        }
        else if(CurStamina > maxStamina) {
            CurStamina = maxStamina;
            IsExhausted = false;
        }
    }

}
