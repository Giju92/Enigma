using UnityEngine;
using System.Collections;

public class CodeRiddle : MonoBehaviour {

    public AreaManager areaManager;
    public Texture[] codeDigits;
    public int[] currentCombination = { 0, 0, 0, 0 };
    public int[] solution = { 0, 0, 0, 0 };
   
    void Start() {
        int i = 0;
        foreach (Transform child in transform) {
            child.GetComponent<CodeDigit>().Initialize(this, codeDigits, currentCombination[i], i);
            i++;
        }
    }

    public void UpdateCombination(int digitPosition, int digitValue) {
        GetComponent<AudioSource>().Play();

        currentCombination[digitPosition] = digitValue;

        bool correct = true;
        for (int i = 0; i < currentCombination.Length; i++) {
            if (currentCombination[i] != solution[i]) {
                correct = false;
                break;
            }
        }
        if (correct) {

            transform.parent.GetComponent<Animator>().SetTrigger("open");
            transform.parent.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetComponentInChildren<Vagone>().Open();
            areaManager.SolvedOne();

            foreach (Transform child in transform) {
                child.gameObject.layer = LayerMask.GetMask("Default");
            }
        }

    }
}
