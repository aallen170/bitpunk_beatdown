using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour {

    P1Score p1Score;
    P2Score p2Score;

    Canvas p1WinCanvas, p2WinCanvas;

    GameObject p1Object, p2Object;

    void Start () {
        p1Object = GameObject.FindGameObjectWithTag("Player1");
        p2Object = GameObject.FindGameObjectWithTag("Player2");
        p1Score = GameObject.FindGameObjectWithTag("P1Score").
            GetComponent<P1Score>();
        p2Score = GameObject.FindGameObjectWithTag("P2Score").
            GetComponent<P2Score>();
        //p1WinCanvas = GameObject.FindGameObjectWithTag("P1Win").
        //    GetComponent<Canvas>();
        //p2WinCanvas = GameObject.FindGameObjectWithTag("P2Win").
        //    GetComponent<Canvas>();
        //p1WinCanvas.enabled = p2WinCanvas.enabled = false;
        print(p1Object);
        print(p2Object);
    }

    private void Update()
    {
        //if (p1Score.gameScore == 5)
        //{
        //    p1WinCanvas.enabled = true;
        //    Destroy(p2Object);
        //}
        //if (p2Score.gameScore == 5)
        //{
        //    p2WinCanvas.enabled = true;
        //    Destroy(p1Object);
        //}
    }
}
