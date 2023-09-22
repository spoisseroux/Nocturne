using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public bool isPaused = false;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public bool jumpingAllowed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    public float runMultiplier;

    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;

    [Header("Audio")]
    public AudioSource footstepsSound;
    public float pitchModifier;

    float horizontalInput;
    float verticalInput;
    float originalPitch;
    float pitch;

    Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        originalPitch = footstepsSound.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check ray cast
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        //apply drag on ground
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        //FIX SOUND CODE LATER
        //walk sound
        if (Input.GetKey(KeyCode.W) && (grounded) && !Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.A) && (grounded) && !Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S) && (grounded) && !Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D) && (grounded) && !Input.GetKey(KeyCode.LeftShift))
        {
            if (Random.value < 0.5f)
                pitch = originalPitch;
            else
                pitch = originalPitch * pitchModifier;

            footstepsSound.pitch = pitch;
            footstepsSound.enabled = true;
        }
        //run sound
        else if (Input.GetKey(KeyCode.W) && (grounded) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.A) && (grounded) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S) && (grounded) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D) && (grounded) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Random.value < 0.5f)
                pitch = originalPitch * 2.0f;
            else
                pitch = originalPitch * 2.0f * pitchModifier;

            footstepsSound.pitch = pitch;
            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.pitch = originalPitch;
            footstepsSound.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isPaused == false)
        {
            MovePlayer();
        }
        else {
            footstepsSound.enabled = false;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded && jumpingAllowed == true)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground and walk
        if (grounded && !Input.GetKey(runKey))
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        //on ground and run
        else if (grounded && Input.GetKey(runKey))
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * runMultiplier, ForceMode.Force);
        }

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
