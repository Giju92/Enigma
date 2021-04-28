using UnityEngine;
using System.Collections;

public class TutorialArea : MonoBehaviour {

    public TutorialManager tutorial;

    void OnTriggerEnter() {
        tutorial.PlayerIn();
        Destroy(transform.gameObject);
    }

}
