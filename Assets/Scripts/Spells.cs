using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spells : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    [Header("Fire Spell")]
    public Image fireSpellImage;
    public TMP_Text fireSpellText;
    public KeyCode fireSpellKey;
    public float fireSpellCooldown;

    private bool isFireSpellCooldown = false;
    private float currentFireSpellCooldown;

    // Start is called before the first frame update
    void Start()
    {
        fireSpellImage.fillAmount = 0;
        fireSpellText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        FireSpell();
        SpellCooldown(ref currentFireSpellCooldown, fireSpellCooldown, ref isFireSpellCooldown, fireSpellImage, fireSpellText);
    }
    private void FireSpell(){
        if(Input.GetKeyDown(fireSpellKey) && !isFireSpellCooldown){
            isFireSpellCooldown = true;
            currentFireSpellCooldown = fireSpellCooldown;
        }
    }
    private void SpellCooldown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, Image skillImage, TMP_Text skillText)
    {
        if(isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if(currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0f;
                if(skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }
                if(skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                if(skillImage != null)
                {
                    skillImage.fillAmount = currentCooldown / maxCooldown;
                }
                if(skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCooldown).ToString();
                }
            }
        }
    }
}
