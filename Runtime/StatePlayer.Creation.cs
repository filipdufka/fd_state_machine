using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace FD.StateMachine {
    public partial class StatePlayer {
        const string goName = "State Machine";

        public void SetupStateMachineAnimator() {
#if UNITY_EDITOR
            var go = GetOrCreateStateMachineGO();
            animator = GetOrCreateAnimator(go);
            //controller = animator?.runtimeAnimatorController;
#endif
        }
        public void CreateMachineController() {
#if UNITY_EDITOR
            Debug.Log("CreateMachineController");
            var con = CreateAnimatorController(gameObject.name);
            if (animator != null) {
                animator.runtimeAnimatorController = con;
                controller = con;
            }
#endif
        }

        public GameObject GetOrCreateStateMachineGO() {
            GameObject go = null;
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).name == goName) {
                    go = transform.GetChild(i).gameObject;
                }
            }
#if UNITY_EDITOR
            if (go == null) {
                Undo.RecordObject(gameObject, "Create State Machine GO");
                go = new GameObject(goName);
                Undo.RegisterCreatedObjectUndo(go, "Create State Machine GO");
                go.transform.parent = transform;
                EditorUtility.SetDirty(gameObject);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
#else
    if (go == null)
    {
        go = new GameObject(goName);
        go.transform.parent = transform;
    }
#endif

            return go;
        }

        public static Animator GetOrCreateAnimator(GameObject go) {
#if UNITY_EDITOR
            var a = go.GetComponent<Animator>();
            if (a == null) {
                a = Undo.AddComponent<Animator>(go);
            }
            return a;
#endif
        }

        public static RuntimeAnimatorController CreateAnimatorController(string objectName) {
#if UNITY_EDITOR
            var path = EditorUtility.SaveFilePanelInProject(
                    "Choose save path for state machine controller",
                    objectName + "StateMachine",
                    "controller",
                    "Choose save path for state machine controller");

            var conn = AnimatorController.CreateAnimatorControllerAtPath(path);
            return conn;
#endif
        }

        public static AnimatorState CreateAnimationState(RuntimeAnimatorController controller, string stateName) {
            Undo.RecordObject(controller, "Create Animator State");
            var layer = ((AnimatorController)controller).layers[0];
            var stateMachine = layer.stateMachine;

            var state = stateMachine.AddState(stateName);
            EditorUtility.SetDirty(controller);
            return state;
        }

        public static string GetStateName(LinkedMonoBehaviour behaviour) {
            if (behaviour != null) {
                return behaviour.GetType().Name.Replace("Behaviour", "State");
            }
            return "";
        }

        public static LinkedStateMachineBehaviour GetLinkedStateBehaviour(AnimatorState state) {
            var lsmb = state.behaviours.ToList().Find(s => s.GetType() == typeof(LinkedStateMachineBehaviour));
            return (LinkedStateMachineBehaviour)lsmb;
        }


#if UNITY_EDITOR
        public static LinkedStateMachineBehaviour AddOrGetLinkedStateBehaviour(RuntimeAnimatorController controller, AnimatorState state) {
            var lsmb = GetLinkedStateBehaviour(state);
            if (lsmb == null) {
                Undo.RecordObject(controller, "Add StateMachineBehaviour");
                lsmb ??= state.AddStateMachineBehaviour<LinkedStateMachineBehaviour>();
                EditorUtility.SetDirty(controller);

            }
            return lsmb;
        }
    }
#endif
}
