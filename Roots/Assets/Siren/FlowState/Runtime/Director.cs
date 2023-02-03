using UnityEngine;

namespace Siren
{
    /// <summary>
    /// A director is a base type used set up and bootstrap a Flow state machine, You should make a custom one for each
    /// game.
    /// </summary>
    public abstract class Director : MonoBehaviour
    {
        public FlowStateMachine FlowStateMachine = new FlowStateMachine();

        private void Start()
        {
            OnStart();
        }
        
        private void Update()
        {
            OnUpdate();
            FlowStateMachine.Update();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
            FlowStateMachine.FixedUpdate();
        }

        public abstract void OnStart();

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();
    }
}