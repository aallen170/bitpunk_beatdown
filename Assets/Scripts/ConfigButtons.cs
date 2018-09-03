using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * The following is a modification of code from Mark Philip of Studica
 *
 * Original code can be found in this video tutorial: https://www.youtube.com/watch?v=iSxifRKQKAA
 */
public class ConfigButtons : MonoBehaviour {

    Transform p1Config, p2Config;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;
    string buttonName;

    bool waitingForStick;
    bool waitingForKey;

    P1GameManager p1GM;
    P2GameManager p2GM;

    int sceneIndex;

    // Use this for initialization
    void Start () {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (gameObject.tag == "P1Config")
        {
            p1Config = transform.Find("Buttons");
            p1GM = P1GameManager.GM;
            waitingForKey = false;
            if (sceneIndex == 2)
            {
                for (int i = 0; i < p1Config.childCount; i++)
                {
                    if (p1Config.GetChild(i).name == "UpButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.up.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "DownButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.down.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "LeftButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.left.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "RightButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.right.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "JumpButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.jump.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "AttackButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.attack.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "GuardButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.guard.ToString();
                    }
                    else if (p1Config.GetChild(i).name == "ProjectileButton")
                    {
                        p1Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p1GM.projectile.ToString();
                    }
                }
            }
        }
        if (gameObject.tag == "P2Config")
        {
            p2Config = transform.Find("Buttons");
            p2GM = P2GameManager.GM;
            waitingForKey = false;
            if (sceneIndex == 2)
            {
                for (int i = 0; i < p2Config.childCount; i++)
                {
                    if (p2Config.GetChild(i).name == "UpButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.up.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "DownButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.down.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "LeftButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.left.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "RightButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.right.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "JumpButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.jump.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "AttackButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.attack.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "GuardButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.guard.ToString();
                    }
                    else if (p2Config.GetChild(i).name == "ProjectileButton")
                    {
                        p2Config.GetChild(i).GetComponentInChildren<Text>().text =
                            p2GM.projectile.ToString();
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //print(Input.GetAxis("J_Horizontal"));
        //print(Input.GetButton("FaceButton1"));
        /*if (Input.GetButton("joystick button 2"))
            print("pressing a");*/

        /*if (sceneIndex == 2)
            menuPanel.gameObject.SetActive(true);
        else
            menuPanel.gameObject.SetActive(false);*/
        //int i = 0;
        //while (i < 4)
        //{
        //    if (Mathf.Abs(Input.GetAxis("Joy" + i + "X")) > 0.2F || Mathf.Abs(Input.GetAxis("Joy" + i + "Y")) > 0.2F)
        //        Debug.Log(Input.GetJoystickNames()[i] + " is moved");
        //    i++;
        //}

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            print(Input.GetJoystickNames()[i]);
        }
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
        print("waiting for key");
        while (!keyEvent.isKey)
        {
            Input.GetButton("FaceButton1");
            yield return null;
        }

        
    }

    public IEnumerator AssignStick(string stickName)
    {
        waitingForStick = true;

        yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey();

        if (gameObject.tag == "P1Config")
        {
            switch (keyName)
            {
                case "up":
                    p1GM.up = newKey;
                    buttonText.text = p1GM.up.ToString();
                    PlayerPrefs.SetString("p1UpKey", p1GM.up.ToString());
                    break;
                case "down":
                    p1GM.down = newKey;
                    buttonText.text = p1GM.down.ToString();
                    PlayerPrefs.SetString("p1DownKey", p1GM.down.ToString());
                    break;
                case "left":
                    p1GM.left = newKey;
                    buttonText.text = p1GM.left.ToString();
                    PlayerPrefs.SetString("p1LeftKey", p1GM.left.ToString());
                    break;
                case "right":
                    p1GM.right = newKey;
                    buttonText.text = p1GM.right.ToString();
                    PlayerPrefs.SetString("p1RightKey", p1GM.right.ToString());
                    break;
                case "jump":
                    p1GM.jump = newKey;
                    buttonText.text = p1GM.jump.ToString();
                    PlayerPrefs.SetString("p1JumpKey", p1GM.jump.ToString());
                    break;
                case "attack":
                    p1GM.attack = newKey;
                    buttonText.text = p1GM.attack.ToString();
                    PlayerPrefs.SetString("p1AttackKey", p1GM.attack.ToString());
                    break;
                case "guard":
                    p1GM.guard = newKey;
                    buttonText.text = p1GM.guard.ToString();
                    PlayerPrefs.SetString("p1GuardKey", p1GM.guard.ToString());
                    break;
                case "projectile":
                    p1GM.projectile = newKey;
                    buttonText.text = p1GM.projectile.ToString();
                    PlayerPrefs.SetString("p1ProjectileKey", p1GM.projectile.ToString());
                    break;
            }
        }
        if (gameObject.tag == "P2Config")
        {
            switch (keyName)
            {
                case "up":
                    p2GM.up = newKey;
                    buttonText.text = p2GM.up.ToString();
                    PlayerPrefs.SetString("p2UpKey", p2GM.up.ToString());
                    break;
                case "down":
                    p2GM.down = newKey;
                    buttonText.text = p2GM.down.ToString();
                    PlayerPrefs.SetString("p2DownKey", p2GM.down.ToString());
                    break;
                case "left":
                    p2GM.left = newKey;
                    buttonText.text = p2GM.left.ToString();
                    PlayerPrefs.SetString("p2LeftKey", p2GM.left.ToString());
                    break;
                case "right":
                    p2GM.right = newKey;
                    buttonText.text = p2GM.right.ToString();
                    PlayerPrefs.SetString("p2RightKey", p2GM.right.ToString());
                    break;
                case "jump":
                    p2GM.jump = newKey;
                    buttonText.text = p2GM.jump.ToString();
                    PlayerPrefs.SetString("p2JumpKey", p2GM.jump.ToString());
                    break;
                case "attack":
                    p2GM.attack = newKey;
                    buttonText.text = p2GM.attack.ToString();
                    PlayerPrefs.SetString("p2AttackKey", p2GM.attack.ToString());
                    break;
                case "guard":
                    p2GM.guard = newKey;
                    buttonText.text = p2GM.guard.ToString();
                    PlayerPrefs.SetString("p2GuardKey", p2GM.guard.ToString());
                    break;
                case "projectile":
                    p2GM.projectile = newKey;
                    buttonText.text = p2GM.projectile.ToString();
                    PlayerPrefs.SetString("p2ProjectileKey", p2GM.projectile.ToString());
                    break;
            }
        }

        yield return null;
    }
}
