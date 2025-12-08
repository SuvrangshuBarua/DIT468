using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
            }
            return _instance;
        }
        private set => _instance = value;
    }

    [SerializeField] private CinemachineCamera _followCamera;
    [SerializeField] private CinemachineConfiner2D _confiner2D;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
