using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace FD.StateMachine {
    public partial class StatePlayer : MonoBehaviour {
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public RuntimeAnimatorController controller;

        [SerializeField, HideInInspector]
        public List<StateLink> links;

        private void OnEnable() {
            Debug.Log("State Player Enable");
            foreach (var link in links) {
                if (link.animatorState != null && link.mono != null) {
                    var stateBehaviour = AddOrGetLinkedStateBehaviour(controller, link.animatorState);
                    stateBehaviour.linkedMonoBehaviour = link.mono;
                }
            }
        }

        public StateLink GetStateLink(LinkedMonoBehaviour monoBehaviour) {
            if (links == null) { return null; }
            return links.Find(l => l.mono.Equals(monoBehaviour));
        }

        public void CreateLink(LinkedMonoBehaviour monoBehaviour, AnimatorState state = null) {
            if (links == null) {
                links = new List<StateLink>();
            }

            links.Add(new StateLink() {
                mono = monoBehaviour,
                animatorState = null,
            });
        }

        public static List<AnimatorState> GetAllStates(RuntimeAnimatorController controller) {
            if (controller == null) { return new List<AnimatorState>(); }
            var states = ((AnimatorController)controller).layers[0].stateMachine.states;
            return states.Select(s => s.state).ToList();
        }
    }
}