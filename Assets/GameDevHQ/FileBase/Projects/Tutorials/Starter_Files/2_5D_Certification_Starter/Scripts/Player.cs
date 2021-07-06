using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 1f;

    private CharacterController _controller;
    private Vector3 _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (!_controller)
            Debug.LogError(name + ": CharacterController component not found!");
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
            _velocity.z = Input.GetAxis("Horizontal") * _speed;

            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y += _jumpHeight;
            }
        }

        _velocity.y += _gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
