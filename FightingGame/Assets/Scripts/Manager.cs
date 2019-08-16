using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    // Singleton Code
    public static Manager instance = null;

    public InputManager[] input = new InputManager[2];

    public float[] maxHP = new float[2]; // 0 is 1, 1 is 2
    public float[] currentHP = new float[2]; // 0 is 1, 1 is 2

    public GameObject player1Template;
    public GameObject player2Template;

    // Singleton Code
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        input[0] = new InputManager();
        input[1] = new InputManager();

        maxHP[0] = 100;
        maxHP[1] = 100;
    }

    public void ResetNumbers()
    {
        currentHP[0] = maxHP[0];
        currentHP[1] = maxHP[1];
    }

    public void PlayerSelectedTemplate(int playerNum, GameObject template)
    {
        if (playerNum == 1)
        {
            player1Template = template;
        }
        if (playerNum == 2)
        {
            player2Template = template;
        }
    }

    public void SpawnPlayers(GameObject location1, GameObject location2, GameObject ground, GameObject wall)
    {
        GameObject p1 = Instantiate(player1Template, location1.transform.position, transform.rotation);
        GameObject p2 = Instantiate(player2Template, location2.transform.position, transform.rotation);

        p1.GetComponent<TestAnimInput>().opponent = p2;
        p1.GetComponent<TestAnimInput>().playerNum = 1;
        p1.GetComponent<TestAnimInput>().ground = ground.GetComponent<BoxCollider2D>();
        p1.GetComponent<TestAnimInput>().walls = wall.GetComponents<BoxCollider2D>();
        p2.GetComponent<TestAnimInput>().opponent = p1;
        p2.GetComponent<TestAnimInput>().playerNum = 2;
        p2.GetComponent<TestAnimInput>().ground = ground.GetComponent<BoxCollider2D>();
        p2.GetComponent<TestAnimInput>().walls = wall.GetComponents<BoxCollider2D>();
        Time.timeScale = 1;
    }

}

// Access using: Manager.instance.player1, etc.
