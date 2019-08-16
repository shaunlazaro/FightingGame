using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterSelectController : MonoBehaviour {


    // Will be used to do character selection and stuff eventually.
    public string gameScene;

    public float mouseSpeed;

    public GameObject mouse1;
    private RectTransform p1Mouse;
    public GameObject mouse2;
    private RectTransform p2Mouse;

    public RectTransform character1Image;
    public RectTransform character2Image;

    private InputManager input1;
    private InputManager input2;

    public GameObject boxerTemplate;
    public GameObject fencerTemplate;

    public void Start()
    {
        p1Mouse = mouse1.GetComponent<RectTransform>();
        p2Mouse = mouse2.GetComponent<RectTransform>();

        input1 = Manager.instance.input[0];
        input2 = Manager.instance.input[1];
    }

    public void Update()
    {
        if (input1.GetButtonDown("up"))
        {
            p1Mouse.anchoredPosition += new Vector2(0, mouseSpeed);
        }
        else if (input1.GetButtonDown("down"))
        {
            p1Mouse.anchoredPosition += new Vector2(0, -mouseSpeed);
        }
        if (input1.GetButtonDown("right"))
        {
            p1Mouse.anchoredPosition += new Vector2(mouseSpeed, 0);
        }
        else if (input1.GetButtonDown("left"))
        {
            p1Mouse.anchoredPosition += new Vector2(-mouseSpeed, 0);
        }
        if (input2.GetButtonDown("up"))
        {
            p2Mouse.anchoredPosition += new Vector2(0, mouseSpeed);
        }
        else if (input2.GetButtonDown("down"))
        {
            p2Mouse.anchoredPosition += new Vector2(0, -mouseSpeed);
        }
        if (input2.GetButtonDown("right"))
        {
            p2Mouse.anchoredPosition += new Vector2(mouseSpeed, 0);
        }
        else if (input2.GetButtonDown("left"))
        {
            p2Mouse.anchoredPosition += new Vector2(-mouseSpeed, 0);
        }

        if (input1.GetButtonDown("StrongAttack"))
        {
            CheckCharacterOverlaps(p1Mouse, 1);
        }
        if (input2.GetButtonDown("StrongAttack"))
        {
            CheckCharacterOverlaps(p2Mouse, 2);
        }

    }

    public void CheckCharacterOverlaps(RectTransform rect, int playerNum)
    {
        Debug.Log("Player " + playerNum + "tries to select...");
        if (RectOverlaps(rect, character1Image))
        {
            Debug.Log("Player " + playerNum + "selected boxer");
            Manager.instance.PlayerSelectedTemplate(playerNum, boxerTemplate);
        }
        else if (RectOverlaps(rect, character2Image))
        {
            Debug.Log("Player " + playerNum + "selected fencer");
            Manager.instance.PlayerSelectedTemplate(playerNum, fencerTemplate);
        }
    }

    public bool RectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}





