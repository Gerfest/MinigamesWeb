using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 1;
    public float runSpeed = 2;
    public Transform cameraTransform;
    public float allowPlayerRotation = 0.1f;

    public float mouseX = 150;
    public float mouseY = 100;

    public float minAngle = 30;
    public float maxAngle = 60;
    private Rigidbody _rBody;
    private Animator _animator;

    private float _angle;
    private bool _jumpOrder;
    private Vector3 _motionInput; 
    public int keysCollected = 0;
    
    [Range(0,1f)]
    public float startAnimTime = 0.3f;
    [Range(0, 1f)]
    public float stopAnimTime = 0.15f;

    void Start()
    {
        _rBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.rotation *= Quaternion.Euler(0, mouseInput.x * mouseX * Time.deltaTime, 0);
        _angle = Mathf.Clamp(_angle - mouseInput.y * mouseY * Time.deltaTime, minAngle, maxAngle);
        cameraTransform.localRotation = Quaternion.Euler(_angle, 0, 0);
        _jumpOrder |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputZ = Input.GetAxis("Vertical");
        var motionInput = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var velocity = _rBody.velocity;
        motionInput.x += velocity.x;
        motionInput.z += velocity.z;
        var speed = Input.GetButton("Fire3") ? runSpeed : moveSpeed;
        motionInput = Vector3.ClampMagnitude(motionInput, speed);
        motionInput.y = _rBody.velocity.y;
        _rBody.velocity = motionInput;
        
        var speedCh = new Vector2(inputX, inputZ).sqrMagnitude;
        if (_jumpOrder)
        {
            _jumpOrder = false;
            _rBody.velocity += new Vector3(0, jumpForce, 0);
        }
        
        if (speedCh > allowPlayerRotation) {
            _animator.SetFloat ("Blend", speedCh, startAnimTime, Time.deltaTime);
        } else if (speedCh < allowPlayerRotation)
        {
            _animator.SetFloat("Blend", speedCh, stopAnimTime, Time.deltaTime);
        }
    }

    public void GetKey()
    {
        keysCollected++;
    }
}