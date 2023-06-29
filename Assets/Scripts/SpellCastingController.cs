using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class SpellCastingController : MonoBehaviour
{
    [SerializeField] public SpellScriptableObject SpellToCast;
   
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform spellStoreParent;
    [SerializeField] private LayerMask PlayerLayerMask;
    
    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    private InputAction shootAction;
    private PlayerMagicSystem manaSystem;

    private bool castingMagic = false;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Image iconCooldown;
    [SerializeField] private float currentCastTimer;
    private float currentManaRechargeTimer;

    [Header("Mana")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] public float currentMana;
    [SerializeField] private float manaRechargeRate = 10f;
    [SerializeField] private float timeToWaitForManaRecharge = 1f;
    [SerializeField] private Image manaBarImage;
    [SerializeField] private TextMeshProUGUI manaText;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        shootAction = playerInput.actions["Shoot"];

        textCooldown.text = "";
        iconCooldown.fillAmount = 0;
        currentMana = maxMana;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        textCooldown.text = "";
        iconCooldown.fillAmount = 0;
    }

    void Update()
    {
        manaText.text = currentMana.ToString("F0") + " / " + maxMana.ToString("F0");
        manaBarImage.fillAmount = currentMana/maxMana;
        bool isSpellCastHeldDown = shootAction.ReadValue<float>() > 0;
        bool hasEnoughMana = currentMana - SpellToCast.ManaCost >= 0f;
        if(!castingMagic && isSpellCastHeldDown && hasEnoughMana)
        {
            castingMagic = true;
            currentMana -= SpellToCast.ManaCost;
            currentCastTimer = 0;
            currentManaRechargeTimer = 0;
            CastPrimarySpell();
        }
        if(castingMagic)
        {
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > SpellToCast.Cooldown) castingMagic = false;

            if(SpellToCast.Cooldown - currentCastTimer > 0 )
            {
                textCooldown.enabled = true;
                iconCooldown.enabled = true;
                textCooldown.text = (SpellToCast.Cooldown - currentCastTimer).ToString("F1");
                iconCooldown.fillAmount = (SpellToCast.Cooldown - currentCastTimer) / SpellToCast.Cooldown;
            } else
            {
                textCooldown.enabled = false;
                iconCooldown.enabled = false;
            } 
        }
        if(currentMana < maxMana)
        {
            currentManaRechargeTimer += Time.deltaTime;
            if(currentManaRechargeTimer > timeToWaitForManaRecharge)
            {
            currentMana += manaRechargeRate * Time.deltaTime;
            if(currentMana > maxMana)  currentMana = maxMana;
            }
        }
        // if(shootAction.ReadValue<float>() > 0) CastPrimarySpell();
    }

    private void CastPrimarySpell(){
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(SpellToCast.SpellPrefab, castPoint.position, castPoint.rotation, spellStoreParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, PlayerLayerMask)){
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else {
            bulletController.target = cameraTransform.position + cameraTransform.forward * SpellToCast.HitMissLifetime;
            bulletController.hit = false;
        }
        // float iTweenDistance = Vector3.Distance(hit.point, castPoint.transform.position);
        // iTween.PunchPosition(bullet, new Vector3 (0, iTweenDistance/20, 0), iTweenDistance/5);
    }
}