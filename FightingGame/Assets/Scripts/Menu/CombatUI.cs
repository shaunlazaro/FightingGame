using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatUI : MonoBehaviour {

    public GameObject healthBarLeft;
    public GameObject healthBarRight;
    private Manager manager = Manager.instance;

    public GameObject player1SpawnPoint;
    public GameObject player2SpawnPoint;
    public GameObject ground;
    public GameObject wall;

    public bool playerIsDead = false;
    public float victoryTime;

    public void UpdateUI()
    {
        healthBarLeft.transform.localScale = new Vector2(manager.currentHP[0] / manager.maxHP[0],1);
        healthBarRight.transform.localScale = new Vector2(manager.currentHP[1] / manager.maxHP[1],1);
    }

    void Start()
    {
        manager.SpawnPlayers(player1SpawnPoint, player2SpawnPoint, ground, wall);
        playerIsDead = false;
    }

	// Update is called once per frame
	void Update () {
        UpdateUI();

        if (!playerIsDead && manager.currentHP[0] <= 0)
        {
            Debug.Log("Player 1 Loss");
            playerIsDead = true;
            StartCoroutine(VictoryTimer());
        }
        if (!playerIsDead && manager.currentHP[1] <= 0)
        {
            Debug.Log("Player 2 Loss");
            playerIsDead = true;
            StartCoroutine(VictoryTimer());
        }
    }

    IEnumerator VictoryTimer()
    {
        Debug.Log("Waiting For Victory...");

        yield return new WaitForSeconds(victoryTime);

        Debug.Log("Done!");
        SceneManager.LoadScene("Title");
    }
}
