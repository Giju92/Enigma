using UnityEngine;
using System.Collections;

public class Angoscia : MonoBehaviour {

    public AreaManager areaManager;

    public int[] currentCombination = new int[4] { 0, 0, 0, 0 };
    public int[] solution = new int[4] { 0, 0, 0, 0 };

    void Start() {
        for (int i = 0; i < 4; i++)      
            transform.GetChild(i).GetComponent<Numero>().Initialize(this, currentCombination[i], i);
    }

    public void UpdateCombination(int digitPosition, int digitValue) {
        GetComponent<AudioSource>().Play();

        currentCombination[digitPosition] = digitValue;

        bool correct = true;
        for (int i = 0; i < 4; i++) {
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

            for (int i = 0; i < 4; i++)
                transform.GetChild(i).gameObject.layer = LayerMask.GetMask("Default");           
        }

    }
}
