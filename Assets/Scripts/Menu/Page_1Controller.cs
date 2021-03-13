using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Page_1Controller : MonoBehaviour
{

    bool recipeMode = false;
    bool quizMode = false;
    bool speedMode = false;

    int clearNum = 0;

    [SerializeField] GameObject[] secretObj = new GameObject[3] { null, null, null };

    [SerializeField] Page_2Controller page_2Controller = null;

    [SerializeField] FooterButton footerButton_2 = null;

    private void Start()
    {
        CheckMode();
    }

    public void CheckMode()
    {
        clearNum = 0;

        for (int i = 0; i < 16; i++)
        {
            int num = PlayerPrefs.GetInt("MISSION_" + i, 0);
            clearNum += num;
        }

        Debug.Log("クリア数" + clearNum);

        if (clearNum >= 3)
        {
            recipeMode = true;
            secretObj[0].SetActive(false);
        }

        if (clearNum >= 5)
        {
            speedMode = true;
            secretObj[1].SetActive(false);
        }

        if (clearNum >= 7)
        {
            quizMode = true;
            secretObj[2].SetActive(false);
        }

    }

    public void OnRecipeModeButton()
    {
        if (recipeMode)
        {
            FadeManager.FadeOut(3);
        }
        else
        {
            footerButton_2.OnFooterButton();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                page_2Controller.ShowMission();
            });
        }
        
    }


    public void OnSpeedButton()
    {
        if (speedMode)
        {
            FadeManager.FadeOut(4);
        }
        else
        {
            footerButton_2.OnFooterButton();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                page_2Controller.ShowMission();
            });
        }

    }

    public void OnQuizModeButton()
    {
        if (quizMode)
        {
            FadeManager.FadeOut(5);
        }
        else
        {
            footerButton_2.OnFooterButton();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                page_2Controller.ShowMission();
            });

        }

    }


}
