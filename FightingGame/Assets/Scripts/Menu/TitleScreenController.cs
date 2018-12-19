using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour {

    public string optionScene;
    public string characterSelectScene;
    public string instructionScene;

    public void Start()
    {
        Manager.instance.ResetNumbers();
    }

	public void OptionButton()
    {
        Debug.Log("Function");
        SceneManager.LoadScene(optionScene);
    }
    public void InfoButton()
    {
        SceneManager.LoadScene(instructionScene);
    }
    public void StartGameButton()
    {
        SceneManager.LoadScene(characterSelectScene);
    }

}
