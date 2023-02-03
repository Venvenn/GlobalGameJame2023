using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour  
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed = 500;

    private Vector3 _goToPos;

    private void Start()
    {

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
    }

    /// <summary>
    /// Move the camera to point towards world point
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(Vector3 pos)
    {
        _goToPos = pos;
        StopAllCoroutines();
        StartCoroutine(MoveToCoroutine());
    }

    IEnumerator MoveToCoroutine()
    {
        float lerpTime = 0;
        Vector3 initialPosition = transform.position;
        while (_goToPos != transform.position)
        {
            transform.position = Vector3.Lerp(initialPosition, _goToPos, lerpTime);
            lerpTime += _speed * Time.deltaTime;
            yield return null;
        }
    }
}
