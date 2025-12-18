using FD.StateMachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PersistanceTest))]
public class PersistanceTestEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var pt = (PersistanceTest)target;

        if (GUILayout.Button("Load")) {
            var controller = pt.animator.runtimeAnimatorController;
            pt.states = StatePlayer.GetAllStates(controller);

            pt.links = new List<StateLink>();
            foreach (var state in pt.states) {
                pt.links.Add(new StateLink() { animatorState = state });
            }
        }
    }

}
