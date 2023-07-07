using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    public Vector3 target {get; set;}
    public bool hit {get; set;}

    private void onEnable()
    {
        Destroy(gameObject, SpellToCast.Lifetime);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, SpellToCast.Speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .1f ){
            Destroy(gameObject);
        }
        Destroy(gameObject, SpellToCast.Lifetime);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            HealthComponent enemyHealth = other.gameObject.GetComponent<HealthComponent>();
            if(SpellToCast.DamageAmount > 0) enemyHealth.TakeDamage(SpellToCast.DamageAmount);
        }
        ContactPoint contact = other.GetContact(0);
        var spellCollision = Instantiate(SpellToCast.SpellCollisionParticles, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
        Destroy(spellCollision, 1f);
        Destroy(gameObject);
    }
}
