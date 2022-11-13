using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    private static int[] pNumToJoyNum = new int[] {0,1,2,3};

    public static void AssignPlayerToJoystick(int playerNum, int joyNum) {
        pNumToJoyNum[playerNum] = joyNum;
    }

    public static int GetPlayerAssignedJoystick(int playerNum) {
        return pNumToJoyNum[playerNum];
    }

    /*
     * These methods get input according to joystick number, NOT player number!
     */
    public static float GetHorizontalInput(int joyNum) {
        if(joyNum == 0) {
            return Input.GetAxis("Horizontal");
        } else {
            return Input.GetAxis("J " + (joyNum + 1) + " Horizontal");
        }
    }

    public static float GetVerticalInput(int joyNum) {
        if (joyNum == 0) {
            return Input.GetAxis("Vertical");
        }
        else {
            return Input.GetAxis("J " + (joyNum + 1) + " Vertical");
        }
    }

    public static bool GetDashInput(int joyNum)
    {
        if (joyNum == 0) 
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        else if (joyNum == 1)
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button4);
        }

        return false;
    }

    public static bool GetSpecialInput(int joyNum)
    {
        if (joyNum == 0)
        {
            return Input.GetMouseButtonDown(1);
        }
        else if (joyNum == 1)
        {
            return Input.GetAxis("J " + (joyNum + 1) + " Special") > 0.1;
        }

        return false;
    }

    public static bool GetFireInput(int joyNum)
    {
        if (joyNum == 0)
        {
            return Input.GetMouseButton(0);
        }
        else if (joyNum == 1)
        {
            return Input.GetAxis("J " + (joyNum + 1) + " Fire") > 0.1;
        }

        return false;
    }

    public static Vector2 GetAimInput(Camera cam, Vector2 pos, int joyNum) {
        if(joyNum == 0) {
            return (Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - pos;
        } else {
            return new Vector2(
                Input.GetAxis("J " + (joyNum + 1) + " AimHorizontal"),
                Input.GetAxis("J " + (joyNum + 1) + " AimVertical")
            );
        }
    }

    public static bool GetReloadInput(int joyNum)
    {
        if (joyNum == 0)
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        else if (joyNum == 1)
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button2);
        }

        return false;
    }

    public static bool GetBackInput(int joyNum)
    {
        if (joyNum == 0)
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
        else if (joyNum == 1)
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button1);
        }

        return false;
    }
}
