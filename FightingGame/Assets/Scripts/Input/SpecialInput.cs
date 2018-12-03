using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialInput{

    public string[] buttons;
    private int currentIndex = 0; //moves along the array as buttons are pressed

    public float allowedTimeBetweenButtons = 0.3f; //tweak as needed
    private float timeLastButtonPressed;

    private int stickNum = 1;
    // 0: Keyboard
    // 1,2,3...: Joystick Numbers

    // Create a specialinput object using a string array
    public SpecialInput(string[] b)
    {
        buttons = b;
    }



    // Second constructor which allows multiple controllers
    public SpecialInput(string[] b, int joyStickNum)
    {
        buttons = b;
        stickNum = joyStickNum;
    }

    //Call in update, does something when 
    public bool Check()
    {
        if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
        {
            if (currentIndex < buttons.Length)
            {
                if ((buttons[currentIndex] == "down" && Down()) ||
                (buttons[currentIndex] == "up" && Up()) ||
                (buttons[currentIndex] == "left" && Left()) ||
                (buttons[currentIndex] == "right" && Right()) ||
                (buttons[currentIndex] != "down" && buttons[currentIndex] != "up" && buttons[currentIndex] != "left"
                    && buttons[currentIndex] != "right" && Input.GetButtonDown(buttons[currentIndex] + "P" + stickNum)))
                {
                    timeLastButtonPressed = Time.time;
                    currentIndex++;
                }

                if (currentIndex >= buttons.Length)
                {
                    currentIndex = 0;
                    return true;
                }
                else
                    return false;
            }
        }

        return false;
    }

    private bool Down()
    {
        return Input.GetAxisRaw("Vertical" + "P" + stickNum) == -1;
    }
    private bool Up()
    {
        return Input.GetAxisRaw("Vertical" + "P" + stickNum) == 1;
    }
    private bool Left()
    {
        return Input.GetAxisRaw("Horizontal" + "P" + stickNum) == -1;
    }
    private bool Right()
    {
        return Input.GetAxisRaw("Horizontal" + "P" + stickNum) == 1;
    }
}
