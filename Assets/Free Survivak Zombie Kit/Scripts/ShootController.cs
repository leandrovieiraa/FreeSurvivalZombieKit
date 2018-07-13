using UnityEngine;
using System.Collections;

public class ShootController : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Animator animator;
    public GameObject FPSCamera;

    public Camera fpsCam;
    public PlayerController playerController;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    public AudioSource gunAudio;
    public LineRenderer laserLine;
    private float nextFire;

    void Update()
    {
        if (playerController.currentMotion == PlayerController.motionstate.idle)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                nextFire = Time.time + fireRate;

                StartCoroutine(ShotEffect());

                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                RaycastHit hit;

                laserLine.SetPosition(0, gunEnd.position);

                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
                {
                    laserLine.SetPosition(1, hit.point);
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
                else
                {
                    laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
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

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}