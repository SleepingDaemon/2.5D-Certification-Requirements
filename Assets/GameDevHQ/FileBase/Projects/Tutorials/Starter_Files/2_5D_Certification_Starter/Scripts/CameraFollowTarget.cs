using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        var x = transform.position.x;
        x = _target.position.x;
    }
}
