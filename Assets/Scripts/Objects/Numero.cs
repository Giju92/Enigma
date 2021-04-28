using UnityEngine;
using System.Collections;

public class Numero : MonoBehaviour, Interactable {

    private Angoscia angoscia;
    private int value;
    private int position;

    public void Initialize(Angoscia angoscia, int value, int position) {
        this.angoscia = angoscia;
        this.value = value;
        this.position = position;

        transform.Rotate(-120f * value, 0f, 0f);
    }

    public void Interact() {
        if (value>=2) {
            value = 0;
            transform.Rotate(240f, 0f, 0f);
        } else {
            value++;
            transform.Rotate(-120f, 0f, 0f);
        }       
        
        angoscia.UpdateCombination(position, value);
    }

    public void SetPointer() {
        PointerManager.Instance.SetHand();
    }
}
