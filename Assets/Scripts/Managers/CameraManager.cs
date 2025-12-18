using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _followCamera;
    [SerializeField] private CinemachineConfiner2D _confiner2D;
    [SerializeField] Camera _camera;

    public Camera Camera { get => _camera; }

    public void SetupConfiner(Collider2D confinerCollider)
    {
        _confiner2D.BoundingShape2D = confinerCollider;
        if (_confiner2D.BakeBoundingShape(_followCamera, 0.01f))
        {
            #if UNITY_EDITOR
            Debug.Log("Baked confiner");
            #endif
        }
    }
}
