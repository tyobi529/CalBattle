using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager = null;
    [SerializeField] GameObject tutorialObj = null;
    [SerializeField] Image titleTextImage = null;

    [SerializeField] Image tutorialPanel = null;

    //[SerializeField] GameObject blackObj = null;

    //素材
    [SerializeField] Sprite[] titleSprite = new Sprite[3] { null, null, null };
    [SerializeField] Sprite[] redPanelSprite = new Sprite[3] { null, null, null };
    [SerializeField] Sprite[] yellowPanelSprite = new Sprite[5] { null, null, null, null, null };
    [SerializeField] Sprite[] greenPanelSprite = new Sprite[4] { null, null, null, null };
    [SerializeField] Sprite[] whitePanelSprite = new Sprite[1] { null };

    [SerializeField] GameObject leftButtonObj = null;
    [SerializeField] GameObject rightButtonObj = null;
    [SerializeField] GameObject okButtonObj = null;


    [SerializeField] GameAnimationController gameAnimationController = null;

    int tutorialNum = 0;
    int panelIndex = 0;

    public void ShowTutorial(int num)
    {
        //blackObj.SetActive(true);
        tutorialObj.GetComponent<CanvasGroup>().alpha = 0;
        tutorialObj.SetActive(true);

        tutorialObj.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);


        ChangeTitle(num);
        ChangePanel(num, 0);



    }


    public void ChangeTitle(int num)
    {
        if (num == 3)
        {
            titleTextImage.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            titleTextImage.sprite = titleSprite[num];
            tutorialNum = num;
        }

        

    }

    public void ChangePanel(int num, int index)
    {
        int maxIndex = 0;
        switch (num)
        {
            case 0:
                tutorialPanel.sprite = redPanelSprite[index];
                maxIndex = redPanelSprite.Length - 1;
                break;
            case 1:
                tutorialPanel.sprite = yellowPanelSprite[index];
                maxIndex = yellowPanelSprite.Length - 1;
                break;
            case 2:
                tutorialPanel.sprite = greenPanelSprite[index];
                maxIndex = greenPanelSprite.Length - 1;
                break;
            case 3:
                tutorialPanel.sprite = whitePanelSprite[index];
                maxIndex = 0;
                break;
        }

        tutorialNum = num;
        panelIndex = index;

        if (index == 0)
        {
            leftButtonObj.SetActive(false);
            rightButtonObj.SetActive(true);
            okButtonObj.SetActive(false);
        }
        else if (index == maxIndex)
        {
            leftButtonObj.SetActive(true);
            rightButtonObj.SetActive(false);
            okButtonObj.SetActive(true);
        }
        else
        {
            leftButtonObj.SetActive(true);
            rightButtonObj.SetActive(true);
            okButtonObj.SetActive(false);
        }

        //最後
        if (num == 3)
        {
            leftButtonObj.SetActive(false);
            rightButtonObj.SetActive(false);
            okButtonObj.SetActive(true);
        }
    }


    public void OnRightButton()
    {
        ChangePanel(tutorialNum, panelIndex + 1);
    }

    public void OnLeftButton()
    {
        ChangePanel(tutorialNum, panelIndex - 1);
    }

    public void OnOkButton()
    {
        if (tutorialNum == 3)
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);
            PlayerPrefs.Save();
            FadeManager.FadeOut(1);

            SoundManager.instance.isTutorial = false;
            return;
        }

        gameAnimationController.MoveTurnObj(true, tutorialNum);

        tutorialObj.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        tutorialObj.SetActive(false);

        //gameManager.ChangeTurn();

    }
}
