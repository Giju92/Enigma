using UnityEngine;
using System.Collections;

public class SlideDoor : Focusable {

    public void Activate() {
        GetComponent<Animator>().SetTrigger("open");
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        StartFocus();
    }

    void Update() {

        CheckFocusTime();
    }
}
