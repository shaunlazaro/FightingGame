using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

    public string[] specialAttackInput;
    private SpecialInput specialAttack;


    public int playerNum;

    void Start()
    {
        specialAttack = new SpecialInput(specialAttackInput,playerNum);
    }

    void Update()
    {
        if(specialAttack.Check())
        {
            Debug.Log("Special Atk");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("LowPunch"))
        {
            Debug.Log("LowPunch");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("HighPunch"))
        {
            Debug.Log("HighPunch");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("LowKick"))
        {
            Debug.Log("LowKick");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("HighKick"))
        {
            Debug.Log("HighKick");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("down"))
        {
            Debug.Log("Down");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("up"))
        {
            Debug.Log("Up");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("left"))
        {
            Debug.Log("Left");
        }
        else if (Manager.instance.input[playerNum-1].GetButtonDown("right"))
        {
            Debug.Log("Right");
        }

    }
}
