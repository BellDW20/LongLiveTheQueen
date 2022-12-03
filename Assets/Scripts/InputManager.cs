using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    private static int[] pNumToJoyNum = new int[] { 0, 1, 2, 3 };

    private static KeyCode[] dashJoyCodes = new KeyCode[] {
        KeyCode.LeftShift,
        KeyCode.Joystick1Button4,
        KeyCode.Joystick2Button4,
        KeyCode.Joystick3Button4
    };
    private static KeyCode[] backJoyCodes = new KeyCode[] {
        KeyCode.Escape,
        KeyCode.Joystick1Button6,
        KeyCode.Joystick2Button6,
        KeyCode.Joystick3Button6,
    };
    private static KeyCode[] reloadJoyCodes = new KeyCode[] {
        KeyCode.R,
        KeyCode.Joystick1Button2,
        KeyCode.Joystick2Button2,
        KeyCode.Joystick3Button2,
    };
    private static KeyCode[] pickupJoyCodes = new KeyCode[] {
        KeyCode.E,
        KeyCode.Joystick1Button0,
        KeyCode.Joystick2Button0,
        KeyCode.Joystick3Button0
    };
    private static KeyCode[] dropJoyCodes = new KeyCode[] {
        KeyCode.T,
        KeyCode.Joystick1Button1,
        KeyCode.Joystick2Button1,
        KeyCode.Joystick3Button1
    };
    private static KeyCode[] swapJoyCodes = new KeyCode[] {
        KeyCode.Tab,
        KeyCode.Joystick1Button3,
        KeyCode.Joystick2Button3,
        KeyCode.Joystick3Button3
    };

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

    public static bool GetDashInput(int joyNum) {
        return Input.GetKeyDown(dashJoyCodes[joyNum]);
    }

    public static bool GetSpecialInput(int joyNum) {
        if (joyNum == 0) {
            return Input.GetMouseButtonDown(1);
        } else {
            return Input.GetAxis("J " + (joyNum + 1) + " Special") > 0.1;
        }
    }

    public static bool GetFireInput(int joyNum) {
        if (joyNum == 0) {
            return Input.GetMouseButton(0);
        } else {
            return Input.GetAxis("J " + (joyNum + 1) + " Fire") > 0.1;
        }
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

    public static bool GetReloadInput(int joyNum) {
        return Input.GetKeyDown(reloadJoyCodes[joyNum]);
    }

    public static bool GetBackInput(int joyNum) {
        return Input.GetKeyDown(backJoyCodes[joyNum]);
    }

    public static bool GetPickupInput(int joyNum) {
        return Input.GetKeyDown(pickupJoyCodes[joyNum]);
    }

    public static bool GetDropInput(int joyNum) {
        return Input.GetKeyDown(dropJoyCodes[joyNum]);
    }

    public static bool GetSwapInput(int joyNum) {
        return Input.GetKeyDown(swapJoyCodes[joyNum]);
    }

}
