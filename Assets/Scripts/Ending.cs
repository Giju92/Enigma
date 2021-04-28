using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    private bool end;
    
    void Update() {
        if (end && Input.anyKeyDown) {
            GameManager.Instance.ExitPressed();
        }
    }

    private void End() {
        end = true;
    }
}
