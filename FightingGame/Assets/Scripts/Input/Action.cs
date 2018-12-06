using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action{
    
    public string input;

    // Only used in an axis
    public bool isAxis = false;
    public int axisTargetValue;

    // Button constructor
    public Action(string rawInput)
    {
        input = rawInput;
    }
    // Axis constructor
    public Action(string rawInput, bool negativeAxis)
    {
        input = rawInput;
        isAxis = true;
        if (negativeAxis)
            axisTargetValue = -1;
        else
            axisTargetValue = 1;
    }
    

    public bool Check()
    {
        if (isAxis)
            return Input.GetAxisRaw(input) == axisTargetValue;
        return Input.GetKeyDown(input);
    }
}
