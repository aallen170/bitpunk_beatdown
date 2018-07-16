using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager GM;

    public KeyCode upKey { get; set; }
    public KeyCode downKey { get; set; }
    public KeyCode leftKey { get; set; }
    public KeyCode rightKey { get; set; }
    public KeyCode jumpKey { get; set; }
    public KeyCode attackKey { get; set; }
    public KeyCode guardKey { get; set; }
    public KeyCode projectileKey { get; set; }

    private void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if(GM != this)
        {
            Destroy(gameObject);
        }

        upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("upKey", "W"));
        downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("downKey", "S"));
        leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("leftKey", "A"));
        rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("rightKey", "D"));
        jumpKey = (KeyCode) System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("jumpKey", "J"));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("attackKey", "K"));
        guardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("guardKey", "L"));
        projectileKey = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("projectileKey", "Space"));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
