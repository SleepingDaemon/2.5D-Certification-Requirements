using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 23f;
    [SerializeField] private float _gravity = 10f;
    [SerializeField] private bool isGrounded;
    private float _yVelocity;

    private CharacterController _controller;
    private Vector3 _direction;
    private Vector3 _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
            print("Controller is null");
    }

    private void Update()
    {
        isGrounded = _controller.isGrounded;

        float z = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            _direction = new Vector3(0, 0, z);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
            }
        }
        else
        {
            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;

        _controller.Move(_velocity * Time.deltaTime);
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}
