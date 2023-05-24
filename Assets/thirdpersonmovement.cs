using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Animator characterAnimator;

    public AudioSource footstepsSound;

    public float pitchModifier;
    private float pitch;
    private float originalPitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalPitch = footstepsSound.pitch;
        footstepsSound.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if ((direction.magnitude >= 0.1f) || (controller.isGrounded == false))
        {
            if (controller.isGrounded == true)
            {
                //handle walking anim
                characterAnimator.SetBool("isRunning", true);

                //handle walking sound
                if (Random.value < 0.5f)
                    pitch = originalPitch;
                else
                    pitch = originalPitch * pitchModifier;

                footstepsSound.pitch = pitch;
                footstepsSound.enabled = true;

            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Check if cjharacter is grounded
            if (controller.isGrounded == false)
            {
                //Add our gravity Vecotr
                moveDir += Physics.gravity;
            }

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            Debug.Log("Moving");
        }
        else {
            //handle walk anim
            characterAnimator.SetBool("isRunning", false);
            footstepsSound.enabled = false;
        }

            
        
    }
}
