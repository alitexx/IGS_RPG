using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode, RequireComponent (typeof(Camera))]

public class CamMovementDetect : MonoBehaviour
{
    private Camera m_Camera;
    private Quaternion m_LastRotation;
    private Vector3 m_LastPosition;

    private bool m_IsIdle = true;
    public bool isIdle { get => m_IsIdle; }

    private void OnEnable()
    {
        m_Camera = GetComponent<Camera>();

        TrackCameraValues();
    }



    private void Update()
    {
        m_IsIdle = IsIdle();
        TrackCameraValues();
    }

    public bool IsIdle()
    {
        return Vector3.Equals(m_Camera.transform.position, m_LastPosition) && Quaternion.Equals(m_Camera.transform.rotation, m_LastRotation);
    }

    private void TrackCameraValues()
    {
        m_LastRotation = transform.rotation;
        m_LastPosition = transform.position;
    }
}
