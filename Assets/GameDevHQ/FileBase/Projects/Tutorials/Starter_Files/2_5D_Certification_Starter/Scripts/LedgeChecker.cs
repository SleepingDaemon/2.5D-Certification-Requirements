using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LedgeGrabChecker"))
        {
            print("Player can ledge grab!");
            var _player = other.GetComponentInParent<Player>();

            if(_player != null)
            {
                _player.GrabLedge();
            }
        }
    }
}
