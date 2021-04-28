using UnityEngine;
using System.Collections;

public class Vagone : Focusable {

    public void Open() {
        StartFocus();
        GetComponent<AudioSource>().Play();
    }

    void Update() {
        CheckFocusTime();
    }
}
