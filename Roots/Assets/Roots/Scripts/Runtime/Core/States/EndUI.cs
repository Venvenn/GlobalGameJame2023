using Siren;
using UnityEngine;

public class EndUI : FlowScreenUI
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _looseScreen;

    public override void InitUI()
    {

    }

    public void SetUp(bool win)
    {
        _winScreen.SetActive(win);
        _looseScreen.SetActive(!win);
    }

    public override void UpdateUI()
    {

    }

    public override void DestroyUI()
    {

    }
}
