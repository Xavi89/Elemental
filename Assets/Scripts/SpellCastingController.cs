using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class SpellCastingController : MonoBehaviour
{
    public Image fireSpellImage;
    public TMP_Text fireSpellText;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 100f;
    [SerializeField] private LayerMask PlayerLayerMask;
    [SerializeField] private Cooldown fireCooldown;
    public float fireSpellCooldown;
    private bool isFireSpellCooldown = false;
    private float currentFireSpellCooldown;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction shootAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        shootAction = playerInput.actions["Shoot"];

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        fireSpellImage.fillAmount = 0;
        fireSpellText.text = "";
    }

    void Update()
    {
        FireSpell();
        SpellCooldown(ref currentFireSpellCooldown, fireSpellCooldown, ref isFireSpellCooldown, fireSpellImage, fireSpellText);
    }
    private void FireSpell(){
        if(shootAction.triggered && !isFireSpellCooldown){
            isFireSpellCooldown = true;
            currentFireSpellCooldown = fireSpellCooldown;
        }
    }

    private void OnEnable() {
        shootAction.performed += _ => CastPrimarySpell();
    }

    private void OnDisable() {
        shootAction.performed -= _ => CastPrimarySpell();
    }

    private void CastPrimarySpell(){
        if(fireCooldown.IsCoolingDown) return;
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, PlayerLayerMask)){
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
        float iTweenDistance = Vector3.Distance(hit.point, barrelTransform.transform.position);
        iTween.PunchPosition(bullet, new Vector3 (0, iTweenDistance/20, 0), iTweenDistance/5);
        fireCooldown.StartCooldown();
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