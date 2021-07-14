using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool _isBeingUsed = false;

    public bool IsBeingUsed()
    {
        return _isBeingUsed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.EnableLadderClimb();
                _isBeingUsed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.DisableLadderClimb();
                _isBeingUsed = false;
            }
        }
    }

    public void EnableLadderClimb()
    {
        _isBeingUsed = true;
    }
}
