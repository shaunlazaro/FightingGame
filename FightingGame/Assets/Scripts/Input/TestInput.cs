using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

    public string[] specialAttackInput;
    private SpecialInput specialAttack;

    public int controllerNumber;

    void Start()
    {
        specialAttack = new SpecialInput(specialAttackInput,controllerNumber);
    }

    void Update()
    {
        if (specialAttack.Check())
            Debug.Log("Special Attack Input Read!");
        else if (Input.GetButtonDown("LowPunchP"+controllerNumber))
            Debug.Log("LowPunch!");
        else if (Input.GetButtonDown("LowKickP"+controllerNumber))
            Debug.Log("LowKick!");
        else if (Input.GetButtonDown("HighKickP"+controllerNumber))
            Debug.Log("HighKick!");
        else if (Input.GetButtonDown("HighPunchP"+controllerNumber))
            Debug.Log("HighPunch!");
        else
            Debug.Log("Rip!");
    }
}
