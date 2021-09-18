using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderUpperMount : MonoBehaviour
{
    [SerializeField] private Ladder _ladder = null;
    [SerializeField] private Transform _mountPosition = null;
    [SerializeField] private Transform _dismountPosition = null;
    [SerializeField] private bool _shouldPlayerFaceRight = false;

    private void Awake()
    {
        if (!_ladder)
            Debug.Log(name + ": Ladder object not found!");

        if (!_mountPosition)
            Debug.Log(name + ": MountPosition Transform not found!");

        if (!_dismountPosition)
            Debug.Log(name + ": DimountPosition Transform not found!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_ladder.IsBeingUsed())
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                if (!player.IsRolling())
                {
                    player.Teleport(_mountPosition, _shouldPlayerFaceRight);
                    player.EnableLadderClimb();
                    _ladder.EnableLadderClimb();
                }
            }
        }
        else if (other.CompareTag("Player") && _ladder.IsBeingUsed())
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                if (!player.IsRolling())
                {
                    player.Teleport(_dismountPosition, _shouldPlayerFaceRight);
                    player.DisableLadderClimb();
                    _ladder.DisableLadderClimb();
                }
            }
        }
    }
}
