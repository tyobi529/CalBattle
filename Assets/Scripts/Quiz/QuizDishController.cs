using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class QuizDishController : MonoBehaviour
{
    [SerializeField] Image iconImage = null;
    [SerializeField] TextMeshProUGUI nameText = null;

    [SerializeField] GameObject batsuObj = null;
    [SerializeField] GameObject maruObj = null;

    bool isAnswer = false;

    //bool canSelected = true;

    QuizManager quizManager = null;

    public void SetQuizDish(int cardID, bool isAnswer)
    {
        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        CardEntity cardEntity = CardData.instance.dishCardEntity[cardID];

        iconImage.sprite = cardEntity.icon;
        nameText.text = cardEntity.name;
        this.isAnswer = isAnswer;

        quizManager = GameObject.Find("QuizManager").GetComponent<QuizManager>();
    }


    public void OnQuizDish()
    {
       
        GetComponent<Button>().interactable = false;

        Vector2 defaultPosition = transform.position;


        if (isAnswer)
        {
            QuizSoundManager.instance.CorrectSE();

            quizManager.CorrectAnswer();

            maruObj.SetActive(true);


            transform.DOLocalMoveY(100f, 0.1f).OnComplete(() =>
            {
                transform.DOLocalMove(defaultPosition, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    //quizManager.CorrectAnswer();
                });

            });
        }
        else
        {
            QuizSoundManager.instance.WrongSE();

            batsuObj.SetActive(true);

            quizManager.UpdateScore(-50);

            transform.DOShakePosition(0.2f, 30f, 100, 90, false, true).OnComplete(() =>
            {
                transform.DOLocalMove(defaultPosition, 0.1f).SetEase(Ease.OutBounce);
            });


        }
        


    }
}
