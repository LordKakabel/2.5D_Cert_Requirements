using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scale = 0.1f;
    [SerializeField] private bool _upIsCloser = true;

    private CinemachineVirtualCamera _vcam;
    private CinemachineTransposer _transposer;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        if (!_vcam)
            Debug.LogError(name + ": CinemachineVirtualCamera component not found!");

        _transposer = _vcam.GetCinemachineComponent<CinemachineTransposer>();
        if (!_transposer)
            Debug.LogError(name + ": CinemachineComposer component not found!");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = _transposer.m_FollowOffset;

        if (_upIsCloser)
            offset.x -= Input.mouseScrollDelta.y * _scale;
        else
            offset.x += Input.mouseScrollDelta.y * _scale;

        _transposer.m_FollowOffset = offset;
    }
}
