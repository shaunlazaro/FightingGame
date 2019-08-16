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
        if (s == "WeakAttack")
            return LowPunch.Check();
        else if (s == "StrongAttack")
            return HighPunch.Check();
        else if (s == "Throw")
            return LowKick.Check();
        else if (s == "Special")
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
