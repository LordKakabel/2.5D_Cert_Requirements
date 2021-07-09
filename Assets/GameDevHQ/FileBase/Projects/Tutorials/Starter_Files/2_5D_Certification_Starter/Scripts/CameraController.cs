using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scale = 0.1f;
    [SerializeField] private bool _upIsCloser = true;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Camera.main.transform.position;
        if (_upIsCloser)
            position.x -= Input.mouseScrollDelta.y * _scale;
        else
            position.x += Input.mouseScrollDelta.y * _scale;
        Camera.main.transform.position = position;
    }
}
