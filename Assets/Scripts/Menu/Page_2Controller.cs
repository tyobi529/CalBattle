using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using DanielLochner.Assets.SimpleScrollSnap;

public class Page_2Controller : MonoBehaviour
{
    [SerializeField] GameObject recordObj = null;
    [SerializeField] GameObject battleDataObj = null;
    [SerializeField] GameObject checkerObj = null;


    [SerializeField] Sprite[] missionWindowSprite = new Sprite[2];

    [SerializeField] Transform checkerParentTransfrom = null;

    [SerializeField] TextMeshProUGUI missionText = null;
    [SerializeField] Image missionWindowImage = null;
    [SerializeField] GameObject clearTextObj = null;

    int onButtonIndex = -1;


    //バトルデータ
    [SerializeField] TextMeshProUGUI battleWinNumText = null;
    [SerializeField] TextMeshProUGUI cookSumNumText = null;
    [SerializeField] TextMeshProUGUI oteHighScoreText = null;
    [SerializeField] TextMeshProUGUI jikHighScoreText = null;
    [SerializeField] TextMeshProUGUI speedHighScoreText = null;
    [SerializeField] TextMeshProUGUI checkerSumText = null;

    //ミッションとバトルデータの入れ替え
    [SerializeField] TextMeshProUGUI changeButtonText = null;


    //演出中にタップできないようにする
    //[SerializeField] GameObject clearMessagePanel = null;
    [SerializeField] GameObject clearMessage = null;
    [SerializeField] GameObject whiteObj = null;
    [SerializeField] GameObject clearMissionTextObj = null;


    //現在のデータ
    //チュートリアルクリア
    int tutorial = 0;
    //レシピ数
    int cookSumNum = 0;
    //勝利数
    int winCount = 0;
    //CPU勝利確認
    int[] winCPU = new int[3] { -1, -1, -1 };
    //クイズ
    int oteHighScore = 0;
    int jikHighScore = 0;
    //スピード
    int speedHighScore = 0;

    //クリアチェッカー数
    int checkerSum = 0;

    //以前のクリア数
    int preCheckerSum = 0;

    List<int> modeNum = new List<int>();


    List<int> newCheckNum = new List<int>();

    [SerializeField] SimpleScrollSnap scrollSnap;

    [SerializeField] MenuController menuController = null;

    [SerializeField] FooterButton footerButton_1 = null;
    [SerializeField] FooterButton footerButton_2 = null;


    //どちらを向いているか
    //public bool battleDataShow { get; set; } = true;

    [SerializeField] Page_1Controller page_1Controller = null;


    [SerializeField] Sprite[] releaseSprite = new Sprite[4] { null, null, null, null };


    //チュートリアル
    [SerializeField] GameObject tutorialObj = null;
    [SerializeField] Image tutorialTitleImage = null;
    [SerializeField] Image tutorialImage = null;
    [SerializeField] GameObject rightButtonObj = null;
    [SerializeField] GameObject leftButtonObj = null;
    [SerializeField] GameObject okButtonObj = null;

    [SerializeField] Sprite tutorialTitleSprite = null;
    [SerializeField] Sprite[] tutorialSprite = new Sprite[3] { null, null, null };
    [SerializeField] Sprite recipeTutorialTitleSprite = null;
    [SerializeField] Sprite[] recipeTutorialSprite = new Sprite[3] { null, null, null };

    int panelIndex = 0;
    bool releaseRecipe = false;
    int tutorialNum = 0;

    //bool allClear = false;

