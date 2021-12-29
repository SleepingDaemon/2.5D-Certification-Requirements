using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _smoothHandSnap = 11f;
    [SerializeField] private float _jumpHeight = 23f;
    [SerializeField] private float _gravity = 10f;
    [SerializeField] private bool _isGrounded;
    private bool _isJumping = false;
    private bool _isHanging = false;
    private float _yVelocity;

    [SerializeField] private int _coins = 0;

    private CharacterController _controller;
    private Animator _animator;

    private Vector3 _direction;
    private Vector3 _velocity;
    private Vector3 _handPos;
    private LedgeChecker _activeLedge;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
            print("Controller is null");

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            print("Animator is null");
    }

    private void Update()
    {
        PlayerMovement();

        if (_isHanging &&  Input.GetKeyDown(KeyCode.W))
        {
            _isHanging = false;
            _animator.SetTrigger("climbUp");
            _animator.SetBool("canLedgeGrab", false);
        }

        if (_isHanging)
        {
            transform.position = Vector3.MoveTowards(transform.position, _handPos, _smoothHandSnap * Time.deltaTime);
        }
    }

    private void PlayerMovement()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded)
        {
            if (_isJumping)
            {
                _isJumping = false;
                _animator.SetBool("isJumping", _isJumping);
            }

            float z = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, z);
            _velocity = _direction * _speed;
            _animator.SetFloat("speed", Mathf.Abs(z));

            if (z != 0)
            {
                Vector3 getFacing = transform.localEulerAngles;
                getFacing.y = _velocity.z > 0 ? 0 : 180;
                transform.localEulerAngles = getFacing;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _isJumping = true;
                _animator.SetBool("isJumping", _isJumping);
            }
        }
        else if (!_isGrounded)
        {
            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;

        _controller.Move(_velocity * Time.deltaTime);
    }

    public void GrabLedge(Vector3 handPos, LedgeChecker currentLedge)
    {
        print("Is Ledge Grabbing!");
        _controller.enabled = false;
        _isHanging = true;
        _animator.SetBool("canLedgeGrab", true);
        _animator.SetFloat("speed", 0.0f);
        _animator.SetBool("isJumping", false);

        //Update Position to hand pos
        _handPos = handPos;

        _activeLedge = currentLedge;
    }

    public void FinishedClimbing()
    { 
        _isHanging = false;

        if(_activeLedge != null)
            gameObject.transform.position = _activeLedge.GetStandingPosition();

        _animator.SetBool("canLedgeGrab", false);
        _controller.enabled = true;
    }

    public void AddCoin()
    {
        _coins += 1;
    }

    public int GetCoins() => _coins;
}
