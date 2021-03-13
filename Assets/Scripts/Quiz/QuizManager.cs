using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Coffee.UIEffects;
using UnityEngine.UI;
using DG.Tweening;

public class QuizManager : MonoBehaviour
{
    [SerializeField] DishInformation dishInformation = null;
    [SerializeField] GameObject quizDishPrefab = null;

    [SerializeField] Transform[] answerFieldTransform = new Transform[4];


    [SerializeField] TextMeshProUGUI quizText;
    [SerializeField] float messageSpeed;


    int[] generateCardID = new int[4] { -1, -1, -1, -1 };

    [SerializeField] UIShiny uIShiny;

    public static QuizManager instance;

    int questionNum = 0;

    int mode = -1;
    int questionSumNum = 10;

    string quizMessage = null;

    //int time = 0;
    [SerializeField] int limitTime = 10;
    [SerializeField] Image countDownImage = null;

    List<int> dishList = new List<int>();


    int score = 0;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI addScoreText = null;

    [SerializeField] Color plusColor;
    [SerializeField] Color minusColor;

    [SerializeField] GameObject popUpObj = null;

    [SerializeField] GameObject popUpTitle = null;
    [SerializeField] GameObject popUpResult = null;


    [SerializeField] TextMeshProUGUI rankingText = null;
    [SerializeField] TextMeshProUGUI[] rankingScoreText = new TextMeshProUGUI[5] { null, null, null, null, null };
    [SerializeField] TextMeshProUGUI currentScoreText = null;

    [SerializeField] GameObject currentScoreObj = null;

    [SerializeField] GameObject whiteObj = null;


    [SerializeField] GameObject changeButtonObj = null;

    int rankingMode = 1;

    [SerializeField] bool noQuiz = false;


    [SerializeField] RectTransform[] dishTransform = new RectTransform[4];
    Vector2[] dishPosition = new Vector2[4];
    Vector2[] movePosition = new Vector2[2];


    bool isDishMove = true;
    [SerializeField] float moveSpeed = 0f;

    Vector2 addDefaultPosition;


    //ポーズ関連
    [SerializeField] GameObject pauseButtonObj = null;
    [SerializeField] GameObject pauseObj = null;
    [SerializeField] TextMeshProUGUI pauseText = null;

    bool pause = false;

    float time = 0f;


    //private void Awake()
    //{
    //    FadeManager.FadeIn();

    //}



    private void Start()
    {
        pauseButtonObj.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            dishPosition[i] = dishTransform[i].localPosition;
        }

        movePosition[0] = dishPosition[1];
        movePosition[1] = dishPosition[3];

        //背景の料理のicon
        for (int i = 0; i < 20; i++)
        {
            //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + i);
            CardEntity cardEntity = CardData.instance.dishCardEntity[i];

            if (i < 5)
            {
                int index = i % 5;
                dishTransform[0].GetChild(index).GetComponent<Image>().sprite = cardEntity.icon;
            }
            else if (i < 10)
            {
                int index = i % 5;
                dishTransform[1].GetChild(index).GetComponent<Image>().sprite = cardEntity.icon;
            }
            else if (i < 15)
            {
                int index = i % 5;
                dishTransform[2].GetChild(index).GetComponent<Image>().sprite = cardEntity.icon;
            }
            else
            {
                int index = i % 5;
                dishTransform[3].GetChild(index).GetComponent<Image>().sprite = cardEntity.icon;
            }
        }


