using System;
using UnityEditor.Animations;

namespace FD.StateMachine {

    [Serializable]
    public class StateLink {
        public LinkedMonoBehaviour mono;
        public AnimatorState animatorState;
    }
}