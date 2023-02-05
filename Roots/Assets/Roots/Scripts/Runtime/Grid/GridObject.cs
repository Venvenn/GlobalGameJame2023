using UnityEngine;
using MoreMountains.Feedbacks;

public class GridObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _unoccupied;
    [SerializeField]
    private GameObject _occupied;
    [SerializeField]
    private MMF_Player _player;

    public void SetOccupied(bool occupied)
    {
        _unoccupied.SetActive(!occupied);
        _occupied.SetActive(occupied);
        _player.PlayFeedbacks();
    }
    
}
