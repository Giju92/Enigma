using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum AreaID {
        None, Profeta, Muse, Incertezza, Trovatore, Canto, Torre, Infinito, Delizia,
        Cavour, Angoscia, Ora, Arrivo
    };
    public enum GameState { MainMenu, Tutorial, Running, Paused, Book, Ending };

    static int totArea = 12;

    private GameState state;
    private GameObject pauseMenu, book, inventory, HUD, title, tutorial, startButton;
    private AreaID currentArea = 0;
    private bool[] visitedArea = new bool[totArea + 1];
    private bool[] solvedArea = new bool[totArea + 1];
    private bool resumePressed = false, switchToEndMusic = false, switchToTutorial = false, 
        switchToRunning = false, switchToEnding = false, transitionToTutorial = false;
    private float musicDelay = 0f;
    private AudioSource music, effects;

    public GameObject ending;
    public AudioClip menu, inGame, endGame, highNote, buttonPressed, buttonHighlighted;

    public static GameManager Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    void Start() {

        for (int i = 0; i < totArea; i++) {
            visitedArea[i] = false;
            solvedArea[i] = false;
        }

        pauseMenu = GameObject.Find("PauseMenu");

        HUD = GameObject.Find("HUD");
        HUD.SetActive(false);

        book = GameObject.Find("BookUI");
        book.SetActive(false);

        title = GameObject.Find("TitleScreen");

        tutorial = GameObject.Find("TutorialManager");
        tutorial.SetActive(false);

        startButton = GameObject.Find("Lavagna").transform.GetChild(0).gameObject;

        music = GetComponents<AudioSource>()[0];
        effects = GetComponents<AudioSource>()[1];

        music.clip = menu;
        music.Play();
        state = GameState.MainMenu;
        InteractionController.Instance.Disable();
        FirstPersonController.Instance.DisableLook();
        FirstPersonController.Instance.DisableMovement();
        FirstPersonController.Instance.EnableCursor();
        startButton.transform.GetComponent<Lavagna>().SetMenu();
    }

    void Update() {

        switch (state) {
            case GameState.MainMenu:
                if (switchToTutorial)
                    MainMenuToTutorial();
                else if (!transitionToTutorial)
                    CheckBeginButton();
                break;

            case GameState.Tutorial:
                if (switchToRunning)
                    TutorialToRunning();
                break;

            case GameState.Running:
                if (switchToEnding)
                    RunningToEnding();
                else if (Input.GetButtonDown("Book"))
                    RunningToBook();
                else if (Input.GetButtonDown("Pause"))
                    RunningToPaused();
                break;

            case GameState.Paused:
                if (Input.GetButtonDown("Pause") || resumePressed)
                    PausedToRunning();
                else if (Input.GetButtonDown("Book")) {
                    PausedToBook();
                }
                break;

            case GameState.Book:
                if (book.GetComponentInChildren<AutoFlip>().IsFlipping() == false) {
                    if (Input.GetButtonDown("Book"))
                        BookToRunning();
                    else if (Input.GetButtonDown("Pause")) {
                        BookToPaused();
                    }
                }
                break;

            case GameState.Ending:
                break;
        }
    }

    private void MainMenuToTutorial() {    
        switchToTutorial = false;
        transitionToTutorial = false;
        tutorial.SetActive(true);
        InventoryManager.Instance.SwitchToGameScreen();
        TextManager.Instance.SwitchToGameScreen();
        InteractionController.Instance.Enable();
        FirstPersonController.Instance.EnableLook();
        FirstPersonController.Instance.EnableMovement();
        HUD.SetActive(true);
        pauseMenu.SetActive(false);
        Destroy(title);
        state = GameState.Tutorial;
    }

    private void TutorialToRunning() {
        switchToRunning = false;
        state = GameState.Running;
    }

    private void RunningToPaused() {
        PointerManager.Instance.Disable();
        InteractionController.Instance.Disable();
        FirstPersonController.Instance.DisableLook();
        FirstPersonController.Instance.DisableMovement();
        FirstPersonController.Instance.EnableCursor();
        InventoryManager.Instance.SwitchToPauseScreen();
        TextManager.Instance.SwitchToPauseScreen();
        pauseMenu.SetActive(true);

        state = GameState.Paused;
    }

    private void PausedToRunning() {
        PointerManager.Instance.Enable();
        InteractionController.Instance.Enable();
        FirstPersonController.Instance.EnableLook();
        FirstPersonController.Instance.EnableMovement();
        FirstPersonController.Instance.DisableCursor();
        InventoryManager.Instance.SwitchToGameScreen();
        TextManager.Instance.SwitchToGameScreen();
        pauseMenu.SetActive(false);

        state = GameState.Running;
        resumePressed = false;
    }

    private void PausedToBook() {
        FirstPersonController.Instance.DisableCursor();
        InventoryManager.Instance.SwitchToBookScreen();
        TextManager.Instance.SwitchToBookScreen();
        book.SetActive(true);
        pauseMenu.SetActive(false);

        state = GameState.Book;
    }

    private void RunningToBook() {
        PointerManager.Instance.Disable();
        InteractionController.Instance.Disable();
        FirstPersonController.Instance.DisableLook();
        FirstPersonController.Instance.DisableMovement();
        InventoryManager.Instance.SwitchToBookScreen();
        TextManager.Instance.SwitchToBookScreen();

        book.SetActive(true);
        GameObject.Find("Book").GetComponent<Book>().updatePage((int)currentArea);

        state = GameState.Book;
    }

    private void BookToRunning() {
        PointerManager.Instance.Enable();
        InteractionController.Instance.Enable();
        FirstPersonController.Instance.EnableLook();
        FirstPersonController.Instance.EnableMovement();
        InventoryManager.Instance.SwitchToGameScreen();
        TextManager.Instance.SwitchToGameScreen();

        book.SetActive(false);

        state = GameState.Running;
    }

    private void BookToPaused() {
        FirstPersonController.Instance.EnableCursor();
        InventoryManager.Instance.SwitchToPauseScreen();
        TextManager.Instance.SwitchToPauseScreen();
        book.SetActive(false);
        pauseMenu.SetActive(true);

        state = GameState.Paused;
    }

    private void RunningToEnding() {
        switchToEnding = false;
        state = GameState.Ending;
        Instantiate(ending);
    }

    private void CheckBeginButton() {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("MainMenu"))) {

            startButton.transform.GetComponent<Lavagna>().SetSelected();

            if (Input.GetButtonDown("Left Click")) {

                startButton.transform.GetComponent<Lavagna>().SetEmpty();
                transitionToTutorial = true;

                PlayHighNote();
                music.Stop();
                music.clip = inGame;
                music.volume = 0.01f;
                music.Play();

                title.GetComponent<Animator>().SetTrigger("fadeOut");
                FirstPersonController.Instance.StartAnimation();              
            }
        } else {
            startButton.transform.GetComponent<Lavagna>().SetMenu();
        }
    }

    public void SetCurrentArea(AreaID areaID) {
        currentArea = areaID;
    }

    public void SetVisited(AreaID areaID) {
        visitedArea[(int)areaID] = true;
    }

    public void SetSolved(AreaID areaID) {
        solvedArea[(int)areaID] = true;
    }

    public bool IsVisited(int i) {
        return visitedArea[i];
    }

    public bool IsSolved(int i) {
        return solvedArea[i];
    }

    public void ExitPressed() {
        PlayButtonPressed();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ResumePressed() {
        PlayButtonPressed();
        resumePressed = true;
    }

    public void SwitchToEndMusic() {
        effects.clip = highNote;
        effects.volume = 0.1f;
        effects.Play();
        music.Stop();
        music.clip = endGame;
        music.volume = 0.1f;
        music.loop = false;
        music.Play();
    }

    public void PlayButtonPressed() {
        effects.clip = buttonPressed;
        effects.Play();
    }

    public void PlayButtonHighLighted() {
        effects.clip = buttonHighlighted;
        effects.Play();
    }

    public void PlayHighNote() {
        music.Stop();
        music.Play();
        effects.clip = highNote;
        effects.Play();
    }

    public void SwitchToRunning() {
        switchToRunning = true;
    }

    public void SwitchToTutorial() {
        switchToTutorial = true;
    }

    public void SwitchToEnding() {
        switchToEnding = true;
    }
}
