using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Material onHitMaterial;
    private Material initialMaterial;

    private void Start()
    {
        initialMaterial = GetComponent<Renderer>().material;
    }
    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damageToApply)
    {
        currentHealth -= damageToApply;
        if (currentHealth <= 0) Destroy(this.gameObject);
        healthBarImage.fillAmount = currentHealth / MaxHealth;
        GetComponent<Renderer>().material = onHitMaterial;
        Invoke("ChangeToInitialMaterial", .05f);
        DamagePopUpGenerator.current.CreatePopUp(transform.position, damageToApply.ToString(), Color.white);
    }

    public void ChangeToInitialMaterial()
    {
        GetComponent<Renderer>().material = initialMaterial;
    }

}
