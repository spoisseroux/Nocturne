using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{

    [HideInInspector] public bool isPaused = false;

    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    [SerializeField]
    private float _zoomSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField] private float _distanceFromTargetMin = 3.0f;
    [SerializeField] private float _distanceFromTargetMax = 3.0f;

    [SerializeField] private float _distanceFromTarget;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    void Update()
    {
        if (isPaused == false)
        {
            //float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            //float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            float mouseX = Input.GetAxis("Horizontal") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Vertical") * _mouseSensitivity;

            _rotationY += mouseX;
            _rotationX += mouseY;

            // Apply clamping for x rotation 
            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

            // Apply damping between rotation changes
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            _distanceFromTarget += Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
            _distanceFromTarget = Mathf.Clamp(_distanceFromTarget, _distanceFromTargetMin, _distanceFromTargetMax);

            // Substract forward vector of the GameObject to point its forward vector to the target
            transform.position = _target.position - transform.forward * _distanceFromTarget;
        }
        else {
            //Reset velocity
        }
    }
}