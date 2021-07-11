using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform _pointA = null;
    [SerializeField] private Transform _pointB = null;

    private Transform _target;

    private void Start()
    {
        _target = _pointB;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) < 0.01f)
        {
            if (_target == _pointA)
                _target = _pointB;
            else
                _target = _pointA;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
