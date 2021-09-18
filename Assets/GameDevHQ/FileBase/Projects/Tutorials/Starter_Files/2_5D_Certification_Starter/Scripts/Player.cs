using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _climbSpeed = 1.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 1f;
    [SerializeField] private Transform _model = null;
    [SerializeField] private Vector3 _standUpOffset = new Vector3(0, 7.05012f, -1.1392f);
    [SerializeField] private float _rollingHeight = 1f;
    [SerializeField] private Vector3 _rollingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float _deathZone = 8f;
    [SerializeField] private Transform _respawnPoint = null;
    [SerializeField] private Quaternion _victoryRotation = new Quaternion(0, 90f, 0, 0);
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    [SerializeField] private float _xZoom = 10f;


    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;
    private bool _isJumping = false;
    private Vector3 _facing;
    private bool _isHanging = false;
    private bool _canClimb = false;
    private Vector3 _controllerOriginalCenter;
    private float _controllerOriginalHeight;
    private bool _isRolling = false;
    private bool _isCheering = false;

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

        _controllerOriginalCenter = _controller.center;
        _controllerOriginalHeight = _controller.height;

        _facing = _model.eulerAngles;
    }

    private void Start()
    {
        if (!_virtualCamera)
            Debug.LogError(name + ": Virtual Camera not found!");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isCheering)
        {
            if (_controller.enabled)
                CalculateMovement();

            if (_isHanging && Input.GetKeyDown(KeyCode.E))
                ClimbUp();
        }
    }

    private void CalculateMovement()
    {
        if (_canClimb)
        {
            _velocity.z = 0;
            float verticalMovement = Input.GetAxisRaw("Vertical");
            _velocity.y = verticalMovement * _climbSpeed;
            _animator.SetFloat("ClimbSpeed", verticalMovement);

            if (Input.GetKeyDown(KeyCode.E))
            {
                _canClimb = false;
                _animator.SetBool("IsClimbing", _canClimb);
                return;
            }
        }
        else
        {
            if (_controller.isGrounded && !_isRolling)
            {
                if (_isJumping)
                {
                    _isJumping = false;
                    _animator.SetBool("IsJumping", _isJumping);
                }

                #region Velocity and Facing

                _velocity.y = 0;
                _velocity.z = Input.GetAxisRaw("Horizontal") * _speed;

                if (_velocity.z > 0)
                    _facing.y = 0;
                else if (_velocity.z < 0)
                    _facing.y = 180f;
                _model.eulerAngles = _facing;

                _animator.SetFloat("Speed", Mathf.Abs(_velocity.z));

                #endregion

                if (Input.GetButtonDown("Jump"))
                {
                    _velocity.y += _jumpHeight;

                    _isJumping = true;
                    _animator.SetBool("IsJumping", _isJumping);
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _isRolling = true;
                    _animator.SetTrigger("DiveRoll");
                    _controller.center = _rollingCenter;
                    _controller.height = _rollingHeight;

                    if (_facing.y == 0)
                    {
                        _velocity = new Vector3(0, 0, _speed);
                    }
                    else
                    {
                        _velocity = new Vector3(0, 0, -_speed);
                    }
                }
            }

            _velocity.y += _gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);

        if (transform.position.y < _deathZone)
        {
            transform.position = _respawnPoint.position;
        }
    }

    public void GrabLedge(Vector3 snapPosition)
    {
        _controller.enabled = false;
        _animator.SetBool("IsGrabbingLedge", true);
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
        _animator.SetBool("IsClimbing", _canClimb);
    }

    public void DisableLadderClimb()
    {
        _canClimb = false;
        _animator.SetBool("IsClimbing", _canClimb);
    }

    public void Teleport(Transform target, bool isFacingRight)
    {
        _controller.enabled = false;

        // Face correct direction and give a push off ladder
        if (isFacingRight)
        {
            _facing.y = 0;
            _velocity = Vector3.zero;
        }
        else
        {
            _facing.y = 180f;
            _velocity = Vector3.zero;
        }
        _model.eulerAngles = _facing;

        transform.position = target.position;
        _controller.enabled = true;
    }

    public void FinishRoll()
    {
        _velocity = Vector3.zero;
        RestoreControllerSize();
    }

    private void RestoreControllerSize()
    {
        _controller.center = _controllerOriginalCenter;
        _controller.height = _controllerOriginalHeight;
        _isRolling = false;
    }

    public bool IsRolling()
    {
        return _isRolling;
    }

    public void Dance()
    {
        _animator.SetBool("IsCheering", true);
        _model.localRotation = _victoryRotation;
        _isCheering = true;
        _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x = _xZoom;
    }
}
