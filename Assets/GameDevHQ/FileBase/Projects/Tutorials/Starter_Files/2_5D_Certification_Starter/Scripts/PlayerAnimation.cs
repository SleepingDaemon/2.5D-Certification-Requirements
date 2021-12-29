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


    }
}
