using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA, _pointB;
    [SerializeField] private float _speed = 3f;
    private bool _switch = false;

    private void FixedUpdate()
    {
        if (!_switch)
            transform.position = Vector3.MoveTowards(transform.position, _pointA.position, _speed * Time.fixedDeltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed * Time.fixedDeltaTime);

        if (transform.position == _pointA.position)
            _switch = true;
        else if (transform.position == _pointB.position)
            _switch = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
