using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition = null;

    private void Awake()
    {
        if (!_snapPosition)
            Debug.LogError(name + ": Snap Position Transform not found!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ledge_Grab_Checker"))
        {
            var player = other.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                player.GrabLedge(_snapPosition.position);
            }
        }
    }
}
