using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform playerTran;
    [SerializeField] private Transform camLookTran;
    [SerializeField] private Transform sliderTran;
    [SerializeField] private Slider greenWheel;
    [SerializeField] private Image greenWheelFillImage;
    [SerializeField] private Slider redWheel;
    [SerializeField] private Image redWheelFillImage;
    [SerializeField] private Image backgroundImage;
    private PlayerStamina playerStaminaScript;
    private Camera cam;
    private Vector3 staminaScreenPos;
    private bool isVisible = false;
    private float maxStamina = 0f;
    private bool isRunningCoroutine = false;
    // colors
    private Color orangeColor;
    private Color redColor;
    private Color greenColor;
    private Color whiteColor;
    private Color pinkColor;
    private Color backgroundColor;

    [Header ("Settings")]
    [SerializeField] private float visibleDuration;
    [SerializeField] private float disapearDuration;
    private float visibleTimer;
    private float replenishedTimer = 0f;
    // color speeds
    private const float EXHUASTED_COLOR_SPEED = 1.2f;
    private const float STAMINA_COLOR_SPEED = 3f;
    private const float REPLENISHED_COLOR_CHANGE_TIME = 0.1f;
    private const float REPLENISHED_WHITE_PAUSE_TIME = 0.1f;

    [Header ("Settings")]
    [SerializeField] private float playerOffsetX;
    [SerializeField] private float playerOffsetY;

    private void Awake() {
        playerStaminaScript = playerTran.GetComponent<PlayerStamina>();
        cam = Camera.main;
        visibleTimer = visibleDuration;
        orangeColor = new Color(1f, 0.4f, 0f, 1f);
        redColor = Color.red;
        greenColor = new Color(0f, 0.9f, 0f, 1f);
        whiteColor = Color.white;
        pinkColor = new Color(1f, 0.5f, 0.5f, 1f);
        backgroundColor = new Color(0f, 0f, 0f, 0.9f);
    }

    private void Start() {
        maxStamina = playerStaminaScript.CurStamina;
    }

    private void Update() {
        ManageVisibility();

        if(isVisible) {
            StaminaWheelValue();
            StaminaWheelColor();
        }
    }

    private void FixedUpdate() {
        // in fixed update since movement is updated in fixed update
        // otherwise stamina wheel will jitter
        if(isVisible)
            StaminaWheelPosition();
    }

    /** Enables and disables the stamina UI canvas **/
    private void ManageVisibility() {

        // set isVisible
        if(playerStaminaScript.CurStamina >= maxStamina) {
            visibleTimer -= Time.deltaTime;

            if(visibleTimer < 0) {
                visibleTimer = visibleDuration;
                isVisible = false;
            }
        }
        else {
            isVisible = true;
            visibleTimer = visibleDuration;
        }


        // enable/disable stamina wheel
        if(sliderTran.gameObject.activeInHierarchy && !isVisible)
            sliderTran.gameObject.SetActive(false);
        else if(!sliderTran.gameObject.activeInHierarchy && isVisible) {
            sliderTran.gameObject.SetActive(true);
            // reset alpha values to 1
            greenWheelFillImage.color = greenColor;
            redWheelFillImage.color = redColor;
            backgroundImage.color = backgroundColor;
        }
    }

    /** Updates the position of the stamina UI wheel to always be next to player **/
    private void StaminaWheelPosition() {
        staminaScreenPos = camLookTran.position + (-cam.transform.right * playerOffsetX + cam.transform.up * playerOffsetY);
        sliderTran.position = cam.WorldToScreenPoint(staminaScreenPos);
    }

    /** Updates the value of the red and green stamina wheels **/
    private void StaminaWheelValue() {
        redWheel.value = playerStaminaScript.CurStamina;
        greenWheel.value = redWheel.value - (playerStaminaScript.CurStaminaUsage/2f);
    }

    /** Changes the color of the stamina wheel depending on its state **/
    private void StaminaWheelColor() {

        // if exhausted
        if(playerStaminaScript.IsExhausted)
            greenWheelFillImage.color = Color.Lerp(orangeColor, redColor, Mathf.PingPong(Time.time * EXHUASTED_COLOR_SPEED, 1));
        
        // if stamina is being used
        else if(playerStaminaScript.CurStaminaUsage > 0) {
            redWheelFillImage.color = Color.Lerp(pinkColor, redColor, Mathf.PingPong(Time.time * STAMINA_COLOR_SPEED, 1));
            greenWheelFillImage.color = greenColor;
        }

        // if stamina is replenished
        else if(!isRunningCoroutine && playerStaminaScript.CurStamina >= maxStamina) {
            StartCoroutine("ReplenishedColorCoroutine");
            Debug.Log("Start Coroutine");
        }

        // if running coroutine even though not replenished, stop it
        if(isRunningCoroutine && playerStaminaScript.CurStamina < maxStamina) {
            StopCoroutine("ReplenishedColorCoroutine");
            isRunningCoroutine = false;
        }

    }

    /** Handles stamina UI color when stamina is replenished.
        The wheel flashes white, then fades away. **/
    private IEnumerator ReplenishedColorCoroutine() {
        isRunningCoroutine = true;
        replenishedTimer = 0f;
        float colorChange = 0f;

        Debug.Log("Once");

        // change to white (0 > 1)
        while(replenishedTimer < REPLENISHED_COLOR_CHANGE_TIME) {

            replenishedTimer += Time.deltaTime;
            colorChange = replenishedTimer / REPLENISHED_COLOR_CHANGE_TIME;

            greenWheelFillImage.color = new Color(colorChange, 1f, colorChange, 1f);
            yield return null;
        }

        // pause while white
        greenWheelFillImage.color = whiteColor;
        yield return new WaitForSeconds(REPLENISHED_WHITE_PAUSE_TIME);
        replenishedTimer = REPLENISHED_COLOR_CHANGE_TIME;

        // change to green (1 > 0)
        while(replenishedTimer > 0f) {

            replenishedTimer -= Time.deltaTime;
            colorChange = replenishedTimer / REPLENISHED_COLOR_CHANGE_TIME;

            greenWheelFillImage.color = new Color(colorChange, 1f, colorChange, 1f);
            yield return null;
        }

        // wait until ready to dissapear
        if(isVisible && visibleTimer > disapearDuration)
            yield return new WaitForSeconds(visibleTimer - disapearDuration);


        // fade away
        while(visibleTimer > 0) {

            colorChange = visibleTimer / disapearDuration;

            greenWheelFillImage.color = new Color(0f, 0.9f, 0f, colorChange);
            redWheelFillImage.color = new Color(1f, 0f, 0f, colorChange);
            backgroundImage.color = new Color(0f, 0f, 0f, colorChange);
            yield return null;
        }

        isRunningCoroutine = false;
    }
}
