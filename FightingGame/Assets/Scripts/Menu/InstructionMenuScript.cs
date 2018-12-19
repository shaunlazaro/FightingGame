using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionMenuScript : MonoBehaviour {

    public string titleName;

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(titleName);
    }
}
