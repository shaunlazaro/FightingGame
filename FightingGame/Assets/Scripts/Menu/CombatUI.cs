using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatUI : MonoBehaviour {

    public GameObject healthBarLeft;
    public GameObject healthBarRight;
    private Manager manager = Manager.instance;

	public void UpdateUI()
    {
        healthBarLeft.transform.localScale = new Vector2(manager.currentHP[0] / manager.maxHP[0],1);
        healthBarRight.transform.localScale = new Vector2(manager.currentHP[1] / manager.maxHP[1],1);
    }

	// Update is called once per frame
	void Update () {
        UpdateUI();

        if (manager.currentHP[0] <= 0)
        {
            Debug.Log("Player 1 Loss");
            SceneManager.LoadScene("Title");
        }
        if (manager.currentHP[1] <= 0)
        {
            Debug.Log("Player 2 Loss");
            SceneManager.LoadScene("Title");
        }
    }
}
