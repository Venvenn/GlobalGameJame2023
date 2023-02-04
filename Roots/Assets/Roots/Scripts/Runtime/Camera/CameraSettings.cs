using UnityEngine;

/// <summary>
/// Scriptable object to store the global camera settings
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "CameraSettings", menuName = "Data/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [SerializeField]
    private float _minZoom = 50f;
    public float MinZoom => _minZoom;

    [SerializeField]
    private float _maxZoom = 200f;
    public float MaxZoom => _maxZoom;

    [SerializeField]
    private float _moveSpeed = 10f;
    public float MoveSpeed => _moveSpeed;
}
