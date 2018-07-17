using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1ConfigButtons : MonoBehaviour {

    Transform menuPanel;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    P1GameManager gm;

    // Use this for initialization
    void Start () {
        menuPanel = transform.Find("P1Buttons");
        waitingForKey = false;

        gm = P1GameManager.GM;

        for(int i = 0; i < menuPanel.childCount; i++)
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
	
	// Update is called once per frame
	void Update () {
        //print("test");
	}
}
