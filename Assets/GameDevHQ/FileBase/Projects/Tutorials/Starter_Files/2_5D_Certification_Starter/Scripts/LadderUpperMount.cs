using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderUpperMount : MonoBehaviour
{
    [SerializeField] private Ladder _ladder = null;
    [SerializeField] private Transform _snapPosition = null;
    [SerializeField] private bool _shouldPlayerFaceRight = false;

    private void Awake()
    {
        if (!_ladder)
            Debug.Log(name + ": Ladder object not found!");

        if (!_snapPosition)
            Debug.Log(name + ": SnapPosition Transform not found!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_ladder.IsBeingUsed())
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.Teleport(_snapPosition.position, _shouldPlayerFaceRight);
                player.EnableLadderClimb();
                _ladder.EnableLadderClimb();
            }
        }
    }
}