    // Start is called before the first frame update
    void Start()
    {
        //battleDataShow = true;

        clearTextObj.SetActive(false);

        missionWindowImage.sprite = missionWindowSprite[0];
        missionText.text = "下のパネルからミッションを選択しよう";
        clearTextObj.SetActive(false);


        //チュートリアル
        tutorial = PlayerPrefs.GetInt("TUTORIAL", 0);



        //データの入力
        //料理作成数
        cookSumNum = 0;
        for (int i = 0; i < 27; i++)
        {
            if (PlayerPrefs.GetInt("COOKED_" + i, 0) == 1)
            {
                cookSumNum++;
            }
        }

        //勝利数
        winCount = PlayerPrefs.GetInt("WINCOUNT", 0);

        //CPU
        for (int i = 0; i < 3; i++)
        {
            winCPU[i] = PlayerPrefs.GetInt("WINCPU_" + i, 0);
        }

        //クイズ
        oteHighScore = PlayerPrefs.GetInt("OTEQUIZSCORE_0", 0);
        jikHighScore = PlayerPrefs.GetInt("JIKQUIZSCORE_0", 0);

        //スピード
        speedHighScore = PlayerPrefs.GetInt("SPEEDMADENUM_0", 0);


        //if (PlayerPrefs.GetInt("ALLCLEAR", 0) == 1)
        //{
        //    allClear = true;
        //}

        Debug.Log("作成料理" + cookSumNum);
        Debug.Log("勝利数" + winCount);
        Debug.Log("おてがる" + oteHighScore);
        Debug.Log("じっくり" + jikHighScore);
        Debug.Log("スピード" + speedHighScore);

        checkerSum = 0;
        preCheckerSum = 0;
        modeNum.Clear();
        

        ClearCheck();

        ShowBattleData();
    }


    //バトルデータ
    void ShowBattleData()
    {
        battleWinNumText.text = winCount.ToString();
        cookSumNumText.text = cookSumNum.ToString() + "/27";
        oteHighScoreText.text = oteHighScore.ToString();
        jikHighScoreText.text = jikHighScore.ToString();
        speedHighScoreText.text = speedHighScore.ToString();
        checkerSumText.text = checkerSum.ToString() + "/16";

    }


    public void OnChangeButton()
    {
        ChangePanel();
    }


