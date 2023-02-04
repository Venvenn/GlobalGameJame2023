using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour  
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraSettings _cameraSettings;

    private Coroutine _moveToRoutine;
    private Coroutine _zoomRoutine;

    bool isZoomOut = true;

    private void Start()
    {
        Zoom(_cameraSettings.MinZoom);
    }

    private void Update()
    {
        //Move the camera to where you click
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 400))
            {
                MoveTo(hit.point);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            float zoom;
            if (isZoomOut)
            {
                zoom = _cameraSettings.MinZoom;
            }
            else
            {
                zoom = _cameraSettings.MaxZoom;
            }

            isZoomOut = !isZoomOut;

            Zoom(zoom);
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
