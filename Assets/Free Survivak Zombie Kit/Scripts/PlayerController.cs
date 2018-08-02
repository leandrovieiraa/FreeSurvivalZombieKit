using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator handsAnim;

    [Header("Speed System")]
    public float walkSpeed = 5.0f;
    public float sneakSpeed = 2.5f;
    public float runSpeed = 8.0f;
    public float crouchWalkSpeed = 3.5f;
    public float crouchRunSpeed = 6.5f;
    public float crouchSneakSpeed = 1f;
    public float jumpSpeed = 6.0f;
    public bool limitDiagonalSpeed = true;
    public bool toggleRun = false;
    public bool toggleSneak = false;
    public bool airControl = false;
    public bool crouching = false;

    public enum motionstate
    {
        idle,
        running,
        jumping
    }
    [Header("Motion System")]
    public motionstate currentMotion;

    [Header("Gravity system")]
    public float gravity = 10.0f;
    public float fallingDamageLimit = 10.0f;
    private bool grounded;

    [Header("Input System")]
    public KeyCode inventoryKey = new KeyCode();
    public KeyCode equipmentKey = new KeyCode();

    [Header("GameObjects")]
    public GameObject camera;
    public GameObject inventory;
    public GameObject equipment;

    // Private Variables
    private Vector3 moveDirection;
    private CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private bool punching;
    private bool playerControl;
    private Crosshair crosshairScript;

    // Use this for initialization
    void Start()
    {
        currentMotion = motionstate.idle;
        moveDirection = Vector3.zero;
        grounded = false;
        playerControl = false;
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        crosshairScript = transform.Find("FPSCamera").GetComponent<Crosshair>();

        // Lock cursor
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? 0.6701f : 1.0f;

        if (inputX == 0 && inputY == 0)
        {
            handsAnim.SetBool("idle", true);
            handsAnim.SetBool("running", false);
            currentMotion = motionstate.idle;
        }

        if (grounded)
        {
            if (falling)
            {
                falling = false;
                if (myTransform.position.y < (fallStartLevel - fallingDamageLimit))
                {
                    FallingDamageAlert(fallStartLevel - myTransform.position.y);
                }
            }

            if (!toggleRun)
            {
                bool running = Input.GetButton("Run");
                speed = running ? runSpeed : walkSpeed;
                handsAnim.SetBool("running", running);
                handsAnim.SetBool("idle", false);

                if (running)
                {
                    currentMotion = motionstate.running;
                    crosshairScript.IncreaseSpread(0.5f);
                }
                else
                {
                    if (crosshairScript.spread != crosshairScript.minSpread)
                        crosshairScript.DecreaseSpread(2f);
                }
            }

            if (Input.GetButtonUp("Run"))
            {
                currentMotion = PlayerController.motionstate.idle;
                handsAnim.SetBool("running", false);
                handsAnim.SetBool("idle", true);
            }

            if (!toggleSneak)
            {
                bool sneaking = Input.GetButton("Sneak");
                speed = sneaking ? sneakSpeed : speed;
                //anim.SetBool("sneaking", sneaking);
            }

            if (crouching)
            {
                speed = Input.GetButton("Run") ? crouchRunSpeed : crouchWalkSpeed;
                speed = Input.GetButton("Sneak") ? crouchSneakSpeed : speed;
            }

            moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor);
            moveDirection = myTransform.TransformDirection(moveDirection) * speed;

            if (!Input.GetButton("Jump"))
            {
                //anim.SetBool("Jump", false);
            }
            else
            {
                moveDirection.y = jumpSpeed;
                crosshairScript.IncreaseSpread(10f);
                currentMotion = motionstate.jumping;
            }
        }
        else
        {
            if (!falling)
            {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }

            if (airControl && playerControl)
            {
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputY * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        moveDirection.y -= gravity * Time.deltaTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
            inventory.active = !inventory.active;

        if (Input.GetKeyDown(equipmentKey))
            equipment.active = !equipment.active;

        if (inventory.activeSelf || equipment.activeSelf)
        {
            DisableController();
        }
        else
        {
            EnableController();
        }

        if (toggleRun && grounded && Input.GetButtonDown("Run"))
            speed = (speed == walkSpeed ? runSpeed : walkSpeed);

        if (Input.GetButtonUp("Crouch"))
        {
            crouching = !crouching;
            //anim.SetBool("Crouch", crouching);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //print(hit.point);
    }

    void FallingDamageAlert(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }

    void EnableController()
    {
        camera.GetComponent<MouseLook>().enabled = true;
        Cursor.visible = false;
    }

    void DisableController()
    {
        camera.GetComponent<MouseLook>().enabled = false;
        Cursor.visible = true;
    }
}