using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    // -- Axis
    public static float MainHorizontal()
    {
        float result = 0.0f;
        result += Input.GetAxisRaw("J_MainHorizontal");
        result += Input.GetAxisRaw("K_MainHorizontal");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float MainVertical()
    {
        float result = 0.0f;
        result += Input.GetAxisRaw("J_MainVertical");
        result += Input.GetAxisRaw("K_MainVertical");
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static Vector3 MainJoystick()
    {
        return new Vector3(MainHorizontal(), 0, MainVertical());
    }

    // -- Buttons
    public static bool FaceButton1()
    {
        return Input.GetButtonDown("FaceButton1");
    }

    public static bool FaceButton2()
    {
        return Input.GetButtonDown("FaceButton2");
    }

    public static bool FaceButton3()
    {
        return Input.GetButtonDown("FaceButton3");
    }

    public static bool FaceButton4()
    {
        return Input.GetButtonDown("FaceButton4");
    }
}
