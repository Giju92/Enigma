using UnityEngine;
using System.Collections;

public abstract class Focusable : MonoBehaviour {

    public float focusTime = 3f;
    private float timePassed = 0f;
    protected bool focused = false;

    protected void StartFocus() {
        focused = true;
        FirstPersonController.Instance.SetFocus(transform);
    }

    protected void CheckFocusTime() {
        if (focused && FirstPersonController.Instance.IsLookEnabled()) {
            timePassed += Time.deltaTime;
            PointerManager.Instance.Disable();
            if (timePassed >= focusTime) {
                focused = false;
                FirstPersonController.Instance.ResetFocus();
                PointerManager.Instance.Enable();
                PointerManager.Instance.SetPoint();
            }
        }
    }
}
