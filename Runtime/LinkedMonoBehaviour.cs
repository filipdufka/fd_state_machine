using UnityEngine;

public class LinkedMonoBehaviour : MonoBehaviour {
    // Show in editor, if this is active
    public bool stateActive { get; protected set; }

    public virtual void OnStateEnter() {
        stateActive = true;
        SetDirty();
    }

    public virtual void OnStateExit() {
        stateActive = false;
        SetDirty();
    }

    public void SetDirty() {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}
