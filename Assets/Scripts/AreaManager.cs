using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour {

    public int totalRiddles;
    public string areaName;
    public GameManager.AreaID areaID;

    private int solvedRiddles;

    public void SolvedOne() {

        solvedRiddles++;

        if (solvedRiddles >= totalRiddles) {
            GameManager.Instance.SetSolved(areaID);
            TextManager.Instance.ShowSubText("Descrizione sbloccata per " + areaName);
        }
    }

    private void OnTriggerEnter(Collider other) {

        TextManager.Instance.ShowMainText(areaName);
        GameManager.Instance.SetCurrentArea(areaID);

        if (!GameManager.Instance.IsVisited((int)areaID)) {
            GameManager.Instance.SetVisited(areaID);
            TextManager.Instance.ShowSubText("Nuova pagina sbloccata");
        }
    }
}
