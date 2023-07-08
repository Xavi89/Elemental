using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class Dashing : MonoBehaviour
{
    PlayerController moveScript;
    public float dashSpeed;
    public float dashTime;

    private PlayerInput playerInput;
    private InputAction dashAction;


    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];
    }

    private void Start() {
        moveScript = GetComponent<PlayerController>();
    }

    void Update() {
        bool isDashing = dashAction.ReadValue<float>() > 0.1f;
        if(isDashing)
        {
            StartCoroutine(Dash());
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
