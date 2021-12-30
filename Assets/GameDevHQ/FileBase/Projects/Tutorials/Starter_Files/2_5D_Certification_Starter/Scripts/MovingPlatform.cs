using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingPlatformType { None, Elevator, Platform }

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MovingPlatformType _type;
    [SerializeField] private Transform _pointA, _pointB;
    [SerializeField] private float _speed = 3f;
    private float _waitTime = 5f;
    private bool _switch = false;

    private void FixedUpdate()
    {
        if (!_switch)
            transform.position = Vector3.MoveTowards(transform.position, _pointA.position, _speed * Time.fixedDeltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed * Time.fixedDeltaTime);

        switch (_type)
        {
            case MovingPlatformType.Elevator:
                if (transform.position == _pointA.position)
                    StartCoroutine(WaitBeforeSwitchingRoutine(true));
                else if (transform.position == _pointB.position)
                    StartCoroutine(WaitBeforeSwitchingRoutine(false));
                break;
            case MovingPlatformType.Platform:
                if (transform.position == _pointA.position)
                    _switch = true;
                else if (transform.position == _pointB.position)
                    _switch = false;
                break;
            default:
                print("Please Select a type for the moving platform!");
                break;
        }
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
