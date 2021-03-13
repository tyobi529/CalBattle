using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    public CardModel model; //データ（model）関することを操作

    SelectController selectController;


    //public void Init(int cardID, int ID, bool isMix)
    public void Init(bool isDish, int cardID, int cost, int condition)
    {
        model = new CardModel(isDish, cardID, cost, condition);
        selectController = GameObject.Find("SelectController").GetComponent<SelectController>();
    }


    public void OnCardObject()
    {
 

        if (model.isSelected)
        {
            BattleSoundManager.instance.TapCardCancelSE();
        }
        else
        {
            BattleSoundManager.instance.TapCardSE();
        }

        model.isSelected = !model.isSelected;


        selectController.SelectCard(this.GetComponent<CardController>(), model.isSelected);
    }

    public void OnEatCardObject()
    {
        //全てのカードを選択不可に
        for (int i = 0; i < 4; i++)
        {
            GameManager.instance.handTransform[0, i].GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = false;
        }


        BattleSoundManager.instance.TapButtonSE();

        Vector3[] scale = new Vector3[2];
        scale[0] = new Vector3(1.1f, 1.1f, 1.1f);
        scale[1] = new Vector3(1f, 1f, 1f);

        transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() =>
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }).OnComplete(() =>
        {
            GameManager.instance.OnDecideButton();

        });


    }


    public void TitleCardInit(int cardID)
    {
        model = new CardModel(false, cardID, 0, 0);
    }





}
