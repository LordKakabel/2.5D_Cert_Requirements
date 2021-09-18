using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform[] _floors = null;
    [SerializeField] private int _currentFloor = 0;
    [SerializeField] private float _timeBetweenFloors = 5f;

    private bool _hasTarget = false;
    private bool _isPlayerOnFloor = false;
    /*private KeyCode[] _keyCodes ={
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9 };*/

    private void Update()
    {
        if (_isPlayerOnFloor && !_hasTarget)
        {
            /*for (int i = 0; i < _floors.Length; i++)
            {
                if (Input.GetKeyDown(_keyCodes[i]))
                {
                    _hasTarget = true;
                    int numberPressed = i;
                    StartCoroutine(MoveOverSeconds(_floors[numberPressed].position, _timeBetweenFloors));
                }
            }*/

            if (Input.GetKeyDown(KeyCode.UpArrow) && _currentFloor > 0)
            {
                _currentFloor--;
                StartCoroutine(
                    MoveOverSeconds(
                        _floors[_currentFloor].position, _timeBetweenFloors));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && _currentFloor < _floors.Length - 1)
            {
                _currentFloor++;
                StartCoroutine(
                    MoveOverSeconds(
                        _floors[_currentFloor].position, _timeBetweenFloors));
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void Recall(int floor)
    {
        if (!_hasTarget && _currentFloor != floor)
        {
            _currentFloor = floor;
            StartCoroutine(
                    MoveOverSeconds(
                        _floors[_currentFloor].position, _timeBetweenFloors));
        }
    }

    private IEnumerator MoveOverSeconds (Vector3 end, float seconds)
    {
        _hasTarget = true;
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(
                startingPosition, end, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = end;
        _hasTarget = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerOnFloor = true;
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerOnFloor = false;
            other.transform.SetParent(null);
        }
    }
}
