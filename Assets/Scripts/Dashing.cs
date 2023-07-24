using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(PlayerInput))]

public class Dashing : MonoBehaviour
{
    PlayerController moveScript;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = .05f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float dashCooldownTimer = 0;
    [SerializeField] private int dashCharges = 0;
    [SerializeField] private int maxDashCharges = 3;

    [SerializeField] private Image iconCD;
    [SerializeField] private Image chargesCD;

    [SerializeField] private TextMeshProUGUI iconCDText;
    [SerializeField] private TextMeshProUGUI chargesText;
    [SerializeField] private ParticleSystem dashParticles;
    [SerializeField] private TrailRenderer dashTrail;



    private PlayerInput playerInput;
    private InputAction dashAction;

    private bool canDash;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];
        iconCD.fillAmount = 1;
        dashCharges = maxDashCharges;
    }

    private void Start() {
        moveScript = GetComponent<PlayerController>();
        dashParticles = GameObject.Find("Dash Lines").GetComponent<ParticleSystem>();
        dashParticles.Stop();
        dashTrail = GameObject.Find("Dash Trail Renderer").GetComponent<TrailRenderer>();
        dashTrail.emitting = false;
    }

    void Update() {
        chargesCD.fillAmount = dashCooldownTimer/dashCooldown;
        if(dashCharges < maxDashCharges){
            dashCooldownTimer += Time.deltaTime;
            if(dashCooldownTimer >= dashCooldown){
                dashCooldownTimer = 0;
                dashCharges += 1;
            }
        }
        if(dashCharges > 0){
            canDash = true;
        } else canDash = false;

        if(canDash && dashAction.triggered)
        {
            StartCoroutine(Dash());
            dashCharges -=1;
        }
        if(dashCharges < 1){
            iconCD.fillAmount = dashCooldownTimer/dashCooldown;
            chargesText.enabled = false;
            iconCDText.enabled = true;
            iconCDText.text = (dashCooldown - dashCooldownTimer).ToString("F0");
        }
        if(dashCharges >= 1){
            chargesText.enabled = true;
            chargesText.text = dashCharges.ToString();
            iconCDText.enabled = false;
        }
    }


    IEnumerator Dash()
    {
        dashParticles.Play();
        dashTrail.emitting = true;
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
        dashParticles.Stop();
        dashTrail.emitting = false;
    }
}
