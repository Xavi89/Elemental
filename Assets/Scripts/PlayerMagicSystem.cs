using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMagicSystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;

    private float currentManaRechargeTimer;
    [SerializeField] private float currentCastTimer;
    [SerializeField] private Transform castPoint;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Image iconCooldown;

    [Header("Mana")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] public float currentMana;
    [SerializeField] private float manaRechargeRate = 10f;
    [SerializeField] private float timeToWaitForManaRecharge = 1f;
    [SerializeField] private Image manaBarImage;
    [SerializeField] private TextMeshProUGUI manaText;


    private bool castingMagic = false;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        currentMana = maxMana;
        textCooldown.text = "";
        iconCooldown.fillAmount = 0;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        manaText.text = currentMana.ToString("F0") + " / " + maxMana.ToString("F0");
        manaBarImage.fillAmount = currentMana/maxMana;
        bool isSpellCastHeldDown = playerControls.Player.SpellCast.ReadValue<float>() > 0.1;
        bool hasEnoughMana = currentMana - spellToCast.SpellToCast.ManaCost >= 0f;
        if(!castingMagic && isSpellCastHeldDown && hasEnoughMana)
        {
            castingMagic = true;
            currentMana -= spellToCast.SpellToCast.ManaCost;
            currentCastTimer = 0;
            currentManaRechargeTimer = 0;
            CastSpell();
        }
        if(castingMagic)
        {
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > spellToCast.SpellToCast.Cooldown) castingMagic = false;

            if(spellToCast.SpellToCast.Cooldown - currentCastTimer > 0 )
            {
                textCooldown.enabled = true;
                iconCooldown.enabled = true;
                textCooldown.text = (spellToCast.SpellToCast.Cooldown - currentCastTimer).ToString("F1");
                iconCooldown.fillAmount = (spellToCast.SpellToCast.Cooldown - currentCastTimer) / spellToCast.SpellToCast.Cooldown;
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
    }
    void CastSpell()
    {
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }

}