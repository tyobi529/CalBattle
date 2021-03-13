using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] FooterButton[] footerButton = new FooterButton[4];

    [SerializeField] SimpleScrollSnap scrollSnap;

    [SerializeField] Image[] BGImage = new Image[2];

    [SerializeField] Color[] bgColor = new Color[4];


    [SerializeField] TextMeshProUGUI nameText;



    private void Start()
    {

        ChangePanel();


        nameText.text = PlayerPrefs.GetString("NAME", "名前なし");
        

    }

    public void ChangePanel()
    {
        int num = scrollSnap.CurrentPanel;

        

        ChangeFooter(num);
        ChangeBGColor(num);
    }

    void ChangeFooter(int num)
    {
        for (int i = 0; i < 4; i++)
        {
   
            if (i == num)
            {
                footerButton[num].ChangeFooterTextImage(1);
            }
            else
            {
                footerButton[i].ChangeFooterTextImage(0);
            }
            
        }


    }

    void ChangeBGColor(int num)
    {
        for (int i = 0; i < 2; i++)
        {
            BGImage[i].DOColor(bgColor[num], 1f);

        }

    }





}
