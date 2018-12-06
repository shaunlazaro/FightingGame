using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager{
    public Action LowPunch;
    public Action HighPunch;
    public Action LowKick;
    public Action HighKick;

    public Action up;
    public Action down;
    public Action left;
    public Action right;
	
    // Returns the "Check" function of the named action
	public bool GetButtonDown(string s)
    {
        if (s == "LowPunch")
            return LowPunch.Check();
        else if (s == "HighPunch")
            return HighPunch.Check();
        else if (s == "LowKick")
            return LowKick.Check();
        else if (s == "HighKick")
            return HighKick.Check();
        else if (s == "down")
            return down.Check();
        else if (s == "up")
            return up.Check();
        else if (s == "right")
            return right.Check();
        else if (s == "left")
            return left.Check();
        else
            Debug.Log("Invalid GetButtonDown!");
        return false;
    }
}
