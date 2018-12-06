using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    // Singleton Code
    public static Manager instance = null;

    public InputManager[] input = new InputManager[2];

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
    }
}

// Access using: Manager.instance.player1, etc.
