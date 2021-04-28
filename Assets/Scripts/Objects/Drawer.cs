using UnityEngine;
using System.Collections;

public class Drawer : Focusable {

	// Update is called once per frame
	void Update () {
        CheckFocusTime();
	}

    public void Open() {
        GetComponent<Animator>().SetTrigger("open");
        GetComponent<AudioSource>().Play();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        StartFocus();
    }
}
