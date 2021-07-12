using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _climbSpeed = 1.5f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] Transform _model = null;
    [SerializeField] Vector3 _standUpOffset = new Vector3(0, 7.05012f, -1.1392f);

    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;
    private bool _isJumping = false;
    private Vector3 _facing;
    private bool _isHanging = false;
    private bool _canClimb = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (!_controller)
            Debug.LogError(name + ": CharacterController component not found!");

        _animator = GetComponentInChildren<Animator>();
        if (!_animator)
            Debug.LogError(name + ": Animator component not found in children!");

        if (!_model)
            Debug.LogError(name + ": Model child Transform not found.");

        _facing = _model.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.enabled)
            CalculateMovement();

        if (_isHanging && Input.GetKeyDown(KeyCode.E))
            ClimbUp();
    }

    private void CalculateMovement()
    {
        if (_canClimb)
        {
            _velocity.y = Input.GetAxisRaw("Vertical") * _climbSpeed;

            if (Input.GetKeyDown(KeyCode.E))
            {
                _canClimb = false;
                return;
            }
        }
        else
        {
            if (_controller.isGrounded)
            {
                if (_isJumping)
                {
                    _isJumping = false;
                    _animator.SetBool("IsJumping", _isJumping);
                }

                _velocity.y = 0;
                _velocity.z = Input.GetAxisRaw("Horizontal") * _speed;

                if (_velocity.z > 0)
                    _facing.y = 0;
                else if (_velocity.z < 0)
                    _facing.y = 180f;
                _model.eulerAngles = _facing;

                _animator.SetFloat("Speed", Mathf.Abs(_velocity.z));

                if (Input.GetButtonDown("Jump"))
                {
                    _velocity.y += _jumpHeight;

                    _isJumping = true;
                    _animator.SetBool("IsJumping", _isJumping);
                }
            }

            _velocity.y += _gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    public void GrabLedge(Vector3 snapPosition)
    {
        _controller.enabled = false;
        _animator.SetBool("IsGrabbingLedge", true);
        // Set other animation variables
        transform.position = snapPosition;
        _isHanging = true;
    }

    public void StandUp()
    {
        transform.position += _standUpOffset;
    }

    public void FinishStanding()
    {
        _isJumping = false;
        _animator.SetBool("IsJumping", _isJumping);
        _animator.SetBool("IsGrabbingLedge", false);
        _animator.SetFloat("Speed", 0);
        _controller.enabled = true;
    }

    private void ClimbUp()
    {
        _isHanging = false;
        _animator.SetTrigger("ClimbUp");
    }

    public void EnableLadderClimb()
    {
        _canClimb = true;
    }

    public void DisableLadderClimb()
    {
        _canClimb = false;
    }
}