    void ChangePanel()
    {
        recordObj.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.2f).OnComplete(() => {
            battleDataObj.SetActive(!battleDataObj.activeSelf);
            checkerObj.SetActive(!checkerObj.activeSelf);

            if (battleDataObj.activeSelf)
            {
                changeButtonText.text = "ミッションを見る";
                //battleDataShow = true;
            }
            else
            {
                changeButtonText.text = "戦いの記録を見る";
                //battleDataShow = false;
            }
            
            recordObj.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.2f).SetDelay(0.3f);
        });

        
        
        //Debug.Log("battleData" + battleDataShow);

    }

    public void ShowMission()
    {
        if (checkerObj.activeSelf)
        {
            return;
        }

        recordObj.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.2f).OnComplete(() => {
            battleDataObj.SetActive(false);
            checkerObj.SetActive(true);

            changeButtonText.text = "戦いの記録を見る";


            recordObj.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.2f).SetDelay(0.3f);
        });


    }



    /// <summary>
    ///ミッション関連 
    /// </summary>

    //クリア状況のチェック
    void ClearCheck()
    {
        //int[] clearNum = new int[16];

        newCheckNum.Clear();

        for (int i = 0; i < 16; i++)
        {
            int num = PlayerPrefs.GetInt("MISSION_" + i, 0);

            if (num == 0)
            {
                checkerParentTransfrom.GetChild(i).GetComponent<CheckerButtonController>().SetCheckerButton(i, num);

                //クリアしているかの確認

                //クリアしているなら
                num = CheckMethod(i);

                //テスト
                //if (i == 0)
                //{
                //    num = 1;
                //}
                

                if (num == 1)
                {
                    newCheckNum.Add(i);
                }
                //Check(i);
            }
            else
            {
                //clearNum[i] = num;
                checkerParentTransfrom.GetChild(i).GetComponent<CheckerButtonController>().SetCheckerButton(i, num);
                preCheckerSum++;
            }

            if (num == 1)
            {
                checkerSum++;
            }


        }



        NewClearCheck();
    }



    void NewClearCheck()
    {

        if (newCheckNum.Count == 0)
        {
            return;
        }

        Debug.Log("チェックあり");



        DOVirtual.DelayedCall(2f, () =>
        {
            ClearMissionAnimation(true, 3);
        });


    }



    //true:チェッカークリア
    //0:レシピ開放
    //1:スピード開放
    //2:クイズ開放
    //3:新しいチェッカークリア
    void ClearMissionAnimation(bool clear, int num)
    {
        if (!clear)
        {
            clearMissionTextObj.GetComponent<Image>().sprite = releaseSprite[num];
        }


        //図鑑開放フラグ
        if (num == 0)
        {
            releaseRecipe = true;
        }



        clearMissionTextObj.transform.localPosition = new Vector3(750f, 0f);
        whiteObj.transform.localPosition = new Vector3(750f, 0f);
        clearMessage.SetActive(true);
        whiteObj.SetActive(true);
        clearMissionTextObj.SetActive(true);


        DOTween.Sequence().Append(DOTween.ToAlpha(
  () => clearMessage.GetComponent<Image>().color,
  color => clearMessage.GetComponent<Image>().color = color,
  0.7f, // 目標値
  1f// 所要時間
  )).Append(
            whiteObj.transform.DOLocalMove(new Vector3(0f, 0f), 0.3f)
            ).Append(
                clearMissionTextObj.transform.DOLocalMove(new Vector3(0f, 0f), 0.3f)
            ).OnComplete(() => {


                //演出
                //clearMissionTextObj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                clearMissionTextObj.transform.GetChild(0).gameObject.SetActive(true);


                DOVirtual.DelayedCall(2, () =>
                {
                    DOTween.Sequence().Append(clearMissionTextObj.transform.DOLocalMove(new Vector3(-750f, 0f), 0.3f)).Append(
    whiteObj.transform.DOLocalMove(new Vector3(-750f, 0f), 0.3f)
    ).Append(
    DOTween.ToAlpha(
() => clearMessage.GetComponent<Image>().color,
color => clearMessage.GetComponent<Image>().color = color,
0f, // 目標値
1f// 所要時間
).OnComplete(() =>
{
    //念の為Kill
    if (DOTween.instance != null)
    {
        DOTween.KillAll();
    }

    //clearMessage.SetActive(false);
    whiteObj.SetActive(false);
    clearMissionTextObj.SetActive(false);

    clearMissionTextObj.transform.GetChild(0).gameObject.SetActive(false);

    if (clear)
    {
        MoveToCheckerPanel();
    }
    else
    {
        modeNum.RemoveAt(0);
        CheckRelease();
    }
    
})

);  
                });


 
            });

   

    }

    //モード開放
    void ReleaseMode()
    {
        //clearMessage.SetActive(true);
        //whiteObj.SetActive(false);
        //clearMissionTextObj.SetActive(false);

        page_1Controller.CheckMode();

        if (MenuSetting.instance.cutMissionClear)
        {
            return;
        }

        //List<int> modeNum = new List<int>();

 
        //最初
        if (preCheckerSum == 0 && checkerSum == 1)
        {
            tutorialNum = 0;
            ShowMissionTutorial();
            return;
        }

        //開放なし
        if (preCheckerSum < 3)
        {
            if (checkerSum >= 7)
            {
                modeNum.Add(0);
                modeNum.Add(1);
                modeNum.Add(2);
            }
            else if (checkerSum >= 5)
            {
                modeNum.Add(0);
                modeNum.Add(1);
            }
            else if (checkerSum >= 3)
            {
                modeNum.Add(0);
            }
        }
        //レシピ開放済み
        else if (preCheckerSum < 5)
        {
            if (checkerSum >= 7)
            {
                modeNum.Add(1);
                modeNum.Add(2);
            }
            else if (checkerSum >= 5)
            {
                modeNum.Add(1);
            }
        }
        //レシピ、スピード開放済み
        else if (preCheckerSum < 7)
        {
            if (checkerSum >= 7)
            {
                modeNum.Add(2);
            }
        }

        //全クリア
        if (preCheckerSum < 16 && checkerSum == 16)
        {
            modeNum.Add(3);
        }


        //新しい開放なし
        if (modeNum.Count == 0)
        {
            return;
        }

        foreach (int a in modeNum)
        {
            Debug.Log("開放" + a);
        }

        CheckRelease();

    }

    void CheckRelease()
    {
        //全ての開放表示したあと
        if (modeNum.Count == 0)
        {
            //念の為Kill
            if (DOTween.instance != null)
            {
                DOTween.KillAll();
            }
            clearMessage.SetActive(false);

            //移動
            footerButton_1.OnFooterButton();

            //レシピ集の開放
            if (releaseRecipe)
            {
                tutorialNum = 1;
                ShowMissionTutorial();
            }
            return;
            
        }

        ClearMissionAnimation(false, modeNum[0]);

        


    }


    void MoveToCheckerPanel()
    {
        footerButton_2.OnFooterButton();


        DOVirtual.DelayedCall(0.5f, () =>
        {
            ShowMission();
            
            //clearMessagePanel.SetActive(false);

            //CheckAnimation();
            StartCoroutine(CheckAnimation());
        });


    }

    void ShowMissionTutorial()
    {
        if (tutorialNum == 0)
        {
            tutorialTitleImage.sprite = tutorialTitleSprite; 
        }
        else
        {
            tutorialTitleImage.sprite = recipeTutorialTitleSprite;
        }

        tutorialObj.GetComponent<CanvasGroup>().alpha = 0;
        tutorialObj.SetActive(true);
        ChangePanel(tutorialNum, 0);

        tutorialObj.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

    }


    void ChangePanel(int num, int index)
    {
        int maxIndex = 2;

        if (num == 0)
        {
            tutorialImage.sprite = tutorialSprite[index];
            maxIndex = 2;
        }
        else
        {
            tutorialImage.sprite = recipeTutorialSprite[index];
            maxIndex = 2;
        }
        

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
        ChangePanel(tutorialNum, panelIndex + 1);
    }

    public void OnLeftButton()
    {
        ChangePanel(tutorialNum, panelIndex - 1);
    }

    public void OnOkButton()
    {
        tutorialObj.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        tutorialObj.SetActive(false);
    }


    IEnumerator CheckAnimation()
    {
        foreach (int num in newCheckNum)
        {


            if (!MenuSetting.instance.cutMissionClear)
            {
                yield return new WaitForSeconds(2);
            }
            
           
            //checkerParentTransfrom.GetChild(num).GetComponent<CheckerButtonController>().clearNum = 1;
            checkerParentTransfrom.GetChild(num).GetComponent<CheckerButtonController>().OnCheckerButton();
            checkerParentTransfrom.GetChild(num).GetComponent<CheckerButtonController>().ChangeClearView();

            PlayerPrefs.SetInt("MISSION_" + num, 1);


  
        }

        PlayerPrefs.Save();

        yield return new WaitForSeconds(2);

        //念の為Kill
        if (DOTween.instance != null)
        {
            DOTween.KillAll();
        }

        clearMessage.SetActive(false);


        ReleaseMode();
        
    }





    int CheckMethod(int num)
    {
        int clearNum = 0;
        switch (num)
        {
            case 0:
                clearNum = tutorial;
                break;
            case 1:
                clearNum = CheckWinCount(0);
                break;
            case 2:
                clearNum = CheckWinCount(1);
                break;
            case 3:
                clearNum = CheckWinCount(2);
                break;
            case 4:
                clearNum = CheckWinCPU(0);
                break;
            case 5:
                clearNum = CheckWinCPU(1);
                break;
            case 6:
                clearNum = CheckWinCPU(2);
                break;
            case 7:
                clearNum = CheckOteQuiz();
                break;
            case 8:
                clearNum = CheckJikQuiz();
                break;
            case 9:
                clearNum = CheckSpeed(0);
                break;
            case 10:
                clearNum = CheckSpeed(1);
                break;
            case 11:
                clearNum = CheckCookSum(0);
                break;
            case 12:
                clearNum = CheckCookSum(1);
                break;
            case 13:
                clearNum = CheckCookSum(2);
                break;
            case 14:
                clearNum = CheckCookSum(3);
                break;
            case 15:
                clearNum = CheckCookSum(4);
                break;
        }

        return clearNum;
    }




    public void OnCheckerButton(int index, int clearNum)
    {
        //押されているボタンのキャンセル
        if (onButtonIndex >= 0)
        {
            checkerParentTransfrom.GetChild(onButtonIndex).GetComponent<CheckerButtonController>().selecter.SetActive(false);

        }

        onButtonIndex = index;

        missionWindowImage.sprite = missionWindowSprite[clearNum];

        if (clearNum == 0)
        {
            clearTextObj.SetActive(false);
        }
        else
        {
            clearTextObj.SetActive(true);
        }
        

        SetMessage(index);
    }



    void SetMessage(int index)
    {
        string message = SelectMissionText(index);

        missionText.text = message;
    }



    string SelectMissionText(int index)
    {
        string message = "";

        switch (index)
        {
            case 0:
                message = "チュートリアルをクリアする";
                break;
            case 1:
                message = "バトルで１回勝利する";
                break;
            case 2:
                message = "バトルで３回勝利する";
                break;
            case 3:
                message = "バトルで５回勝利する";
                break;
            case 4:
                message = "\"ひとりでバトル\"でCPU(よわい)に勝利する";
                break;
            case 5:
                message = "\"ひとりでバトル\"でCPU(ふつう)に勝利する";
                break;
            case 6:
                message = "\"ひとりでバトル\"でCPU(つよい)に勝利する";
                break;
            case 7:
                message = "\"料理クイズ\"おてがるで1500点以上取る";
                break;
            case 8:
                message = "\"料理クイズ\"じっくりで4000点以上取る";
                break;
            case 9:
                message = "\"料理スピード\"で10品以上作る";
                break;
            case 10:
                message = "\"料理スピード\"で15品以上作る";
                break;
            case 11:
                message = "料理レシピを５種類以上集める";
                break;
            case 12:
                message = "料理レシピを10種類以上集める";
                break;
            case 13:
                message = "料理レシピを15種類以上集める";
                break;
            case 14:
                message = "料理レシピを20種類以上集める";
                break;
            case 15:
                message = "料理レシピを全て(27種)集める";
                break;

        }

        return message;

    }

    int CheckCookSum(int num)
    {
        int needNum = 0;
        switch (num)
        {
            case 0:
                needNum = 5;
                break;
            case 1:
                needNum = 10;
                break;
            case 2:
                needNum = 15;
                break;
            case 3:
                needNum = 20;
                break;
            case 4:
                needNum = 27;
                break;
        }

        if (cookSumNum >= needNum)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }



    int CheckWinCount(int num)
    {
        int needNum = 0;
        switch (num)
        {
            case 0:
                needNum = 1;
                break;
            case 1:
                needNum = 3;
                break;
            case 2:
                needNum = 5;
                break;
        }

        if (winCount >= needNum)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    int CheckWinCPU(int num)
    {
        return winCPU[num];
    }

    int CheckOteQuiz()
    {
        if (oteHighScore >= 1500)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    int CheckJikQuiz()
    {
        if (jikHighScore >= 4000)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    int CheckSpeed(int num)
    {
        int needNum = 0;

        switch (num)
        {
            case 0:
                needNum = 10;
                break;
            case 1:
                needNum = 15;
                break;
        }

        if (speedHighScore >= needNum)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

}
