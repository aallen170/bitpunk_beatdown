using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * The following is a modification of code from Mark Philip of Studica
 *
 * Original code can be found in this video tutorial: https://www.youtube.com/watch?v=iSxifRKQKAA
 */
public class P2GameManager : MonoBehaviour {

    public static P2GameManager GM;

    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode attack { get; set; }
    public KeyCode guard { get; set; }
    public KeyCode projectile { get; set; }

    int sceneIndex;

    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

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
            PlayerPrefs.GetString("p2UpKey", "UpArrow"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2DownKey", "DownArrow"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2LeftKey", "LeftArrow"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2RightKey", "RightArrow"));
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2JumpKey", "Keypad1"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2AttackKey", "Keypad2"));
        guard = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2GuardKey", "Keypad3"));
        projectile = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p2ProjectileKey", "Keypad0"));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
