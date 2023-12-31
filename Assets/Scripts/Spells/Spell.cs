using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;


    private void Awake() {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        Destroy(this.gameObject, SpellToCast.Lifetime);
    }

    private void Update() {
        if (SpellToCast.Speed > 0)
        {
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other) {
        AudioSource.PlayClipAtPoint(SpellToCast.SpellAudioHit, other.transform.position);
        GameObject collisionParticles = GameObject.Instantiate(SpellToCast.Collision, transform.position, Quaternion.identity);
        Destroy(collisionParticles, 1f);
        if(other.gameObject.CompareTag("Enemy"))
        {
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            if(SpellToCast.DamageAmount > 0) enemyHealth.TakeDamage(SpellToCast.DamageAmount);
        }
        Destroy(this.gameObject);
    }   
}
