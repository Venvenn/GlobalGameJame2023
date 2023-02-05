using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class VegetableObject : MonoBehaviour
{
    [SerializeField]
    private MMF_Player _feedbackPlayer;

    [SerializeField]
    private GameObject _sparkleEffect;

    public void Pull()
    {
        _feedbackPlayer.Initialization();
        _feedbackPlayer.PlayFeedbacks();
    }

    public void FinishGrow()
    {
        if (_sparkleEffect != null)
        {
            _sparkleEffect.SetActive(true);
        }
    }
}
