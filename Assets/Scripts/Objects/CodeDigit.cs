using UnityEngine;
using System.Collections;
using System;

public class CodeDigit : MonoBehaviour, Interactable {

    private CodeRiddle codeRiddle;
    private Renderer digitRenderer;
    private Texture[] codeDigits;
    private int value;
    private int position;
    
    public void Initialize(CodeRiddle codeRiddle, Texture[] codeDigits, int value, int position) {
        this.codeRiddle = codeRiddle;
        this.codeDigits = codeDigits;
        this.value = value;
        this.position = position;
        digitRenderer = GetComponent<Renderer>();
        digitRenderer.material.mainTexture = codeDigits[value];
    }

    public void Interact() {
        value = (value >= 2) ? 0 : value + 1;
        digitRenderer.material.mainTexture = codeDigits[value];
        codeRiddle.UpdateCombination(position, value);
    }

    public void SetPointer() {
        PointerManager.Instance.SetHand();
    }
}
