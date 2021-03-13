using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SpeedGameManager : MonoBehaviour
{
    //[SerializeField] GameObject correctEffect = null;

    //[SerializeField] RectTransform parentRectTransform = null;

    [SerializeField] SpeedCardGenerator speedCardGenerator = null;

    [SerializeField] Transform canvasTransform = null;


    [SerializeField] GameObject correctPrefab = null;
    [SerializeField] Sprite[] correctSprite = new Sprite[3];

    [SerializeField] Image timeImage = null;
    public int limitTime = 0;


    int noCorrectTime = 0;

    public SpeedCardController[] speedCardController { get; set; } = new SpeedCardController[4] { null, null, null, null };
    public SpeedCardController[] speedDishController { get; set; } = new SpeedCardController[3] { null, null, null };


    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] TextMeshProUGUI[] addTimeText = new TextMeshProUGUI[3] { null, null, null };

    bool showHint = false;
    public bool isWrong { get; set; } = false;


    bool isDishMove = false;


    [SerializeField] Transform[] titleCardLeft = new Transform[4];
    [SerializeField] Transform[] titleCardRight = new Transform[4];

    [SerializeField] GameObject leftObj = null;
    [SerializeField] GameObject rightObj = null;



    Vector2[] defaultLeftPosition = new Vector2[4];
    Vector2[] defaultRightPosition = new Vector2[4];

    Vector2[] movePosition = new Vector2[2];


    [SerializeField] float moveSpeed = 0f;


    [SerializeField] GameObject titleObj = null;
    [SerializeField] GameObject gameObj = null;


    [SerializeField] Image startCountTextImage = null;
    [SerializeField] GameObject startObj = null;
    [SerializeField] GameObject endObj = null;
    [SerializeField] Sprite[] numberSprite = new Sprite[3];


    //結果
    [SerializeField] GameObject resultObj = null;
    [SerializeField] GameObject sinasuObj = null;
    [SerializeField] TextMeshProUGUI madeNumberText = null;
    [SerializeField] GameObject rankTextObj = null;
    [SerializeField] GameObject rankObj = null;

    [SerializeField] GameObject returnButtonObj = null;

    [SerializeField] Sprite[] rankSprite = new Sprite[4];

    public int madeNum { get; set; } = 0;

    int timeCount = 0;


    //ボタン
    [SerializeField] GameObject startButtonObj = null;
    [SerializeField] GameObject quitButtonObj = null;


    //ランキング
    [SerializeField] GameObject rankingButtonObj = null;
    [SerializeField] GameObject rankingObj = null;
    [SerializeField] TextMeshProUGUI[] rankMadeNumText = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] rankText = new TextMeshProUGUI[3];


    //ポーズ関連
    [SerializeField] GameObject pauseButtonObj = null;
    [SerializeField] GameObject pauseObj = null;
    [SerializeField] TextMeshProUGUI pauseText = null;
    

    bool pause = false;

    float time = 0f;


    //チュートリアル
    //チュートリアル
    [SerializeField] GameObject tutorialObj = null;
    [SerializeField] Image tutorialImage = null;
    [SerializeField] Sprite[] tutorialSprite = new Sprite[3] { null, null, null };

    [SerializeField] GameObject rightButtonObj = null;
    [SerializeField] GameObject leftButtonObj = null;
    [SerializeField] GameObject okButtonObj = null;

    int panelIndex = 0;


    private void Start()
    {

        //StartCoroutine(MoveTitle());

        timeCount = limitTime;

        speedCardGenerator.SetTitleCard();



        movePosition[0] = titleCardLeft[3].localPosition;
        movePosition[1] = titleCardRight[3].localPosition;

        for (int i = 0; i < 4; i++)
        {
            defaultLeftPosition[i] = titleCardLeft[i].localPosition;
            defaultRightPosition[i] = titleCardRight[i].localPosition;
        }

        isDishMove = true;

    }


   

    void PrepareGame()
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }

        StopAllCoroutines();

        pauseButtonObj.SetActive(true);
        CleanHandCard();
        CleanFieldCard();

        limitTime = timeCount;

        madeNum = 0;
        noCorrectTime = 0;

        MoveToGame();
    }

    private void Update()
    {
        if (isDishMove)
        {
            for (int i = 0; i < 4; i++)
            {
                titleCardLeft[i].Translate(new Vector2(0f, -moveSpeed));
                titleCardRight[i].Translate(new Vector2(0f, -moveSpeed));

                if (titleCardLeft[i].localPosition.y < -750f)
                {    
                    titleCardLeft[i].localPosition = movePosition[0];
                }

                if (titleCardRight[i].localPosition.y < -750f)
                {
                    titleCardRight[i].localPosition = movePosition[1];
                }

            }


        }

        if (pause)
        {
            time += 1f / 60f;

            float sin = Mathf.Sin((time * 7f) / 2f + 0.5f);

            pauseText.color = new Color(1f, 1f, 1f, sin);
        }
    }


    void StartGame()
    {
        StopAllCoroutines();

        StartCoroutine(CountDown());

        SoundManager.instance.source.Play();


        for (int i = 0; i < 4; i++)
        {
            speedCardGenerator.GiveCardToHand(i, false);
        }

        for (int i = 0; i < 3; i++)
        {
            speedCardGenerator.GiveCardToField(i);
        }
    }


    public void CorrectEffect(Vector2 position)
    {
        int spriteNum = -1;


        if (showHint)
        {
            for (int i = 0; i < 4; i++)
            {
                if (speedCardController[i] != null)
                {
                    speedCardController[i].GetComponent<SpeedCardView>().StopBlink();
                }
            }

            showHint = false;
        }

        if (noCorrectTime < 2 && !isWrong)
        {
            spriteNum = 2;
            limitTime += 3;

            UpdateTime(3);

            SpeedSoundManager.instance.KiraSE(1);
        }
        else if (noCorrectTime < 3 && !isWrong)
        {
            spriteNum = 1;
            limitTime += 2;

            UpdateTime(2);

            SpeedSoundManager.instance.KiraSE(0);
        }
        else
        {
            spriteNum = 0;
            limitTime += 1;

            UpdateTime(1);

            SpeedSoundManager.instance.KiraSE(0);
        }

        isWrong = false;


        noCorrectTime = 0;


        GameObject correct = Instantiate(correctPrefab, canvasTransform);

        correct.GetComponent<Image>().sprite = correctSprite[spriteNum];

        correct.transform.position = position;

        correct.transform.DOScale(1.5f, 0.7f);


        DOTween.ToAlpha(
() => correct.GetComponent<Image>().color,
color => correct.GetComponent<Image>().color = color,
0f, // 目標値
0.4f// 所要時間
).SetDelay(0.3f).OnComplete(() =>
{
    Destroy(correct);
});


    }


    IEnumerator CountDown()
    {

        while (limitTime > 0)
        {
            yield return new WaitForSeconds(1);


            limitTime -= 1;

            //timerBar.valueCurrent = limitTime;
            timeImage.fillAmount = (float)limitTime / (float)30f;


            UpdateTime(0);

            //Debug.Log(limitTime);

            noCorrectTime++;

            if (noCorrectTime >= 6 && !showHint)
            {
                ShowHint();
                showHint = true;
            }
        }


        //yield return new WaitForSeconds(1);

        UpdateTime(0);
        //timerBar.valueCurrent = limitTime;


        ShowResult();


    }

    IEnumerator StartCountDown()
    {
        int count = 3;

        startCountTextImage.gameObject.SetActive(true);

        while (count > 0)
        {
            count--;

            startCountTextImage.sprite = numberSprite[count];

            yield return new WaitForSeconds(1);
        }

        startCountTextImage.gameObject.SetActive(false);




        startObj.SetActive(true);

        DOTween.ToAlpha(
() => startObj.GetComponent<Image>().color,
color => startObj.GetComponent<Image>().color = color,
0f, // 目標値
0.5f// 所要時間
).SetDelay(0.5f).OnComplete(() =>
{
    startObj.SetActive(false);
    startObj.GetComponent<Image>().color = Color.white;

});

        StartGame();


    }


    void ShowResult()
    {
        //StopAllCoroutines();

        for (int i = 0; i < 4; i++)
        {
            if (speedCardController[i] != null)
            {
                //speedCardController[i].GetComponent<SpeedCardMovement>().OnEndDrag(null);
                speedCardController[i].GetComponent<CanvasGroup>().blocksRaycasts = false;                

            }
        }

        endObj.SetActive(true);
        endObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);


        //テスト
        //madeNum = Random.Range(0, 25);

        //結果の記入
        madeNumberText.text = madeNum.ToString();
        if (madeNum < 5)
        {
            rankObj.GetComponent<Image>().sprite = rankSprite[0];
        }
        else if (madeNum < 10)
        {
            rankObj.GetComponent<Image>().sprite = rankSprite[1];
        }
        else if (madeNum < 15)
        {
            rankObj.GetComponent<Image>().sprite = rankSprite[2];
        }
        else
        {
            rankObj.GetComponent<Image>().sprite = rankSprite[3];
        }

        //結果のセーブ
        SaveRanking(madeNum);

        sinasuObj.SetActive(false);
        rankObj.SetActive(false);
        rankTextObj.SetActive(false);


        endObj.transform.DOScale(1f, 1f).OnComplete(() =>
        {

            resultObj.SetActive(true);

            DOVirtual.DelayedCall(2f, () =>
            {
                DOTween.ToAlpha(
() => endObj.GetComponent<Image>().color,
color => endObj.GetComponent<Image>().color = color,
0f, // 目標値
0.4f// 所要時間
).SetDelay(0.5f).OnComplete(() => {

    endObj.SetActive(false);
    endObj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
});




                MoveToTitle();

                ShowRank();

            });
        });

  



    }


    private void UpdateTime(int addTime)
    {
        if (limitTime > 30)
        {
            limitTime = 30;
        }

        timeText.text = limitTime.ToString();

        if (addTime > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!addTimeText[i].gameObject.activeSelf)
                {
                    addTimeText[i].gameObject.SetActive(true);

                    addTimeText[i].text = "+" + addTime.ToString();


                    DOVirtual.DelayedCall(1, () =>
                    {
                        addTimeText[i].gameObject.SetActive(false);
                    });

                    return;
                }
       
            }

        }
    }


    void ShowHint()
    {
        bool[] haveCorrect = new bool[4] { false, false, false, false };



        for (int i = 0; i < 4; i++)
        {
            if (speedCardController[i] == null)
            {
                return;
            }

        }

        for (int i = 0; i < 3; i++)
        {
            if (speedDishController[i] == null)
            {
                return;
            }
        }


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (speedCardController[i].model.cardID == speedDishController[j].model.ingredientCardID[0] || speedCardController[i].model.cardID == speedDishController[j].model.ingredientCardID[1])
                {
                    haveCorrect[i] = true;
                    break;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (haveCorrect[i])
            {
                speedCardController[i].GetComponent<SpeedCardView>().StartBlink();
            }
        }
    }


    //詰んでいないかチェック
    public void CheckCard()
    {

        for (int i = 0; i < 4; i++)
        {
            if (speedCardController[i] == null)
            {
                return;
            }

        }

        for (int i = 0; i < 3; i++)
        {
            if (speedDishController[i] == null)
            {
                return;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (speedCardController[i].model.cardID == speedDishController[j].model.ingredientCardID[0] || speedCardController[i].model.cardID == speedDishController[j].model.ingredientCardID[1])
                {
                    //詰みなし
                    return;
                }
            }
        }

 
        Debug.Log("詰みあり");

        //詰みあり
        //手札カードの入れ替え
        for (int i = 0; i < 4; i++)
        {
            speedCardController[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        //StopAllCoroutines();

        ResetHand();


    }

    void ResetHand()
    {
        Debug.Log("入れ替え");

        CleanHandCard();
        for (int i = 0; i < 4; i++)
        {
            speedCardGenerator.GiveCardToHand(i, false);
        }

        CheckCard();
    }


    void CleanHandCard()
    {
        for (int i = 0; i < 4; i++)
        {
            if (speedCardController[i] != null)
            {
                Destroy(speedCardController[i].gameObject);
            }
            
            
        }
    }

    void CleanFieldCard()
    {
        for (int i = 0; i < 3; i++)
        {
            if (speedDishController[i] != null)
            {
                Destroy(speedDishController[i].gameObject);
            }


        }
    }




    public void OnStartButton()
    {
        startButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            startButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });


        PrepareGame();

        

    }

    public void OnQuitButton()
    {
        quitButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            quitButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });



        FadeManager.FadeOut(1);

    }

    public void OnReturnButton()
    {
        returnButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            returnButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });

        titleObj.SetActive(true);

        resultObj.transform.DOScale(0f, 0.3f).OnComplete(() => {
            resultObj.SetActive(false);
            resultObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });


    }


    public void OnTutorialButton()
    {
        tutorialObj.GetComponent<CanvasGroup>().alpha = 0;
        tutorialObj.SetActive(true);
        ChangePanel(0);

        tutorialObj.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
    }

    void ChangePanel(int index)
    {
        int maxIndex = 2;


        tutorialImage.sprite = tutorialSprite[index];

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

        panelIndex = index;
    }


    public void OnRightButton()
    {
        ChangePanel(panelIndex + 1);
    }

    public void OnLeftButton()
    {
        ChangePanel(panelIndex - 1);
    }

    public void OnOkButton()
    {
        tutorialObj.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        tutorialObj.SetActive(false);
    }



    public void OnRankingButton()
    {
        rankingButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            rankingButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });

        SetRanking();

        rankingObj.transform.localScale = new Vector3(0f, 0f, 0f);

        rankingObj.SetActive(true);

        rankingObj.transform.DOScale(1f, 0.3f).OnComplete(() => {

   
        });


    }

    public void OnRankingRetrnButton()
    {
        rankingButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            rankingButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });

        //titleObj.SetActive(true);

        rankingObj.transform.DOScale(0f, 0.3f).OnComplete(() => {
            rankingObj.SetActive(false);
            rankingObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });


    }


    void SetRanking()
    {
        int[] madeNum = new int[3] { -1, -1, -1 };

        for (int i = 0; i < 3; i++)
        {
            madeNum[i] = PlayerPrefs.GetInt("SPEEDMADENUM_" + i, 0);

            rankMadeNumText[i].text = madeNum[i].ToString();

            if (madeNum[i] < 5)
            {
                rankText[i].text = "C";
            }
            else if (madeNum[i] < 10)
            {
                rankText[i].text = "B";
            }
            else if (madeNum[i] < 15)
            {
                rankText[i].text = "A";
            }
            else
            {
                rankText[i].text = "S";
            }

        }



    }

    void SaveRanking(int num)
    {
        int[] madeNum = new int[3] { -1, -1, -1 };

        for (int i = 0; i < 3; i++)
        {
            madeNum[i] = PlayerPrefs.GetInt("SPEEDMADENUM_" + i, 0);

            Debug.Log(madeNum[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            if (num > madeNum[i])
            {

                PlayerPrefs.SetInt("SPEEDMADENUM_" + i, num);


                for (int j = 2; j > i; j--)
                {
                    PlayerPrefs.SetInt("SPEEDMADENUM_" + j, madeNum[j - 1]);
                    madeNum[j] = madeNum[j - 1];
                }

                madeNum[i] = num;
                break;
            }
        }

        PlayerPrefs.Save();

    }


    void ShowRank()
    {
        returnButtonObj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        returnButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0f);

        DOVirtual.DelayedCall(1.5f, () =>
        {
            sinasuObj.SetActive(true);

            DOVirtual.DelayedCall(1f, () =>
            {
                rankTextObj.SetActive(true);

                DOVirtual.DelayedCall(1f, () =>
                {
                    rankObj.SetActive(true);

                    DOTween.ToAlpha(
() => returnButtonObj.GetComponent<Image>().color,
color => returnButtonObj.GetComponent<Image>().color = color,
1f, // 目標値
0.4f// 所要時間
).SetDelay(0.5f);

                    DOTween.ToAlpha(
() => returnButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color,
color => returnButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color,
1f, // 目標値
0.4f// 所要時間
).SetDelay(0.5f);

                });
            });
        });
    }

    void MoveToGame()
    {
        StopAllCoroutines();
        isDishMove = false;

        gameObj.SetActive(true);

        Vector2 defaultLeftPosition = leftObj.transform.localPosition;
        Vector2 defaultRightPosition = rightObj.transform.localPosition;


        leftObj.transform.DOMoveX(-10f, 0.5f).OnComplete(() =>
        {
            leftObj.SetActive(false);
            leftObj.transform.localPosition = defaultLeftPosition;
        });


        rightObj.transform.DOMoveX(10f, 0.5f).OnComplete(() =>
        {
            rightObj.SetActive(false);
            rightObj.transform.localPosition = defaultRightPosition;

        });

        SoundManager.instance.source.Stop();


        DOVirtual.DelayedCall(0.5f, () =>
        {
            titleObj.transform.DOLocalMoveX(-1334f, 0.5f);
            gameObj.transform.DOMove(new Vector3(0f, 0f, 0f), 0.5f).OnComplete(() =>
            {
                titleObj.SetActive(false);
                StartCoroutine(StartCountDown());

            }); ;
        });


    }



    void MoveToTitle()
    {

        isDishMove = true;

        titleObj.SetActive(true);


  

        leftObj.SetActive(true);
        rightObj.SetActive(true);


        DOVirtual.DelayedCall(0.5f, () =>
        {
            gameObj.transform.DOLocalMoveX(1334f, 0.5f);
            titleObj.transform.DOMove(new Vector3(0f, 0f, 0f), 0.5f).OnComplete(() =>
            {
                //StartCoroutine(StartCountDown());

            }); ;
        });
    }



    //ポーズ関連
    public void OnPauseButton()
    {
        pauseObj.SetActive(true);

        pauseButtonObj.SetActive(false);

        pause = true;
        time = 0f;

        Time.timeScale = 0;
    }

    public void OnBatsuButton()
    {
        pauseObj.SetActive(false);

        pauseButtonObj.SetActive(true);

        pause = false;

        Time.timeScale = 1;
    }


    public void OnRestartButton()
    {
        pauseObj.SetActive(false);
        PrepareGame();

        pause = false;

        Time.timeScale = 1;
    }

    public void OnLeaveButton()
    {
        FadeManager.FadeOut(1);

        pause = false;

        Time.timeScale = 1;
    }


}
