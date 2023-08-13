using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{

    [SerializeField] public bool isPaused = false; // [HideInInspector]
    public bool justUnPaused = false;

    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    [SerializeField]
    private float _zoomSensitivity = 3.0f;

    [SerializeField] private float _rotationY;
    [SerializeField] private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField] private float _distanceFromTargetMin = 3.0f;
    [SerializeField] private float _distanceFromTargetMax = 3.0f;

    [SerializeField] private float _distanceFromTarget;

    [SerializeField] private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    [SerializeField]
    private Vector3 nextRotation = Vector3.zero;

    float mouseX = 0.0f;
    float mouseY = 0.0f;


    void Update()
    {
        if (isPaused == false)
        {
            //float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            //float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
            mouseX = Input.GetAxis("Horizontal") * _mouseSensitivity;
            mouseY = Input.GetAxis("Vertical") * _mouseSensitivity;

            if (justUnPaused)
            {
                ResetSmooth();
            }

            _rotationY += mouseX;
            _rotationX += mouseY;

            // Apply clamping for x rotation 
            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            nextRotation = new Vector3(_rotationX, _rotationY); // Vector3 local variable before serializefield addition

            // Apply damping between rotation changes
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            _distanceFromTarget += Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
            _distanceFromTarget = Mathf.Clamp(_distanceFromTarget, _distanceFromTargetMin, _distanceFromTargetMax);

            // Subtract forward vector of the GameObject to point its forward vector to the target
            transform.position = _target.position - transform.forward * _distanceFromTarget;

            // _smoothTime = 0.2f; // this is causing the spin after interacts end, if we do not reset to 0.2f it fixes the issue
            // maybe we lerp a coroutine to gradually increase it to 0.2f again???
            justUnPaused = false;
        }
        else
        {
            _smoothTime = 2f;
            _smoothVelocity = Vector3.zero;
        }
    }

    // brittle fix
    public void ResetSmooth()
    {
        StartCoroutine(ResetSmoothTime());
    }

    private IEnumerator ResetSmoothTime()
    {
        while (_smoothTime > 0.2f)
        {
            _smoothTime -= 0.1f;
            yield return new WaitForSeconds(0.09f);
        }

    }
}