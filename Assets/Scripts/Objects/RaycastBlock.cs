using UnityEngine;
using System.Collections;
using System;

public class RaycastBlock : MonoBehaviour, Interactable {
    public void Interact() {
    }

    public void SetPointer() {
        PointerManager.Instance.SetPoint();
    }
}
