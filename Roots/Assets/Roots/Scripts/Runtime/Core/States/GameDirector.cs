using System.Collections;
using System.Collections.Generic;
using Siren;
using UnityEngine;

public class GameDirector : Director
{
    public override void OnStart()
    {
        m_flowStateMachine = new FlowStateMachine();
        m_flowStateMachine.Push(new FSSystem());
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }
}