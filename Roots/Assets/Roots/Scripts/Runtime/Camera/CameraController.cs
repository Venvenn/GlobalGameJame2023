using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour  
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraSettings _cameraSettings;

    public CameraSettings CameraSettings => _cameraSettings;

    private Coroutine _moveToRoutine;
    private Coroutine _zoomRoutine;

    bool isZoomOut = false;

    public bool IsZoomOut => isZoomOut;

    public void SnapCamera(Vector3 targetWorldPos, float zoomDistance)
    {
        transform.position = targetWorldPos;
        _camera.transform.localPosition = -_camera.transform.forward * zoomDistance;
        if (zoomDistance == _cameraSettings.MaxZoom)
        {
            isZoomOut = true;
        }
        else
        {
            isZoomOut = false;
        }
    }

    /// <summary>
    /// Move the camera to point towards world point
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(Vector3 pos)
    {
        if (_moveToRoutine != null)
        {
            StopCoroutine(_moveToRoutine);
        }

        _moveToRoutine = StartCoroutine(MoveToCoroutine(pos));
    }

    IEnumerator MoveToCoroutine(Vector3 pos)
    {
        float lerpTime = 0;
        Vector3 initialPosition = transform.position;
        while (pos != transform.position)
        {
            transform.position = Vector3.Lerp(initialPosition, pos, lerpTime);
            lerpTime += _cameraSettings.MoveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void Zoom(float zoomDistance)
    {
        if (_zoomRoutine != null)
        {
            StopCoroutine(_zoomRoutine);
        }

        if (zoomDistance == _cameraSettings.MaxZoom)
        {
            isZoomOut = true;
        }
        else
        {
            isZoomOut = false;
        }
        _zoomRoutine = StartCoroutine(ZoomOutCoroutine(zoomDistance));
    }

    IEnumerator ZoomOutCoroutine(float zoomDistance)
    {
        float lerpTime = 0;
        Vector3 targetCameraPos = -_camera.transform.forward * zoomDistance;
        Vector3 initialCameraPos = _camera.transform.localPosition;

        while (targetCameraPos != initialCameraPos)
        {
            _camera.transform.localPosition = Vector3.Lerp(initialCameraPos, targetCameraPos, lerpTime);
            lerpTime += _cameraSettings.MoveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
