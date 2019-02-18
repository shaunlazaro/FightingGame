using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    // Singleton Code
    public static Manager instance = null;

    public InputManager[] input = new InputManager[2];

    public float[] maxHP = new float[2]; // 0 is 1, 1 is 2
    public float[] currentHP = new float[2]; // 0 is 1, 1 is 2

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
}

// Access using: Manager.instance.player1, etc.
