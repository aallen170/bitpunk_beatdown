using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class P1ConfigButtons : MonoBehaviour {

    Transform menuPanel;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    P1GameManager gm;

    int sceneIndex;

    // Use this for initialization
    void Start () {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        menuPanel = transform.Find("P1Buttons");
        waitingForKey = false;

        gm = P1GameManager.GM;
        if (sceneIndex == 2)
        {
            for (int i = 0; i < menuPanel.childCount; i++)
            {
                if (menuPanel.GetChild(i).name == "P1UpButton")
                {
                    print("found up");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.up.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1DownButton")
                {
                    print("found down");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.down.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1LeftButton")
                {
                    print("found left");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.left.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1RightButton")
                {
                    print("found right");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.right.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1JumpButton")
                {
                    print("found jump");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.jump.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1AttackButton")
                {
                    print("found attack");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.attack.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1GuardButton")
                {
                    print("found guard");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.guard.ToString();
                }
                else if (menuPanel.GetChild(i).name == "P1ProjectileButton")
                {
                    print("found projectile");
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                        gm.projectile.ToString();
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*if (sceneIndex == 2)
            menuPanel.gameObject.SetActive(true);
        else
            menuPanel.gameObject.SetActive(false);*/
    }

    void OnGUI()
    {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey();

        switch(keyName)
        {
            case "up":
                gm.up = newKey;
                buttonText.text = gm.up.ToString();
                PlayerPrefs.SetString("p1UpKey", gm.up.ToString());
                break;
            case "down":
                gm.down = newKey;
                buttonText.text = gm.down.ToString();
                PlayerPrefs.SetString("p1DownKey", gm.down.ToString());
                break;
            case "left":
                gm.left = newKey;
                buttonText.text = gm.left.ToString();
                PlayerPrefs.SetString("p1LeftKey", gm.left.ToString());
                break;
            case "right":
                gm.right = newKey;
                buttonText.text = gm.right.ToString();
                PlayerPrefs.SetString("p1RightKey", gm.right.ToString());
                break;
            case "jump":
                gm.jump = newKey;
                buttonText.text = gm.jump.ToString();
                PlayerPrefs.SetString("p1JumpKey", gm.jump.ToString());
                break;
            case "attack":
                gm.attack = newKey;
                buttonText.text = gm.attack.ToString();
                PlayerPrefs.SetString("p1AttackKey", gm.attack.ToString());
                break;
            case "guard":
                gm.guard = newKey;
                buttonText.text = gm.guard.ToString();
                PlayerPrefs.SetString("p1GuardKey", gm.guard.ToString());
                break;
            case "projectile":
                gm.projectile = newKey;
                buttonText.text = gm.projectile.ToString();
                PlayerPrefs.SetString("p1ProjectileKey", gm.projectile.ToString());
                break;
        }

        yield return null;
    }
}
