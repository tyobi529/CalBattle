using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
//using Kayac;

using UnityEditor;
using UnityEngine.UI;

public class FooterButton : MonoBehaviour
{
    [SerializeField] int pageNum;

    [SerializeField] Transform canvasTransform;

    [SerializeField] Transform contentTransform;
    //[SerializeField] DefaultDebugTapper tapper;

    [SerializeField] SimpleScrollSnap scrollSnap;

    [SerializeField] Image footerImage = null;
    [SerializeField] Image footerTextImage = null;

    [SerializeField] Sprite[] footerImageSprite = new Sprite[2];
    [SerializeField] Sprite[] footerTextSprite = new Sprite[2];


    public void OnFooterButton()
    {

        if (scrollSnap.CurrentPanel != pageNum)
        {
            scrollSnap.GoToPanel(pageNum);
        }

    }

    public void ChangeFooterTextImage(int num)
    {
        footerImage.sprite = footerImageSprite[num];
        footerTextImage.sprite = footerTextSprite[num];
    }
}