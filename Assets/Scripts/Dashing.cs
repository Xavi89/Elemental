using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class Dashing : MonoBehaviour
{
    PlayerController moveScript;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = .05f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float dashCooldownTimer;

    private PlayerInput playerInput;
    private InputAction dashAction;

    private bool canDash;


    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];
    }

    private void Start() {
        moveScript = GetComponent<PlayerController>();
        canDash = true;
        dashCooldownTimer = dashCooldown;
    }

    void Update() {
        if(dashCooldownTimer >= dashCooldown) canDash = true;
        else{
            canDash = false;
            dashCooldownTimer += Time.deltaTime;
            dashCooldownTimer = Mathf.Clamp(dashCooldownTimer, 0f, dashCooldown);
        }
        bool isDashing = dashAction.ReadValue<float>() > 0f;
        if(isDashing && canDash)
        {
            StartCoroutine(Dash());
            canDash = false;
            dashCooldownTimer = 0;
        }
    }


    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            if(moveScript.move != Vector3.zero)
            {
                moveScript.controller.Move(moveScript.move * dashSpeed * Time.deltaTime);
            }
            else
            {
                moveScript.controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            }
        yield return null;
        }
    }
}
