using UnityEngine;
using System.Collections;

public class Lavagna : Focusable {

    public Texture[] array;
    public TutorialManager tutorial;

    bool alreadySelected = true;

    void Update() {
        CheckFocusTime();
    }

    public void Begin() {  
        StartFocus();     
        GetComponent<AudioSource>().Play();
        tutorial.ChalkGiven();
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation() {
        for (int i = 0; i < 10; i++) {
            transform.GetComponent<Renderer>().material.mainTexture = array[i];
            yield return new WaitForSeconds((float)0.5);
        }
        tutorial.BoardWritten();
    }

    public void SetEmpty() {
        transform.GetComponent<Renderer>().material.mainTexture = array[10];
    }

    public void SetMenu() {
        if (alreadySelected) {
            transform.GetComponent<Renderer>().material.mainTexture = array[11];
            alreadySelected = false;
        }
    }

    public void SetSelected() {
        if (!alreadySelected) {
            GameManager.Instance.PlayButtonHighLighted();
            transform.GetComponent<Renderer>().material.mainTexture = array[12];
            alreadySelected = true;
        }
    }

}
