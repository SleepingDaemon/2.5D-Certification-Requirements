using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform _pointA, _pointB;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _waitTime = 5f;
    private bool _switch = false;

    private void FixedUpdate()
    {
        if (!_switch)
            transform.position = Vector3.MoveTowards(transform.position, _pointA.position, _speed * Time.fixedDeltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed * Time.fixedDeltaTime);

        if (transform.position == _pointA.position)
            StartCoroutine(WaitBeforeSwitchingRoutine(true));
        else if (transform.position == _pointB.position)
            StartCoroutine(WaitBeforeSwitchingRoutine(false));
    }

    IEnumerator WaitBeforeSwitchingRoutine(bool _canSwitch)
    {
        yield return new WaitForSeconds(_waitTime);
        _switch = _canSwitch;
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
