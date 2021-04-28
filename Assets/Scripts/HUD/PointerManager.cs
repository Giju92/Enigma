using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PointerManager : MonoBehaviour {

    public Sprite point, hand, nope;
    Image pointer;

    public static PointerManager Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    void Start() {
        pointer = GetComponent<Image>();
    }

    public void SetPoint() {
        pointer.sprite = point;
    }

    public void SetHand() {
        pointer.sprite = hand;
    }

    public void SetNope() {
        pointer.sprite = nope;
    }

    public void SetIcon(Sprite icon) {
        pointer.sprite = icon;
    }

    public void Disable() {
        pointer.enabled = false;
    }

    public void Enable() {
        pointer.enabled = true;
    }
}
