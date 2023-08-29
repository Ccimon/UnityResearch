using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepthTexture : MonoBehaviour
{
    [SerializeField] private DepthTextureMode _depthTextureMode;
    private Camera _camera;

    private void OnValidate()
    {
        SetCameraDepthMode();
    }

    private void Awake()
    {
        SetCameraDepthMode();
    }

    private void SetCameraDepthMode()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }

        _camera.depthTextureMode = _depthTextureMode;
    }
}
