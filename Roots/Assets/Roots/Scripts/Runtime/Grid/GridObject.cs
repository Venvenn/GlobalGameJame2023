using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;

public class GridObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _unoccupied;
    [SerializeField]
    private GameObject _occupied;
    [SerializeField]
    private MMF_Player _player;
    [SerializeField]
    private GameObject[] _seeds;
    [SerializeField]
    private GameObject _seedParent;


    private void Start()
    {
        _player.Initialization();
    }

    public void SetOccupied(bool occupied, int vegType)
    {
        for (int i = 0; i < _seeds.Length; i++)
        {
            _seeds[i].SetActive(false);
            if (i == vegType)
            {
                _seeds[i].SetActive(true);
            }
        }

        if (occupied)
        {
            StartCoroutine(modelSwap());
        }
        else
        {
            _unoccupied.SetActive(!occupied);
            _occupied.SetActive(occupied);
            _player.PlayFeedbacks();
        }
    }

    IEnumerator modelSwap()
    {
        _unoccupied.SetActive(true);
        _seedParent.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        _occupied.SetActive(true);
        _unoccupied.SetActive(false);
        _seedParent.SetActive(false);
        _player.PlayFeedbacks();
    }
}
