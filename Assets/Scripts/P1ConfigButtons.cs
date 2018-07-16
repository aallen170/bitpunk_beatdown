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

        for(int i = 0; i < 5; i++)
        {
            if (menuPanel.GetChild(i).name == "P1UpButton")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                    gm.up.ToString();
            else if (menuPanel.GetChild(i).name == "P1DownButton")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                    gm.down.ToString();
            else if (menuPanel.GetChild(i).name == "P1LeftButton")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                    gm.left.ToString();
            else if (menuPanel.GetChild(i).name == "P1RightButton")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                    gm.right.ToString();
            else if (menuPanel.GetChild(i).name == "P1JumpButton")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text =
                    gm.jump.ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {
        //print("test");
	}
}
