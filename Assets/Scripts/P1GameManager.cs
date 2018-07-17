using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            PlayerPrefs.GetString("p1UpKey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1DownKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1LeftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1RightKey", "D"));
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1JumpKey", "J"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1AttackKey", "K"));
        guard = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1GuardKey", "L"));
        projectile = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            PlayerPrefs.GetString("p1ProjectileKey", "Space"));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
