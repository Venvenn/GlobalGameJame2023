using Siren;
using UnityEngine;

public class FSShop : FlowState
{
    private ShopUI _ui;
    private UIManager _uiManager;
    
    public FSShop(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        Time.timeScale = 0;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<ShopUI>("UI/Screens/ShopUI", this);
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {

    }

    public override void ActiveFixedUpdate()
    {
    }

    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
        }
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        Time.timeScale = 1;
    }

    public override void FinishDismiss()
    {
        Object.Destroy(_ui.gameObject);
    }
}
