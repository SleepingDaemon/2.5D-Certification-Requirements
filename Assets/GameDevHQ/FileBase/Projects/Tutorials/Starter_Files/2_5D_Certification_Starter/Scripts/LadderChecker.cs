using UnityEngine;

public class LadderChecker : MonoBehaviour
{
    [SerializeField] private Transform standingPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LadderGrabChecker"))
        {
            Player _player = other.transform.parent.gameObject.GetComponent<Player>();
            if(_player != null)
            {
                print("Can Climb Up from Ladder!");
                if (_player.IsClimbing() == true)
                    _player.SetClimbingOffLadderTrigger(this);
            }
        }
    }

    public Vector3 GetStandingPosition() => standingPos.position;
}
