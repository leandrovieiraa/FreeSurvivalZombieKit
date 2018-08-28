using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;

    [Header("Reload Settings")]
    public int bulletsPerClip = 12;
    public int maxNumberOfClips = 5;
    public int numberOfClips;   
    public int bulletsLeft;

    [Header("Weapon UI Settings")]
    public GameObject WeaponUI;
    public Text bulletNumberUI;
    public Text clipNumberUI;

    [Header("Weapon Objects")]
    public Transform gunEnd;
    public Animator animator;
    public GameObject FPSCamera;
    public Camera fpsCam;
    public PlayerController playerController;
    public AudioSource gunAudio;

    // Privates
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);  
    private float nextFire;

    private void OnEnable()
    {
        // Disable WeaponUI
        WeaponUI.SetActive(true);
    }

    private void OnDisable()
    {
        // Enable WeaponUI
        WeaponUI.SetActive(false);
    }

    private void Start()
    {
        // Set current values for bullet system
        numberOfClips = maxNumberOfClips;
        bulletsLeft = bulletsPerClip;    
    }

    void Update()
    {
        // Fix max bullets ammount
        if (bulletsLeft >= bulletsPerClip)
            bulletsLeft = bulletsPerClip;
        
        // Fix min bullets ammount
        if (bulletsLeft <= 0)
            bulletsLeft = 0;

        // Fix max clip ammount
        if (numberOfClips >= maxNumberOfClips)
            numberOfClips = maxNumberOfClips;

        // Fix min clip ammount
        if (numberOfClips <= 0)
            numberOfClips = 0;

        // Update Weapon UI
        if (WeaponUI.activeSelf)
        {
            bulletNumberUI.text = string.Format("Bullets {0}/{1}", bulletsLeft, bulletsPerClip);
            clipNumberUI.text = string.Format("Clips {0}/{1}", numberOfClips, maxNumberOfClips);
        }

        if (playerController.inventory.activeSelf)
            return;

        if (playerController.currentMotion == PlayerController.motionstate.idle)
        {
            // Press "R" for reload weapon
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Reload only if you have clips and already used some bullets
                if(numberOfClips > 0 && bulletsLeft < bulletsPerClip)
                {
                    // Log
                    Debug.Log("Reload weapon");

                    // Increase bullets
                    bulletsLeft = bulletsPerClip;

                    // Remove clip
                    numberOfClips -= 1;
                }
                else
                {
                    // Log
                    Debug.Log("Without ammo for reload weapon");
                }
            }
        }

        if (playerController.currentMotion == PlayerController.motionstate.idle)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                if(bulletsLeft <= 0)
                {
                    // Log
                    Debug.Log("Without ammo on weapon");
                
                    // Do nothing with no bullets
                    return;
                }

                // Remove ammo
                bulletsLeft -= 1;
                    
                nextFire = Time.time + fireRate;

                StartCoroutine(ShotEffect());

                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                RaycastHit hit;

                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
                {
                    EnemyController health = hit.collider.GetComponent<EnemyController>();

                    if (health != null)
                    {
                        health.Damage(gunDamage);
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * hitForce);
                    }
                }
            }
            else
            {
                animator.SetBool("fire", false);
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        animator.SetBool("fire", true);
        FPSCamera.GetComponent<Crosshair>().IncreaseSpread(10);

        yield return shotDuration;
    }
}