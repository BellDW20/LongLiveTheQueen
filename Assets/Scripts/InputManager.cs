using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static float GetHorizontalKeyboard()
    {
        return Input.GetAxis("Horizontal");
    }

    public static float GetVerticalKeyboard()
    {
        return Input.GetAxis("Vertical");
    }

    public static float GetHorizontalGamepad(int playerNum)
    {
        return Input.GetAxis("J " + (playerNum + 1) + " Horizontal");
    }

    public static float GetVerticalGamepad(int playerNum)
    {
        return Input.GetAxis("J " + (playerNum + 1) + " Vertical");
    }

    public static bool GetDashInput(int playerNum)
    {
        if (playerNum == 0) 
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        else if (playerNum == 1)
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button4);
        }

        return false;
    }

    public static bool GetSpecialInput(int playerNum)
    {
        if (playerNum == 0)
        {
            return Input.GetMouseButtonDown(1);
        }
        else if (playerNum == 1)
        {
            return Input.GetAxis("J " + (playerNum + 1) + " Special") > 0.1;
        }

        return false;
    }

    public static bool GetFireInput(int playerNum)
    {
        if (playerNum == 0)
        {
            return Input.GetMouseButton(0);
        }
        else if (playerNum == 1)
        {
            return Input.GetAxis("J " + (playerNum + 1) + " Fire") > 0.1;
        }

        return false;
    }

    public static float GetAimInputGamepadHorizontal(int playerNum)
    {
        if (playerNum == 0)
        {
            return 0;
        }
        else if (playerNum == 1)
        {
            return Input.GetAxis("J " + (playerNum + 1) + " AimHorizontal");
        }

        return 0;
    }

    public static float GetAimInputGamepadVertical(int playerNum)
    {
        if (playerNum == 0)
        {
            return 0;
        }
        else if (playerNum == 1)
        {
            return Input.GetAxis("J " + (playerNum + 1) + " AimVertical");
        }

        return 0;
    }
}
