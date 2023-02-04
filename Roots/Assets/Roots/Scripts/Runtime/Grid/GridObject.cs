using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _unoccupied;
    [SerializeField]
    private GameObject _occupied;

    public void SetOccupied(bool occupied)
    {
        _unoccupied.SetActive(!occupied);
        _occupied.SetActive(occupied);
    }
    
}
