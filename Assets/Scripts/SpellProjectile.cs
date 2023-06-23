using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [SerializeField] private Transform explosionParticles;

    private Rigidbody bulletRigidbody;
    
    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float speed = 40f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}