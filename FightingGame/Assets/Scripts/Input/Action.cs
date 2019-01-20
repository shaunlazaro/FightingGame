using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action{
    
    public string input;

    // Only used in an axis
    public bool isAxis = false;
    public float axisTargetValue;

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
            axisTargetValue = -0.5f;
        else
            axisTargetValue = 0.5f;
    }
    

    public bool Check()
    {
        if (isAxis && axisTargetValue > 0)
            return Input.GetAxisRaw(input) >= axisTargetValue;
        if (isAxis && axisTargetValue < 0)
            return Input.GetAxisRaw(input) <= axisTargetValue;
        return Input.GetKeyDown(input);
    }
}
