using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	/*Image quitBanner, yesBanner, noBanner;
	Text quitText, yesText, noText;
	Button yesButton, noButton;*/

	Canvas pauseCanvas, quitCanvas;

	int pauseCount = 0;
	int sceneIndex;
	int pauseHold = 0;

	bool paused;

    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    /*Transform p1MenuPanel, p2MenuPanel;
    Transform p1ConfigButtons, p2ConfigButtons;*/

    public void MainMenu() {
		SceneManager.LoadScene (0);
	}

	public void StageSelect() {
		SceneManager.LoadScene (1);
	}

    public void ButtonConfig()
    {
        SceneManager.LoadScene(2);
    }

	public void Stage1() {
		SceneManager.LoadScene (3);
	}

	public void Stage2() {
		SceneManager.LoadScene (4);
	}

	public void QuitBoxYes() {
		Application.Quit ();
	}

	public void QuitMenu() {
		quitCanvas.enabled = true;
		pauseCanvas.enabled = false;
	}

	public void QuitBoxNo() {
		quitCanvas.enabled = false;
	}

	public void Start() {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		if (sceneIndex != 2 && sceneIndex != 1 && sceneIndex != 0) {
			quitCanvas = GameObject.FindGameObjectWithTag ("Quit").GetComponent<Canvas> ();
			pauseCanvas = GameObject.FindGameObjectWithTag ("Pause").GetComponent<Canvas> ();
			quitCanvas.enabled = false;
			pauseCanvas.enabled = false;
		}
        /*p1MenuPanel = transform.Find("P1");
        p1ConfigButtons = p1MenuPanel.Find("P1Buttons");
        //p2ConfigButtons = transform.Find("P2Buttons");
        print(p1ConfigButtons);
        /*for (int i = 0; i < 5; i++)
        {
            if (p1ConfigButtons.GetChild(i).name == "P1UpButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.upKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1DownButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.downKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1LeftButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.leftKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1RightButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.rightKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1JumpButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.jumpKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1AttackButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.attackKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1GuardButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.guardKey.ToString();
            else if (p1ConfigButtons.GetChild(i).name == "P1ProjectileButton")
                p1ConfigButtons.GetChild(i).GetComponentInChildren<Text>().text =
                    GameManager.GM.projectileKey.ToString();
        }*/
        //print("test");
    }

	public void Update() {
        print("pauseHold = " + pauseHold);

        if (paused)
            waitingForKey = true;
        else
            waitingForKey = false;

        if (Input.GetKeyDown("escape") &&
            (sceneIndex == 1 || sceneIndex == 2))
            SceneManager.LoadScene(0);

        if (Input.GetKey ("escape"))
			pauseHold++;

		if (Input.GetKeyUp ("escape"))
			pauseHold = 0;

		if (pauseHold >= 180)
			Application.Quit ();

		if (sceneIndex != 2 && sceneIndex != 1 && sceneIndex != 0) {
			print ("scene check running");
			paused = pauseCanvas.enabled;

			if (Input.GetKeyDown ("escape"))
				pauseCount++;

			if (pauseCount >= 2)
				pauseCount = 0;

			if (pauseCount % 2 == 1 && !quitCanvas.enabled) {
				pauseCanvas.enabled = true;
			} else if (pauseCount % 2 == 0 && !quitCanvas.enabled) {
				pauseCanvas.enabled = false;
			} else if (quitCanvas.enabled && Input.GetKeyDown ("escape")) {
				quitCanvas.enabled = false;
			}
		}
	}
}
