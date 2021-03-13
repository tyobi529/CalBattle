using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameAnimationController : MonoBehaviour
{
    [SerializeField] CharacterController characterController = null;
    [SerializeField] UIManager uiManager;




    //ゲーム開始時
    [SerializeField] Image blackImage = null;

    [SerializeField] GameObject[] vsObj = new GameObject[2];

    [SerializeField] GameObject[] vsBGObj = new GameObject[2];
    Vector2[] vsBGObjPosition = new Vector2[2];

    //背景を配置する場所
    [SerializeField] Transform[] cookingFieldTransform = new Transform[2];

    [SerializeField] GameObject vsTextObj;



    [SerializeField] GameObject[] fireEffectObj = new GameObject[2];
    [SerializeField] GameObject thunderEffectObj;


    [SerializeField] float fadeTime;



    [SerializeField] GameObject startEffectPrefab = null;
    [SerializeField] GameObject cookStartTextObj = null;


    //結果
    [SerializeField] GameObject finishGameBG;
    [SerializeField] GameObject[] finishGameText = new GameObject[3];



    [SerializeField] Transform kitchenTransform = null;
    [SerializeField] GameObject winEffectPrefab = null;

    [SerializeField] Setting setting = null;




    [SerializeField] GameObject timer = null;
    [SerializeField] Image timerImage = null;
    [SerializeField] Transform timerHariTransform = null;
    Vector2 timerPosition;

    float maxTime = 0f;

    [SerializeField] TextMeshProUGUI turnText = null;

    public GameObject rainEffect = null;

    //チュートリアル
    public GameObject tutorialTextObj = null;
    [SerializeField] Sprite[] tutorialSprite = new Sprite[3];


    // Start is called before the first frame update
    void Start()
    {
        timerPosition = timer.transform.localPosition;

        //timerPosition[0] = timer[0].transform.localPosition;
        //timerPosition[1] = new Vector2(0f, -275f);

        maxTime = setting.timeLimit;


        DOTween.ToAlpha(
() => blackImage.color,
color => blackImage.color = color,
180f / 255f, // 目標値
1f// 所要時間
);
        



    }



    public void MoveVsBG()
    {
        if (setting.cutStartAnimation)
        {
            vsObj[0].SetActive(false);
            characterController.ShowCharacter(true);
            characterController.ChangeIdleSprite();
            GameManager.instance.PrepareGame();
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            //vsBGObj[i].transform.position = vsBGObjPosition[i];
            vsBGObj[i].SetActive(true);
            fireEffectObj[i].SetActive(true);
        }
        

        DOVirtual.DelayedCall(1, () =>
        {

            vsBGObj[0].transform.DOMove(cookingFieldTransform[0].position, 0.3f).OnComplete(() => {
                fireEffectObj[0].transform.localPosition = new Vector2(0f, fireEffectObj[0].transform.localPosition.y);
            });
            vsBGObj[1].transform.DOMove(cookingFieldTransform[1].position, 0.3f).OnComplete(() => {
                fireEffectObj[1].transform.localPosition = new Vector2(0f, fireEffectObj[1].transform.localPosition.y);
                ScaleVsText();

                //SE
                BattleSoundManager.instance.StartSE();
                BattleSoundManager.instance.FireSE();


            });
        });

  

    }

    void ScaleVsText()
    {



        thunderEffectObj.SetActive(true);

        vsObj[0].SetActive(true);
        vsObj[1].SetActive(true);

        //characterObj.SetActive(true);

        vsTextObj.SetActive(true);
        vsTextObj.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 3f).OnComplete(() => {




            characterController.ShowCharacter(true);
            characterController.ChangeIdleSprite();
            

            vsTextObj.transform.DOScale(new Vector3(7f, 7f, 7f), 0.5f).OnComplete(() => {
                for (int i = 0; i < 2; i++)
                {
                    vsObj[i].SetActive(false);
                    fireEffectObj[i].SetActive(false);
                }


                //SE消す
                BattleSoundManager.instance.StopSE();


                DOTween.ToAlpha(
() => vsTextObj.GetComponent<Image>().color,
color => vsTextObj.GetComponent<Image>().color = color,
0f, // 目標値
1f// 所要時間
).OnComplete(() => {
    vsTextObj.SetActive(false);
    GameManager.instance.PrepareGame();
});

                
            });
        });
    }


    public void MoveTurnObj(bool tutorial, int tutorialNum)
    {


        if (tutorial)
        {

            timerImage.fillAmount = 0f;
            timerHariTransform.rotation = Quaternion.Euler(0, 0, 0);

            timer.SetActive(true);
            timer.transform.DOScale(new Vector3(4f, 4f, 4f), 0.5f);


            timer.transform.DOLocalMove(new Vector2(0f, 0f), 0.5f);


            //チュートリアル
            tutorialTextObj.GetComponent<Image>().sprite = tutorialSprite[tutorialNum];
            tutorialTextObj.transform.localScale = new Vector3(0f, 0f, 0f);
            tutorialTextObj.transform.position = new Vector3(0f, 0f, 0f);

            tutorialTextObj.gameObject.SetActive(true);


            tutorialTextObj.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(() =>
            {
                BattleSoundManager.instance.TurnSE();


                Instantiate(startEffectPrefab);

                //はりまわす
                timerHariTransform.DOLocalRotate(new Vector3(0, 0, -360f), 0.7f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

                DOTween.To(
                () => 0f,
                x =>
                {
                    timerImage.fillAmount = x;
                },
                1f,
                0.7f
                ).SetEase(Ease.Linear);

                tutorialTextObj.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1f).OnComplete(() =>
                {
                    timer.transform.DOScale(1f, 0.5f);
                    timer.transform.DOLocalMove(timerPosition, 0.5f);

                    //チュートリアル
                    tutorialTextObj.transform.DOScale(1f, 0.5f);
                    tutorialTextObj.transform.DOLocalMove(timerPosition, 0.5f).OnComplete(() =>
                    {
                        GameManager.instance.ChangeTurn();
                    });

                });



            });
        }
        else
        {
            //準備
            cookStartTextObj.SetActive(true);
            cookStartTextObj.GetComponent<CanvasGroup>().alpha = 1f;
            turnText.text = "ターン " + GameManager.instance.turnCount;
            turnText.color = new Color(0f, 0f, 0f, 1f);


            cookStartTextObj.transform.localScale = new Vector3(0f, 0f, 0f);



            timerImage.fillAmount = 0f;
            timerHariTransform.rotation = Quaternion.Euler(0, 0, 0);

            timer.SetActive(true);
            timer.transform.DOScale(new Vector3(4f, 4f, 4f), 0.5f);


            timer.transform.DOLocalMove(new Vector2(0f, 0f), 0.5f);


            cookStartTextObj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).OnComplete(() =>
            {
                BattleSoundManager.instance.TurnSE();

                Instantiate(startEffectPrefab);




                //はりまわす
                timerHariTransform.DOLocalRotate(new Vector3(0, 0, -360f), 0.7f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

                DOTween.To(
                () => 0f,
                x =>
                {
                    timerImage.fillAmount = x;
                },
                1f,
                0.7f
                ).SetEase(Ease.Linear).OnComplete(() => {

                    //不器用状態
                    float timeLimit = GameManager.instance.timeLimit;
                    if (timeLimit != maxTime)
                    {                        
                        float valueTo = timeLimit / maxTime;
                        float angleTo = 360f * valueTo;

                        DOTween.To(
                        () => 1f,
                        x =>
                        {
                        timerImage.fillAmount = x;
                        },
                        valueTo,
                        0.3f
                        ).SetEase(Ease.Linear);


                        timerHariTransform.rotation = Quaternion.Euler(0, 0, 0);
                        //timerHariTransform.DOLocalRotate(new Vector3(0, 0, angleTo), 0.3f).SetEase(Ease.Linear);
                        timerHariTransform.DOLocalRotate(new Vector3(0, 0, angleTo), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
                    }


                });




                cookStartTextObj.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f).OnComplete(() =>
                {



                    timer.transform.DOScale(1f, 0.5f);
                    timer.transform.DOLocalMove(timerPosition, 0.5f);


                    cookStartTextObj.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
                    {
                        cookStartTextObj.SetActive(false);
                        GameManager.instance.ChangeTurn();
                    });

                });



            });
        }


    }
               




    

    //0:勝ち
    //1:負け
    public void ShowResultObj(bool win)
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }

        finishGameBG.SetActive(true);

        int resultNum = -1;
        if (win)
        {
            resultNum = 1;
        }
        else
        {
            resultNum = 2;
        }

        finishGameText[0].SetActive(true);
        finishGameText[0].transform.localScale = new Vector3(0f, 0f, 0f);
        finishGameText[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);


        DOTween.Sequence().Append(finishGameText[0].transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f))
    .OnComplete(() => {

        DOTween.ToAlpha(
        () => finishGameText[0].GetComponent<Image>().color,
        color => finishGameText[0].GetComponent<Image>().color = color,
        0f, // 目標値
        1f// 所要時間
        ).OnComplete(() =>
        {

            ////サウンド
            BattleSoundManager.instance.ResultSE(resultNum);

            finishGameText[resultNum].SetActive(true);
            finishGameText[resultNum].transform.localScale = new Vector3(0f, 0f, 0f);

            DOTween.Sequence().Append(finishGameText[resultNum].transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f))
                .Append(finishGameText[resultNum].transform.DOScale(new Vector3(1f, 1f, 1f), 3f)).
                Append(finishGameText[resultNum].transform.DOScale(new Vector3(2f, 0f, 1f), 0.2f)).OnComplete(() =>
                {        


                    //resultObj.SetActive(false);
                    DOVirtual.DelayedCall(1, () =>
                    {
                        finishGameBG.SetActive(false);
                        finishGameText[resultNum].SetActive(false);
                        uiManager.ShowResult(win);

                        if (GameManager.instance.surrender)
                        {
                            GameManager.instance.LeaveGame();
                        }

                        if (win)
                        {
                            StartCoroutine(WinEffect());
                        }
                        else
                        {
                            rainEffect.SetActive(true);
                        }

                        //BGM戻る                        
                        //SoundManager.instance.source.mute = false;
                        SoundManager.instance.StartBGM("Result");


                    });

                }).SetDelay(1);


        }).SetDelay(1);

}).SetDelay(2);



    }

    IEnumerator WinEffect()
    {
        float width = Screen.width;
        float height = kitchenTransform.GetComponent<RectTransform>().sizeDelta.y;



        while (true)
        {
            float x = Random.Range(-width / 2f, width / 2f);
            float y = Random.Range(-height / 2f, height / 2f);
            GameObject winEffect = GameObject.Instantiate(winEffectPrefab, kitchenTransform);
            winEffect.transform.localPosition = new Vector3(x, y, -12000f);

            yield return new WaitForSeconds(1);
        }
    }



    public void OnRestartButton()
    {
        GameManager.instance.OnRestartGameButton();
    }

    public void OnLeaveButton()
    {
        GameManager.instance.OnLeaveButton();
        
    }


    public void CompleteTween()
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }
    }
}
