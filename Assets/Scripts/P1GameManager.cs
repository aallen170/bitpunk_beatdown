using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1GameManager : MonoBehaviour {

    public static P1GameManager GM;

    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode attack { get; set; }
    public KeyCode guard { get; set; }
    public KeyCode projectile { get; set; }

    void Awake()
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

        up = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("upKey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("downKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("rightKey", "D"));
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("jumpKey", "J"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("attackKey", "K"));
        guard = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("guardKey", "L"));
        projectile = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("projectileKey", "Space"));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
