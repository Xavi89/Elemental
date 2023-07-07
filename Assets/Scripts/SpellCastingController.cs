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
    [SerializeField] public SpellScriptableObject SpellToCast2;

   
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform spellStoreParent;
    [SerializeField] private LayerMask hitLayerMask;
    
    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    private InputAction shootAction;
    private InputAction shootSecondaryAction;

    private PlayerMagicSystem manaSystem;

    private bool castingMagic = false;
    private bool castingMagic2 = false;

    [SerializeField] private Image icon;
    [SerializeField] private Image icon2;
    [SerializeField] private Image iconCooldown;
    [SerializeField] private Image iconCooldown2;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private TMP_Text textCooldown2;
    [SerializeField] private float currentCastTimer;
    [SerializeField] private float currentCastTimer2;

    private float currentManaRechargeTimer;

    [Header("Mana")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] public float currentMana;
    [SerializeField] private float manaRechargeRate = 10f;
    [SerializeField] private float timeToWaitForManaRecharge = 1f;
    [SerializeField] private Image manaBarImage;
    [SerializeField] private TextMeshProUGUI manaText;

    [SerializeField] LineRenderer lineRend;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        shootAction = playerInput.actions["Shoot"];
        shootSecondaryAction = playerInput.actions["Spell Cast"];

        currentMana = maxMana;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        textCooldown.text = "";
        textCooldown2.text = "";
        icon.sprite = SpellToCast.SpellIcon;
        icon2.sprite = SpellToCast2.SpellIcon;
        iconCooldown.sprite = SpellToCast.SpellIcon;
        iconCooldown2.sprite = SpellToCast2.SpellIcon;
        // iconCooldown.GetComponent<Image>().color = new Color32(255,0,0,100);
        iconCooldown.fillAmount = 0;
        iconCooldown2.fillAmount = 0;
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, hitLayerMask)){
            Debug.DrawRay(castPoint.transform.position, hit.point, Color.green);
        }
        lineRend.SetPosition(0, castPoint.transform.position);
        lineRend.SetPosition(1, hit.point);
        manaText.text = currentMana.ToString("F0") + " / " + maxMana.ToString("F0");
        manaBarImage.fillAmount = currentMana/maxMana;
        bool isSpellCastHeldDown = shootAction.ReadValue<float>() > 0;
        bool isSpellCastHeldDown2 = shootSecondaryAction.ReadValue<float>() > 0;
        bool hasEnoughMana = currentMana - SpellToCast.ManaCost >= 0f;
        bool hasEnoughMana2 = currentMana - SpellToCast2.ManaCost >= 0f;
        if(!castingMagic && isSpellCastHeldDown && hasEnoughMana)
        {
            castingMagic = true;
            currentMana -= SpellToCast.ManaCost;
            currentCastTimer = 0;
            currentManaRechargeTimer = 0;
            CastPrimarySpell();
        }
        if(!castingMagic2 && isSpellCastHeldDown2 && hasEnoughMana2)
        {
            castingMagic2 = true;
            currentMana -= SpellToCast2.ManaCost;
            currentCastTimer2 = 0;
            currentManaRechargeTimer = 0;
            CastSecondarySpell();
        }
        if(castingMagic)
        {
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > SpellToCast.Cooldown) castingMagic = false;

            if(SpellToCast.Cooldown - currentCastTimer > 0 )
            {
                textCooldown.enabled = true;
                iconCooldown.enabled = true;
                textCooldown.text = (SpellToCast.Cooldown - currentCastTimer).ToString("F0");
                iconCooldown.fillAmount = (SpellToCast.Cooldown - currentCastTimer) / SpellToCast.Cooldown;
            } else
            {
                textCooldown.enabled = false;
                iconCooldown.enabled = false;
            } 
        }
        if(castingMagic2)
        {
            currentCastTimer2 += Time.deltaTime;
            if(currentCastTimer2 > SpellToCast2.Cooldown) castingMagic2 = false;

            if(SpellToCast2.Cooldown - currentCastTimer2 > 0 )
            {
                textCooldown2.enabled = true;
                iconCooldown2.enabled = true;
                textCooldown2.text = (SpellToCast2.Cooldown - currentCastTimer2).ToString("F0");
                iconCooldown2.fillAmount = (SpellToCast2.Cooldown - currentCastTimer2) / SpellToCast2.Cooldown;
            } else
            {
                textCooldown2.enabled = false;
                iconCooldown2.enabled = false;
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
    }

    private void CastPrimarySpell(){
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(SpellToCast.SpellPrefab, castPoint.position, castPoint.rotation, spellStoreParent);
        SpellController spellController = bullet.GetComponent<SpellController>();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, hitLayerMask)){
            spellController.target = hit.point;
            spellController.hit = true;
        }
        else {
            spellController.target = cameraTransform.position + cameraTransform.forward * SpellToCast.HitMissLifetime;
            spellController.hit = false;
        }
        // float iTweenDistance = Vector3.Distance(hit.point, castPoint.transform.position);
        // iTween.PunchPosition(bullet, new Vector3 (0, iTweenDistance/20, 0), iTweenDistance/5);
    }

        private void CastSecondarySpell(){
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(SpellToCast2.SpellPrefab, castPoint.position, castPoint.rotation, spellStoreParent);
        SpellController spellController = bullet.GetComponent<SpellController>();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, hitLayerMask)){
            spellController.target = hit.point;
            spellController.hit = true;
        }
        else {
            spellController.target = cameraTransform.position + cameraTransform.forward * SpellToCast2.HitMissLifetime;
            spellController.hit = false;
        }
        // float iTweenDistance = Vector3.Distance(hit.point, castPoint.transform.position);
        // iTween.PunchPosition(bullet, new Vector3 (0, iTweenDistance/20, 0), iTweenDistance/5);
    }
}