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
	}

	public void Update() {
        print("pauseHold = " + pauseHold);

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
