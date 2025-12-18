#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace FD.StateMachine.Editor {
    [CustomEditor(typeof(StatePlayer))]
    public class StatePlayerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var sp = (StatePlayer)target;

            base.OnInspectorGUI();
            EditorGUILayout.Space();

            ShowAnimator(sp);
            ShowController(sp);

            EditorGUILayout.Space();
            ListAllBehaviours(sp);

        }

        static void ShowAnimator(StatePlayer sp) {
            EditorGUIUtility.labelWidth = 70f;

            EditorGUILayout.BeginHorizontal();
            var newAnimator = (Animator)EditorGUILayout.ObjectField(
                "Animator",
                sp.animator,
                typeof(Animator),
                true
            );

            if (newAnimator != sp.animator) {
                Undo.RecordObject(sp, "Change of Animator");
                sp.animator = newAnimator;
                EditorUtility.SetDirty(sp);
            }

            if (sp.animator == null) {
                if (GUILayout.Button("Create")) {
                    sp.SetupStateMachineAnimator();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        static void ShowController(StatePlayer sp) {
            if (sp.animator == null) { return; }

            EditorGUIUtility.labelWidth = 70f;
            EditorGUILayout.BeginHorizontal();
            var newController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(
                "Controller",
                 sp.controller,
                typeof(RuntimeAnimatorController),
                true
            );

            if (newController != sp.controller) {
                Undo.RecordObject(sp, "Change of Controller");
                sp.controller = newController;
                EditorUtility.SetDirty(sp);
                Undo.RecordObject(sp.animator, "Setting Controller to Animator");
                sp.animator.runtimeAnimatorController = newController;
                EditorUtility.SetDirty(sp.animator);
            }

            if (sp.controller == null) {
                if (GUILayout.Button("Create")) {
                    sp.CreateMachineController();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        static void ListAllBehaviours(StatePlayer sp) {
            if (sp.animator != null && sp.controller != null) {
                var sizes = new int[] { 120, 70 };
				var behaviours = sp.GetComponents<LinkedMonoBehaviour>();

				if(behaviours.Length > 0){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Behaviours:", GUILayout.Width(sizes[0]));
					EditorGUILayout.LabelField("States:");
					EditorGUILayout.LabelField("", GUILayout.Width(sizes[1]));

					EditorGUILayout.EndHorizontal();


					var states = StatePlayer.GetAllStates(sp.controller);
					var stateNames = states.ConvertAll(x => x.name).ToList();
					stateNames.Insert(0, "No State");
					var stateNamesArr = stateNames.ToArray();

					foreach (var c in behaviours) {
						ShowBehaviour(sp, c, sizes, states, stateNamesArr);
					}
				}else{
					EditorGUILayout.HelpBox(
						"To use State Player, Add Components to this object which are inheriting LinkedMonoBehaviour.",
						MessageType.Warning
					);
				}
            }
        }

        static void ShowBehaviour(StatePlayer sp, LinkedMonoBehaviour c, int[] sizes, List<AnimatorState> states, string[] stateNamesArr) {
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.white;

            EditorGUILayout.LabelField(c.GetType().Name, GUILayout.Width(sizes[0]));

            var link = sp.GetStateLink(c);
            if (link != null) {
                GUI.color = link.animatorState != null ? Color.white : Color.red;
                var index = states.IndexOf(link.animatorState) + 1;
                var nextIndex = EditorGUILayout.Popup(index, stateNamesArr);

                if (nextIndex != index) {
                    Undo.RecordObject(sp, "Link State");
                    if (nextIndex == 0) {
                        link.animatorState = null;
                    } else {
                        link.animatorState = states[nextIndex - 1];
                    }
                    EditorUtility.SetDirty(sp);
                }

                GUI.color = link.animatorState != null ? Color.green : Color.white;
                if (link.animatorState == null) {
                    if (GUILayout.Button("New State", GUILayout.Width(sizes[1]))) {
                        var stateName = StatePlayer.GetStateName(c);
                        var newState = StatePlayer.CreateAnimationState(sp.controller, stateName);
                        StatePlayer.AddOrGetLinkedStateBehaviour(sp.controller, newState);
                        link.animatorState = newState;
                    }
                } else {
                    EditorGUILayout.LabelField("linked", GUILayout.Width(sizes[1] - 20));
                    if (GUILayout.Button("👁", GUILayout.Width(20))) {
                        Selection.activeObject = link.animatorState;
                        EditorGUIUtility.PingObject(Selection.activeObject);
                    }
                }
            } else {
                Undo.RecordObject(sp, "Create Link");
                sp.CreateLink(c);
                EditorUtility.SetDirty(sp);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
