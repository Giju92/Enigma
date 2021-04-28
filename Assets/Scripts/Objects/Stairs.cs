using UnityEngine;
using System.Collections;

public class Stairs : Focusable {

	void Update () {
        CheckFocusTime();
	}

    public void Begin() {
        StartFocus();
        GetComponent<Animator>().SetTrigger("start");
    }
}
