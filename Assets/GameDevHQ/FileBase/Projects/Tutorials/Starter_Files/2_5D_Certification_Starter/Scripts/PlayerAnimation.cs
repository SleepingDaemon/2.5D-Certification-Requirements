using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Player _player;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            print("Animator is null");

        _player = GetComponent<Player>();
        if (_player == null)
            print("Player is null");
    }

    private void Update()
    {
        Vector3 _velocity   = _player.GetVelocity();
        bool    _isJumping  = _player.IsJumping();
        bool    _isHanging  = _player.IsHanging();

        _animator.SetFloat("speed", Mathf.Abs(_velocity.z));
        _animator.SetBool("isJumping", _isJumping);
        _animator.SetBool("canLedgeGrab", _isHanging);
    }
}
