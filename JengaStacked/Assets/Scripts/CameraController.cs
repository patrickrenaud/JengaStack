using System;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public bool allowRotation = true;
    [SerializeField] public CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] float _yAxisSpeed = -8;
    [SerializeField] float _xAxisSpeed = -500;

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            allowRotation = true;
        }
        else
        {
            allowRotation = false;
        }
        HandleCamera();
    }

    void HandleCamera()
    {
        if (allowRotation)
        {
            _cinemachineFreeLook.m_XAxis.m_MaxSpeed = _xAxisSpeed;
            _cinemachineFreeLook.m_YAxis.m_MaxSpeed = _yAxisSpeed;

        }
        else
        {
            _cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            _cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
        }
    }

    public void MoveToStack(Transform stackPos)
    {
        _cinemachineFreeLook.LookAt = stackPos;
        _cinemachineFreeLook.Follow = stackPos;
    }
}
