using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    [SerializeField] private Vector3 handPosition;
    [SerializeField] private Transform _stand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LedgeGrabChecker"))
        {
            print("Player can ledge grab!");
            var _player = other.GetComponentInParent<Player>();

            if(_player != null)
            {
                _player.GrabLedge(handPosition, this);
            }
        }
    }

    public Vector3 GetStandingPosition() => _stand.position;
}
