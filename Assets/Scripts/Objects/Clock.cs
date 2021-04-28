using UnityEngine;
using System.Collections;

public class Clock : Focusable {

    void Update() {
        CheckFocusTime();
    }

    public void Begin() {
        this.GetComponent<Animator>().SetTrigger("start");
        StartFocus();
    }
}
