using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class VegetableObject : MonoBehaviour
{
    [SerializeField]
    private MMF_Player _feedbackPlayer;

    public void Pull()
    {
        _feedbackPlayer.Initialization();
        _feedbackPlayer.PlayFeedbacks();
    }
}
