using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialInput{

    public string[] buttons;
    private int currentIndex = 0; //moves along the array as buttons are pressed

    public float allowedTimeBetweenButtons = 0.3f; //tweak as needed
    private float timeLastButtonPressed;

    private int stickNum = 1;
    // 1: Player 1?
    // 2: Player 2?

    

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
                (buttons[currentIndex] != "down" && buttons[currentIndex] != "up" 
                    && buttons[currentIndex] != "left" && buttons[currentIndex] != "right" 
                    && Manager.instance.input[stickNum-1].GetButtonDown(buttons[currentIndex])))
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
        return Manager.instance.input[stickNum-1].GetButtonDown("down");
    }
    private bool Up()
    {
        return Manager.instance.input[stickNum-1].GetButtonDown("up");
    }
    private bool Left()
    {
        return Manager.instance.input[stickNum - 1].GetButtonDown("left");
    }
    private bool Right()
    {
        return Manager.instance.input[stickNum - 1].GetButtonDown("right");
    }
}
