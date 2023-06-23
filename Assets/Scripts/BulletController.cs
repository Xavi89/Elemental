using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    [SerializeField] private GameObject bulletDecal;
    private float speed = 80f;
    private float timeToDestroy = 3f;

    public Vector3 target {get; set;}
    public bool hit {get; set;}

    private void onEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .1f ){
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            HealthComponent enemyHealth = other.gameObject.GetComponent<HealthComponent>();
            if(SpellToCast.DamageAmount > 0) enemyHealth.TakeDamage(SpellToCast.DamageAmount);
        }
        ContactPoint contact = other.GetContact(0);
        var bulletDecalInstance = Instantiate(bulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
        Destroy(bulletDecalInstance, 1f);
        Destroy(gameObject);
    }
}
