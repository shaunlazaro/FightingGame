using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterSelectController : MonoBehaviour {


    // Will be used to do character selection and stuff eventually.
    public string gameScene;
    public void BeginGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}
