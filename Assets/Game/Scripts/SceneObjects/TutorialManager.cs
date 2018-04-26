using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TutorialManager : MonoBehaviour
{
    public int tutorialPhase;
    private string tutorialText;

    public Image textBG;
    public Text textContents;
    public Text clickHere;
    public Image zeusIcon;
    public GameObject skipButton;

    public static TutorialManager instance = null;


    private Animator tutorialAnimator;
    public Text tutorialTextObject;

    private void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerData.evoverve"))
        {
            Init();
        }
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (tutorialPhase != -1)
        {
            tutorialAnimator = gameObject.GetComponent<Animator>();
            StartTutorial(tutorialPhase);
        }
        else
        {
            HideTutorial();
        }
	}

    private void OnEnable()
    {
        GameManager.Loaded += LoadTutorial;
    }

    private void OnDisable()
    {
        GameManager.Loaded -= LoadTutorial;
    }

    private void HideTutorial()
    {
        textBG.enabled = false;
        textContents.enabled = false;
        zeusIcon.enabled = false;
        skipButton.GetComponent<Button>().enabled = false;
        skipButton.GetComponent<Image>().enabled = false;
        clickHere.enabled = false;

    }

    public bool TutorialComplete()
    {
        if (tutorialPhase == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateText()
    {
        switch (tutorialPhase)
        {
            case -1:
                tutorialText = "";
                break;

            case 0:
                tutorialText = "Welcome to Evo-Verve! Your own personal planet!";
                break;

            case 1:
                tutorialText = "My name is Zeus, and I am here to guide you in helping your planet grow and evolve.";
                break;

            case 2:
                tutorialText = "To get started simply tap your planet to earn a credit!";
                break;

            case 3:
                tutorialText = "These are your credits, go ahead and earn 70!";
                break;

            case 4:
                tutorialText = "You now have enough to buy your first item! Tap on the shop item shown.";
                break;

            case 5:
                tutorialText = "Here are all the items you can currently see, tap on the available item to select it!";
                break;

            case 6:
                tutorialText = "Tap the shop item again to hide the shop, then tap where you want the item on your planet, remember though not all items can live in the same environment.";
                break;

            case 7:
                tutorialText = "Congratulations! You have just taken your first step towards evolving your planet!";
                break;

            case 8:
                tutorialText = "This item will generate you credits while its alive! The more you have, the more credits you will earn!";
                break;

            case 9:
                tutorialText = "If you want to remove an item. Tap the meteor icon and tap the item to destroy! Also holding tap with the meteor selected, increases its range!";
                break;
        }

        tutorialTextObject.text = tutorialText;

    }

    public void StartTutorial(int phase)
    {
        tutorialPhase = phase;
        tutorialAnimator.Play("TutorialCompleteFadeReverse");
        UpdateText();
    }

    public void EndTutorial()
    {
        tutorialPhase = -1;
        tutorialAnimator.Play("TutorialCompleteFade");
    }

    private void LoadTutorial(PlayerData data)
    {
        tutorialPhase = data.tutorialSegment;
        Init();
    }

    private void NextSegment()
    {
        tutorialPhase++;

        tutorialAnimator.Play("TutorialFadeReverse");

        InvokeRepeating("CheckAnimationFinished", 0.0f, 0.1f);
    }

    private bool CheckAnimationFinished()
    {
        if (tutorialAnimator.IsInTransition(0))
        {
            CancelInvoke("CheckAnimationFinished");
            UpdateText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckPhaseComplete(int phaseNumber)
    {
        if (phaseNumber == tutorialPhase && tutorialPhase < 9)
        {
            NextSegment();
        }
        else if (tutorialPhase == 9)
        {
            EndTutorial();
        }
    }

    public void SkipSegment()
    {
        if (tutorialPhase < 9 && tutorialPhase != -1)
        {
            NextSegment();
        }
        else if (tutorialPhase == 9)
        {
            EndTutorial();
        }
    }


}
