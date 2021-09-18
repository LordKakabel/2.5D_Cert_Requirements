using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallElevator : MonoBehaviour
{
    [SerializeField] private Elevator _elevator = null;
    [SerializeField] private int _targetFloor = 0;
    [SerializeField] private GameObject _UIText = null;

    private bool _isOccupied = false;

    private void Start()
    {
        if (!_UIText)
            Debug.LogError(name + ": UIText object not found!");

        if (!_elevator)
            Debug.LogError(name + ": Elevator not found!");
    }

    private void Update()
    {
        if (_isOccupied && Input.GetKeyDown(KeyCode.R))
        {
            _elevator.Recall(_targetFloor);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isOccupied = true;
            _UIText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isOccupied = false;
            _UIText.SetActive(false);
        }
    }
}
