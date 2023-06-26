using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptableObject : ScriptableObject
{
    public string SpellName = "";
    public float DamageAmount = 10f;
    public float ManaCost = 5f;
    public float Lifetime = 2f;
    public float Speed = 15f;
    public float SpellRadius = 1f;
    public AudioClip SpellAudioCast;
    public AudioClip SpellAudioHit;
    public float Cooldown = 1f;
    public Texture2D SpellIcon = null;
    public GameObject SpellPrefab = null;
    public GameObject SpellCollisionParticles = null;

    // Element School
    // Status effect
    // Time between casts
}