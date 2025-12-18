using UnityEngine;

namespace FD.StateMachine {
    public class LinkedStateMachineBehaviour : StateMachineBehaviour {
        [HideInInspector]
        public LinkedMonoBehaviour linkedMonoBehaviour;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            linkedMonoBehaviour?.OnStateEnter();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit(animator, stateInfo, layerIndex);
            linkedMonoBehaviour?.OnStateExit();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }
    }
}