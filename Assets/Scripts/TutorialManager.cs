using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    private enum Phases { Start, Grab, Give, BookOpen, BookClose, Pause, End }

    private Phases phase;
    private float delay = 0f;
    private bool playerIn = false, chalkGiven = false, boardWritten= false, bookOpened = false;

    public Sprite movementIcon, interactIcon, bookIcon, browseIcon, pauseIcon;
    public Transform key;

    void Start() {
        phase = Phases.Start;
        key.gameObject.SetActive(false);
    }

    void Update() {
        switch (phase) {
            case Phases.Start:
                phase++;
                TextManager.Instance.ShowTutorialIcon(movementIcon);
                TextManager.Instance.ShowTutorialText("Avvicinati al profeta");
                break;

            case Phases.Grab:
                if (playerIn) {
                    playerIn = false;
                    phase++;
                    TextManager.Instance.ShowTutorialIcon(interactIcon);
                    TextManager.Instance.ShowTutorialText("Raccogli i gessetti sul tavolo");
                }
                break;

            case Phases.Give:
                if (InventoryManager.Instance.Contains(ItemID.Chalk)) {
                    phase++;
                    TextManager.Instance.ShowTutorialText("Consegna i gessetti al profeta");
                }
                break;

            case Phases.BookOpen:
                if (chalkGiven) {
                    chalkGiven = false;
                    TextManager.Instance.EndTutorialText();
                    TextManager.Instance.EndTutorialIcon();
                } else if (boardWritten) {
                    boardWritten = false;
                    phase++;
                    key.gameObject.SetActive(true);
                    TextManager.Instance.ShowTutorialIcon(bookIcon);
                    TextManager.Instance.ShowTutorialText("Apri il libro");
                    GameManager.Instance.SwitchToRunning();
                }
                break;

            case Phases.BookClose:
                if (Input.GetButtonDown("Book")) {
                    phase++;
                    TextManager.Instance.ShowTutorialIcon(browseIcon);
                    TextManager.Instance.EndTutorialText();
                }
                break;

            case Phases.Pause:
                if (Input.GetButtonDown("Book") || Input.GetButtonDown("Pause")) {
                    phase++;
                    TextManager.Instance.ShowTutorialIcon(pauseIcon);
                }
                break;

            case Phases.End:
                delay += Time.deltaTime;
                if (delay >= 5f || Input.GetButtonDown("Pause")) {
                    TextManager.Instance.EndTutorialIcon();
                    Destroy(transform.gameObject);
                }
                break;
        }
    }

    public void PlayerIn() {
        playerIn = true;
    }

    public void ChalkGiven() {
        chalkGiven = true;
    }

    public void BoardWritten() {
        boardWritten = true;
    }
}
