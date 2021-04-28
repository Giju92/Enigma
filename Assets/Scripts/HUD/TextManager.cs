using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextManager : MonoBehaviour {

    private static Color invisible = new Color(255, 255, 255, 0);
    private static Color visible = new Color(255, 255, 255, 255);

    private Text mainText, subText, tutorialText;
    private Image tutorialIcon;
    private Animator mainTextAnimator, subTextAnimator, tutorialTextAnimator, tutorialIconAnimator;
    private Queue<string> mainTextQueue, subTextQueue, tutorialTextQueue;
    private Queue<Sprite> tutorialIconQueue;

    public static TextManager Instance { get; private set; }

    void Awake() {
        Instance = this;

        GameObject obj = transform.Find("MainText").gameObject;
        mainText = obj.GetComponent<Text>();
        mainTextAnimator = obj.GetComponent<Animator>();

        obj = transform.Find("SubText").gameObject;
        subText = obj.GetComponent<Text>();
        subTextAnimator = obj.GetComponent<Animator>();

        obj = transform.Find("TutorialText").gameObject;
        tutorialText = obj.GetComponent<Text>();
        tutorialTextAnimator = obj.GetComponent<Animator>();

        obj = transform.Find("TutorialIcon").gameObject;
        tutorialIcon = obj.GetComponent<Image>();
        tutorialIconAnimator = obj.GetComponent<Animator>();

        mainTextQueue = new Queue<string>();
        subTextQueue = new Queue<string>();
        tutorialTextQueue = new Queue<string>();
        tutorialIconQueue = new Queue<Sprite>();

        mainText.text = "";
        subText.text = "";
        tutorialText.text = "";
    }

    void Update() {

        // Check the queue for the main text
        if (mainTextQueue.Count != 0) {
            AnimatorStateInfo state = mainTextAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Empty")) {
                mainText.text = mainTextQueue.Dequeue();
                mainTextAnimator.SetTrigger("fadeIn");

            } else if (state.IsName("Yield")) {
                mainTextAnimator.SetTrigger("fadeOut");
            }
        }

        // Check the queue for the sub text
        if (subTextQueue.Count != 0) {
            AnimatorStateInfo state = subTextAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Empty")) {
                subText.text = subTextQueue.Dequeue();
                subTextAnimator.SetTrigger("fadeIn");

            } else if (state.IsName("Yield")) {
                subTextAnimator.SetTrigger("fadeOut");
            }
        }

        // Check the queue for the tutorial text
        if (tutorialTextQueue.Count != 0) {
            AnimatorStateInfo state = tutorialTextAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Empty")) {
                tutorialText.text = tutorialTextQueue.Dequeue();
                tutorialTextAnimator.SetTrigger("fadeIn");

            } else if (state.IsName("Yield")) {
                tutorialTextAnimator.SetTrigger("fadeOut");
            }
        }

        if (tutorialIconQueue.Count != 0) {
            AnimatorStateInfo state = tutorialIconAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Empty")) {
                tutorialIcon.sprite = tutorialIconQueue.Dequeue();
                tutorialIconAnimator.SetTrigger("fadeIn");

            } else if (state.IsName("Yield")) {
                tutorialIconAnimator.SetTrigger("fadeOut");
            }
        }
    }

    public void ShowMainText(string text) {
        if (mainText.text != text) {
            mainTextQueue.Enqueue(text);
        }
    }

    public void ShowSubText(string text) {
        subTextQueue.Enqueue(text);
    }

    public void ShowTutorialText(string text) {
        tutorialTextQueue.Enqueue(text);
    }

    public void ShowTutorialIcon(Sprite sprite) {
        tutorialIconQueue.Enqueue(sprite);
    }

    public void EndTutorialText() {
        tutorialTextQueue.Clear();
        tutorialTextAnimator.SetTrigger("fadeOut");
    }

    public void EndTutorialIcon() {
        tutorialIconQueue.Clear();
        tutorialIconAnimator.SetTrigger("fadeOut");
    }

    public void SwitchToPauseScreen() {
        mainTextAnimator.enabled = false;
        mainText.color = visible;
        subTextAnimator.enabled = false;
        subText.color = invisible;
    }

    public void SwitchToBookScreen() {
        mainTextAnimator.enabled = false;
        mainText.color = invisible;
        subTextAnimator.enabled = false;
        subText.color = invisible;
    }

    public void SwitchToGameScreen() {
        mainTextAnimator.enabled = true;
        mainText.color = visible;
        subTextAnimator.enabled = true;
        subText.color = visible;
    }
}
