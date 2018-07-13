using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator weaponAnimator;

    void Update ()
    {
        if (playerController.currentMotion == PlayerController.motionstate.idle)
        {
            // weapon idle position
            weaponAnimator.SetBool("running", false);
        }

        if (playerController.currentMotion == PlayerController.motionstate.running)
        {
            // weapon running position
            weaponAnimator.SetBool("running", true);
        }
    }
}
