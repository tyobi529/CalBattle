using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseController : MonoBehaviour
{
    public GameObject PauseObj = null;

    [SerializeField] GameObject ingredientObj = null;

    [SerializeField] Transform[] handTransform = new Transform[4];

    private GameManager gameManager = null;

    public bool pause = false;

    float time = 0f;
    float sin = 0f;
    Color color = Color.white;

    int[] cardID = new int[4] { -1, -1, -1, -1 };


    [SerializeField] GameObject quitGameObj = null;
    [SerializeField] GameObject quitGameButtonObj = null;
    [SerializeField] Button leaveButton = null;

    [SerializeField] TextMeshProUGUI surrenderText = null;

    
    //[SerializeField] TextMeshProUGUI[] surrenderButtonText = new TextMeshProUGUI[2];


    float quitTime = 0f;
    bool canQuit = false;

    private void Update()
    {
        if (pause)
        {
            time += 1f / 60f;

            sin = Mathf.Sin((time * 10f) / 2f + 0.5f);

            if (sin < 140f / 255f)
            {
                sin = 140f / 255f;
            }

            color.b = sin;

            for (int i = 0; i < 4; i++)
            {
                if (cardID[i] == -1)
                {
                    continue;
                }

                ingredientObj.transform.GetChild(cardID[i]).GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, sin);

            }
        }

        if (canQuit)
        {
            quitTime += 1f / 60f;

            if (quitTime > 1.5f)
            {
                canQuit = false;
                leaveButton.interactable = true;
            }
        }
    }


    public void SetPauseMenu(bool single)
    {
        if (single)
        {
            gameManager = GameObject.Find("SingleGameManager").GetComponent<GameManager>();

            if (GameManager.instance.tutorial)
            {
                surrenderText.text = "チュートリアルを\nスキップしますか？";
                leaveButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "スキップする";
                quitGameButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "スキップする";
                //quitGameButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "チュートリアルを\nスキップする";
            }
        }
        else
        {
            gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        }



    }


    public void OnPauseButton()
    {
        if (gameManager.single)
        {
            Time.timeScale = 0;
        }

        PauseObj.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            cardID[i] = -1;
        }

        cardID = SearchIngredient();



        pause = true;
        time = 0f;

    }


    public void OnBatsuButton()
    {
        PauseObj.SetActive(false);

        pause = false;

        for (int i = 0; i < 9; i++)
        {
            ingredientObj.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;

        }

        if (gameManager.single)
        {
            Time.timeScale = 1;
        }

        quitGameButtonObj.SetActive(true);
        quitGameObj.SetActive(false);

        quitTime = 0f;
        canQuit = false;
        leaveButton.interactable = false;

    }


    public void OnQuitGameButton()
    {

        quitGameButtonObj.SetActive(false);
        quitGameObj.SetActive(true);

        quitTime = 0f;
        canQuit = true;
    }

    public void OnLeaveButton()
    {
        if (gameManager.single)
        {
            Time.timeScale = 1;

            if (GameManager.instance.tutorial)
            {
                Time.timeScale = 1;
                Debug.Log("チュートリアルスキップ");
                PlayerPrefs.SetInt("TUTORIAL", 1);
                PlayerPrefs.Save();
                //FadeManager.FadeOut(1);

                SoundManager.instance.isTutorial = false;

                //return;
            }

        }

        //gameManager.SurrenderGame();
        gameManager.ReturnMenu();

    }

    public void OnCancelButton()
    {
        quitGameButtonObj.SetActive(true);
        quitGameObj.SetActive(false);

        quitTime = 0f;
        canQuit = false;
        leaveButton.interactable = false;
    }

    int[] SearchIngredient()
    {
        int[] cardID = new int[4] { -1, -1, -1, -1 };

        for (int i = 0; i < 4; i++)
        {
            if (handTransform[i].childCount != 0)
            {
                cardID[i] = handTransform[i].GetChild(0).GetComponent<CardController>().model.cardID;

        
            }
        }

        return cardID;
    }


}
