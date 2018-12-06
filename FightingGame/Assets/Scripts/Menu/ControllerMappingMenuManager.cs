using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerMappingMenuManager : MonoBehaviour {

    public GameObject displayObject;
    private Image displaySprite;

    private int progress = 0;
    
    // Size should always be 9!
    public Sprite[] pics;
    /*
     * 0: Empty
     * 1: HP (left)
     * 2: HK (down)
     * 3: LK (right)
     * 4: LP (up)
     * 5: Up
     * 6: Left
     * 7: Down
     * 8: Right
     */

    public int playerNumber;
    public GameObject controllerNumberTextObject;
    private Text controllerNumberText;
    public GameObject instructionsTextObject;
    private Text instructionsText;

    bool waiting = false;

	// Use this for initialization
	void Start () {
        Manager.instance.gameObject.name = "MappingManager";

        displaySprite = displayObject.GetComponent<Image>();
        controllerNumberText = controllerNumberTextObject.GetComponent<Text>();
        controllerNumberText.text = "Player Number: " + playerNumber;

        instructionsText = instructionsTextObject.GetComponent<Text>();

        displaySprite.sprite = pics[0];
        StartCoroutine(Progression());
    }

    IEnumerator Progression()
    {
        progress++;
        displaySprite.sprite = pics[0];
        waiting = true;
        yield return new WaitForSeconds(0.5f);
        if (progress < pics.Length)
        {
            displaySprite.sprite = pics[progress];
        }
        waiting = false;
        if(progress == 9)
        {
            instructionsText.text = "Press HP button to assign player 2 controls.\n" +
                "Press LK to return to main menu.";
        }
    }

    void AssignControl(string input)
    {
        switch (progress)
        {
            case 1:
                Manager.instance.input[playerNumber - 1].HighPunch = new Action(input);
                break;
            case 2:
                Manager.instance.input[playerNumber - 1].HighKick = new Action(input);
                break;
            case 3:
                Manager.instance.input[playerNumber - 1].LowKick = new Action(input);
                break;
            case 4:
                Manager.instance.input[playerNumber - 1].LowPunch = new Action(input);
                break;
            case 5:
                Manager.instance.input[playerNumber - 1].left = new Action(input, true);
                break;
            case 6:
                Manager.instance.input[playerNumber - 1].down = new Action(input, true);
                break;
            case 7:
                Manager.instance.input[playerNumber - 1].right = new Action(input, false);
                break;
            case 8:
                Manager.instance.input[playerNumber - 1].up = new Action(input, false);
                break;
            default:
                Debug.Log("Unlucky!");
                break;
        }
    }
    void AssignControl(string input, bool isAxisDown)
    {
        switch (progress)
        {
            case 1:
                Manager.instance.input[playerNumber - 1].HighPunch = new Action(input);
                break;
            case 2:
                Manager.instance.input[playerNumber - 1].HighKick = new Action(input);
                break;
            case 3:
                Manager.instance.input[playerNumber - 1].LowKick = new Action(input);
                break;
            case 4:
                Manager.instance.input[playerNumber - 1].LowPunch = new Action(input);
                break;
            case 5:
                Manager.instance.input[playerNumber - 1].left = new Action(input, isAxisDown);
                break;
            case 6:
                Manager.instance.input[playerNumber - 1].down = new Action(input, isAxisDown);
                break;
            case 7:
                Manager.instance.input[playerNumber - 1].right = new Action(input, isAxisDown);
                break;
            case 8:
                Manager.instance.input[playerNumber - 1].up = new Action(input, isAxisDown);
                break;
            default:
                Debug.Log("Unlucky!");
                break;
        }
    }

    void Update()
    {
        if (progress < 5 && !waiting)
        {
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (Input.GetKeyDown("joystick " + j + " button " + i))
                    {
                        AssignControl("joystick " + j + " button " + i);
                        StartCoroutine(Progression());
                    }
                }
            }
        }

        if (progress > 4 && !waiting && progress <= 8)
        {
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if(Input.GetAxisRaw("Joy"+j+"A"+i) < 0)
                    {
                        AssignControl("Joy" + j + "A" + i, true);
                        StartCoroutine(Progression());
                    }
                    else if (Input.GetAxisRaw("Joy"+j+"A"+i) > 0)
                    {
                        AssignControl("Joy" + j + "A" + i, false);
                        StartCoroutine(Progression());
                    }
                }
            }
        }
        
        if(progress == 9)
        {
            if (Manager.instance.input[playerNumber-1].GetButtonDown("HighPunch"))
            {
                progress = 0;
                playerNumber++;
                instructionsText.text = "Now assigning player 2 controls." +
                    "\nPlease put down the player 1 controller";
                Start();
            }
            else if (Manager.instance.input[playerNumber-1].GetButtonDown("LowKick"))
            {
                SceneManager.LoadScene("Title");
            }
        }
        if(Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
