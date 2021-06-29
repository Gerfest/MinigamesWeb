
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

    public float velocity;
    [Space]

	public float inputX;
	public float inputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float horizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float verticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float startAnimTime = 0.3f;
    [Range(0, 1f)]
    public float stopAnimTime = 0.15f;

    public float verticalVel;
    private Vector3 _moveVector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		InputMagnitude ();
		/*
        isGrounded = controller.isGrounded;
        
        if (isGrounded)
        {
            verticalVel = 0;
        }
        else
        {
            verticalVel -= 1;
        }
        _moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(_moveVector);*/
	}

    void PlayerMoveAndRotation() {
		inputX = Input.GetAxis ("Horizontal");
		inputZ = Input.GetAxis ("Vertical");
		var transform1 = cam.transform;
		var forward = transform1.forward;
		var right = transform1.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * inputZ + right * inputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * (Time.deltaTime * velocity));
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {
        var transform1 = cam.transform;
        var forward = transform1.forward;
        var right = transform1.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    private void InputMagnitude() {
		//Calculate Input Vectors
		inputX = Input.GetAxis ("Horizontal");
		inputZ = Input.GetAxis ("Vertical");

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		speed = new Vector2(inputX, inputZ).sqrMagnitude;

        //Physically move player

		if (speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", speed, startAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", speed, stopAnimTime, Time.deltaTime);
		}
	}
}