        addDefaultPosition = addScoreText.transform.localPosition;
    }


    private void Update()
    {
        if (isDishMove)
        {
            for (int i = 0; i < 4; i++)
            {
                //左向き
                if (dishTransform[i].position.y > 0f)
                {
                    dishTransform[i].Translate(new Vector2(-moveSpeed, 0f));
                    if (dishTransform[i].localPosition.x < -750f)
                    {
                        dishTransform[i].localPosition = movePosition[1];
                    }
                }
                else
                {
                    dishTransform[i].Translate(new Vector2(moveSpeed, 0f));
                    if (dishTransform[i].localPosition.x > 750f)
                    {
                        dishTransform[i].localPosition = movePosition[0];
                    }
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


    void SetDishView(bool isShow)
    {
  
        for (int i = 0; i < 4; i++)
        {
            dishTransform[i].gameObject.SetActive(isShow);
            dishTransform[i].localPosition = dishPosition[i];

        }
        
    }



    void PrepareQuiz()
    {
        SetDishView(false);
        isDishMove = false;

        StopAllCoroutines();

        CleanField();

        questionNum = 0;
        score = 0;
        scoreText.text = 0.ToString();
        dishList.Clear();

        for (int i = 0; i < 27; i++)
        {
            dishList.Add(i);
        }


        StartQuiz();
        
    }

    void StartQuiz()
    {
        pauseButtonObj.SetActive(true);

        quizText.alignment = TextAlignmentOptions.Midline;

        string message = "";

        if (mode == 1)
        {
            questionSumNum = 10;
            message = "おてがるモードで開始します";
        }
        else
        {
            questionSumNum = 27;
            message = "じっくりモードで開始します";
        }

        uIShiny.Play();
        StartCoroutine(QuizMessage(message));

        DOVirtual.DelayedCall(2, () =>
        {
            NextQuiz();
        });

    }


    public void CorrectAnswer()
    {
        //全て押せないように
        for (int i = 0; i < 4; i++)
        {
            answerFieldTransform[i].GetChild(0).GetComponent<Button>().interactable = false;
        }

        int addScore = 0;

        StopAllCoroutines();

        if (countDownImage.fillAmount > 0)
        {
            addScore += 100;
            addScore += (int)(countDownImage.fillAmount * 100);

            UpdateScore(addScore);

            quizText.text = quizMessage;


        }
        else
        {
            //時間切れ
            quizText.text = "時間切れです...";


        }

        NextQuiz();


    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();


        if (addScore > 0)
        {
            addScoreText.color = plusColor;
            addScoreText.text = "+ " + addScore;

        }
        else
        {
            addScoreText.color = minusColor;
            addScoreText.text = "- " + -addScore;

        }

        addScoreText.gameObject.SetActive(true);
        //Vector2 defaultPosition = addScoreText.transform.localPosition;

        addScoreText.transform.localPosition = addDefaultPosition;

        addScoreText.gameObject.transform.DOLocalMoveY(80f, 1f).OnComplete(() =>
        {
            addScoreText.gameObject.SetActive(false);
            //addScoreText.transform.localPosition = defaultPosition;
        });


    }



    void NextQuiz()
    {
        questionNum++;

        //テスト
        if (noQuiz)
        {
            questionNum = 30;
        }
        

        StartCoroutine(QuizStart(questionNum));
 
    }



    IEnumerator QuizStart(int num)
    {
        SoundManager.instance.source.Stop();

        yield return new WaitForSeconds(2f);

 

        quizText.alignment = TextAlignmentOptions.Midline;
        

        if (questionNum > questionSumNum)
        {
            quizText.text = "クイズ終了！おつかれさまでした！！";
            DOVirtual.DelayedCall(2, () =>
            {
                ShowResult();
            });
            yield break;
        }
        

        quizText.text = "第" + num + "問目！";

        QuizSoundManager.instance.QuestionSE();

        countDownImage.fillAmount = 1f;

        uIShiny.Play();

        CleanField();



        yield return new WaitForSeconds(2f);

        SoundManager.instance.StartBGM("Timer");

        quizText.text = "";
        quizText.alignment = TextAlignmentOptions.TopLeft;

        SelectDish();

        GenerateQuizDish();

    }


    void ShowResult()
    {
        SoundManager.instance.StartBGM("Quiz");

        pauseButtonObj.SetActive(false);

        SetDishView(true);
        isDishMove = true;


        changeButtonObj.SetActive(false);
        currentScoreObj.SetActive(true);

        popUpTitle.SetActive(false);
        popUpResult.SetActive(true);

        popUpObj.SetActive(true);

        whiteObj.SetActive(true);

        LoadScore();


        popUpObj.transform.DOScale(1f, 0.5f).OnComplete(() =>
        {
            //LoadScore();
        });
    }


    void LoadScore()
    {
        //テスト
        if (noQuiz)
        {
            score = Random.Range(0, 1000);
        }



        int rankInNum = -1;

        int[] quizScore = new int[5] { 0, 0, 0, 0, 0 };

        if (mode == 1)
        {
            rankingText.text = "ランキング (おてがる)";
            for (int i = 0; i < 5; i++)
            {
                quizScore[i] = PlayerPrefs.GetInt("OTEQUIZSCORE_" + i, 0);
            }

            for (int i = 0; i < 5; i++)
            {
                if (score > quizScore[i])
                {

                    PlayerPrefs.SetInt("OTEQUIZSCORE_" + i, score);


                    for (int j = 4; j > i; j--)
                    {
                        PlayerPrefs.SetInt("OTEQUIZSCORE_" + j, quizScore[j - 1]);
                        quizScore[j] = quizScore[j - 1];
                    }

                    quizScore[i] = score;
                    rankInNum = i;
                    break;
                }
            }

            PlayerPrefs.Save();

        }
        else
        {
            rankingText.text = "ランキング (じっくり)";

            for (int i = 0; i < 5; i++)
            {
                quizScore[i] = PlayerPrefs.GetInt("JIKQUIZSCORE_" + i, 0);
            }

            for (int i = 0; i < 5; i++)
            {
                if (score > quizScore[i])
                {

                    PlayerPrefs.SetInt("JIKQUIZSCORE_" + i, score);


                    for (int j = 4; j > i; j--)
                    {
                        PlayerPrefs.SetInt("JIKQUIZSCORE_" + j, quizScore[j - 1]);
                        quizScore[j] = quizScore[j - 1];
                    }

                    quizScore[i] = score;
                    rankInNum = i;
                    break;
                }
            }

            PlayerPrefs.Save();
        }

        

        //ランキングに代入
        for (int i = 0; i < 5; i++)
        {
            if (i == rankInNum)
            {
                rankingScoreText[i].color = Color.red;
            }
            else
            {
                rankingScoreText[i].color = Color.black;
            }

            rankingScoreText[i].text = quizScore[i].ToString();                 
        }

        currentScoreText.text = score.ToString();
        

    }



    void SelectDish()
    {
        int answerCardID = dishList[Random.Range(0, dishList.Count)];

        dishList.Remove(answerCardID);

        quizMessage = dishInformation.DishInformationText(answerCardID);

        StartCoroutine(QuizMessage(quizMessage));
        StartCoroutine(CountDown());


        //カード生成
        //答えは一旦0に入れる
        generateCardID[0] = answerCardID;

        List<int> cardIndex = new List<int>();

        for (int i = 0; i < 27; i++)
        {
            if (i == answerCardID)
            {
                continue;
            }

            cardIndex.Add(i);
        }


        for (int i = 1; i < 4; i++)
        {
            int index = Random.Range(0, cardIndex.Count);

            generateCardID[i] = cardIndex[index];

            cardIndex.Remove(generateCardID[i]);

        }
    }


    void CleanField()
    {
        foreach (Transform field in answerFieldTransform)
        {
            if (field.childCount != 0)
            {
                Destroy(field.GetChild(0).gameObject);
            }

        }
    }


    void GenerateQuizDish()
    {

        //捨てるレア素材の選択
        List<int> fieldIndex = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            fieldIndex.Add(i);
        }

        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, fieldIndex.Count);

            bool isAnswer = false;
            if (i == 0)
            {
                isAnswer = true;
            }

            Debug.Log("index" + index);
            Debug.Log("fieldIndex" + fieldIndex[index]);

            GameObject quizDish = Instantiate(quizDishPrefab, answerFieldTransform[fieldIndex[index]], false);
            quizDish.GetComponent<QuizDishController>().SetQuizDish(generateCardID[i], isAnswer);

            fieldIndex.Remove(fieldIndex[index]);
        }



    }



    //文字列を受けて１文字づつ表示する。
    private IEnumerator QuizMessage(string message)
    {        
        quizText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            quizText.text += message[i];
            yield return new WaitForSeconds(messageSpeed);//任意の時間待つ

        }

    }



    IEnumerator CountDown()
    {
        int count = limitTime;

        while (count > 0)
        {
            yield return new WaitForSeconds(0.1f);
            //countDownBar.valueCurrent = count;

            countDownImage.fillAmount = (float)count / (float)limitTime;
            count -= 1;
        }

        //countDownBar.valueCurrent = 0;
        countDownImage.fillAmount = 0f;

        CorrectAnswer();

    }


    public void OnOtegaruButton()
    {
        mode = 1;

        whiteObj.SetActive(false);

        popUpObj.transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            popUpObj.SetActive(false);
            PrepareQuiz();
        }); 
    }

    public void OnJikkuriButton()
    {
        mode = 2;

        whiteObj.SetActive(false);

        popUpObj.transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            popUpObj.SetActive(false);
            PrepareQuiz();
        });
    }

    public void OnQuitButton()
    {
        FadeManager.FadeOut(1);

    }


    public void OnChangeButton()
    {
        popUpObj.transform.DORotate(new Vector3(0f, 90f, 0f), 0.3f).OnComplete(() =>
        {


            if (rankingMode == 1)
            {
                rankingMode = 2;
            }
            else
            {
                rankingMode = 1;
            }

            ShowRanking(rankingMode);

            popUpObj.transform.DORotate(new Vector3(0f, 0f, 0f), 0.3f).SetDelay(0.5f);


            //popUpObj.transform.DOScale(1f, 0.5f);


        });

    }

    public void OnReturnButton()
    {

        popUpObj.transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            popUpResult.SetActive(false);
            popUpTitle.SetActive(true);
            changeButtonObj.SetActive(true);


            popUpObj.transform.DOScale(1f, 0.5f);

        });
    }


    public void OnRankingButton()
    {
        ShowRanking(rankingMode);

        popUpObj.transform.DOScale(0f, 0.5f).OnComplete(() =>
        {            
            popUpTitle.SetActive(false);
            popUpResult.SetActive(true);
            

            popUpObj.transform.DOScale(1f, 0.5f);

        });
    }

    void ShowRanking(int mode)
    {

        int[] quizScore = new int[5] { 0, 0, 0, 0, 0 };
        

        currentScoreObj.SetActive(false);

        if (mode == 1)
        {
            rankingText.text = "ランキング (おてがる)";
            changeButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "じっくり";

            for (int i = 0; i < 5; i++)
            {
                quizScore[i] = PlayerPrefs.GetInt("OTEQUIZSCORE_" + i, 0);
            }

        }
        else
        {
            rankingText.text = "ランキング (じっくり)";
            changeButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "おてがる";

            for (int i = 0; i < 5; i++)
            {
                quizScore[i] = PlayerPrefs.GetInt("JIKQUIZSCORE_" + i, 0);
            }

        }

        //ランキングに代入
        for (int i = 0; i < 5; i++)
        {
            rankingScoreText[i].text = quizScore[i].ToString();
        }


    }

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
        PrepareQuiz();

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
