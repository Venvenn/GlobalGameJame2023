using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class VegetableObject : MonoBehaviour
{
    [SerializeField]
    private MMF_Player _feedbackPlayer;
    [SerializeField]
    private MMF_Player _killFeedbackPlayer;
    [SerializeField]
    private MMF_Player _damageFeedbackPlayer;

    [SerializeField]
    private GameObject _sparkleEffect;

    public void Pull(int cropNumber, int seedNumber, string cropName, bool disableMessage)
    {
        List<MMF_FloatingText> feedback = _feedbackPlayer.GetFeedbacksOfType<MMF_FloatingText>();
        if (feedback.Count > 0)
        {
            if (cropNumber > 0 && !disableMessage)
            {
                feedback[0].Value = $"+{cropNumber} {cropName}";
            }
            else
            {
                feedback[0].Active = false;
            }
            if (seedNumber > 0 && !disableMessage)
            {
                string seedsName = seedNumber == 1 ? "Seed" : "Seeds";
                feedback[1].Value = $"+{seedNumber} {seedsName}";
            }
            else
            {
                feedback[1].Active = false;
            }
        }

        _feedbackPlayer.Initialization();
        _feedbackPlayer.PlayFeedbacks();
    }
    
    public void Kill()
    {
        _killFeedbackPlayer.Initialization();
        _killFeedbackPlayer.PlayFeedbacks();
    }

    public void Damage()
    {
        _damageFeedbackPlayer.Initialization();
        _damageFeedbackPlayer.PlayFeedbacks();
    }

    public void FinishGrow()
    {
        if (_sparkleEffect != null)
        {
            _sparkleEffect.SetActive(true);
        }
    }
}
