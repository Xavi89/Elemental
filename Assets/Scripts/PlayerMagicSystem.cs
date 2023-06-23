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

    [Header("Mana")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana;
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

        }
        if(currentMana < maxMana && !castingMagic && !isSpellCastHeldDown)
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