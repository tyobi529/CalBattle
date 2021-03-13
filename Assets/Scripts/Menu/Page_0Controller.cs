using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Page_0Controller : MonoBehaviour
{

    [SerializeField] GameObject[] popUp = new GameObject[2] { null, null };
    [SerializeField] GameObject returnButtonObj = null;

    [SerializeField] Image[] strengthButtonImage = new Image[3] { null, null, null };


    [SerializeField] Toggle keyRoomToggle = null;
    [SerializeField] TMP_InputField roomIDinputField = null;
    [SerializeField] GameObject alignRoomIDTextObj = null;



    [SerializeField] GameObject singleStartButtonObj = null;
    [SerializeField] GameObject multiStartButtonObj = null;



    [SerializeField] GameObject matchingPanel = null;


    [SerializeField] GameObject singlePlayButtonObj = null;
    [SerializeField] GameObject multiPlayButtonObj = null;


    [SerializeField] TextMeshProUGUI roomIDText = null;
    //[SerializeField] TextMeshProUGUI searchingText = null;
    [SerializeField] Image matchingTextImage = null;
    [SerializeField] Sprite[] matchingTextSprite = new Sprite[3] { null, null, null };


    [SerializeField] GameObject okButtonObj = null;
    [SerializeField] GameObject cancelButtonObj = null;
    [SerializeField] GameObject quitButtonObj = null;
    [SerializeField] GameObject roadingObj = null;


    [SerializeField] MatchingController matchingController = null;


    private string preRoomID = null;

    [SerializeField] int maxRoomIDCount = 0;


    //public static int cpuLevel = 3;

    int cpuLevel = -1;

    // Start is called before the first frame update
    void Start()
    {
        //cpuLevel = 1;
        OnStrengthButton_0();

        CheckKeyRoom();

        for (int i = 0; i < 2; i++)
        {
            popUp[i].transform.localScale = new Vector3(0f, 0f, 0f);
        }

    }



    public void OnSinglePlayButton()
    {

        //singlePlayButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
        //    singlePlayButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        //});


        popUp[0].transform.localScale = new Vector3(0f, 0f, 0f);
        popUp[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);

        returnButtonObj.SetActive(true);

    }

    public void OnMultiPlayButton()
    {
        //multiPlayButtonObj.transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
        //    multiPlayButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        //});

        popUp[1].transform.localScale = new Vector3(0f, 0f, 0f);
        popUp[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);

        returnButtonObj.SetActive(true);
    }

    public void OnReturnButton()
    {
        for (int i = 0; i < 2; i++)
        {
            popUp[i].transform.localScale = new Vector3(1f, 1f, 1f);
            popUp[i].transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f);
        }

        returnButtonObj.SetActive(false);



    }


    public void OnStrengthButton_0()
    {
        cpuLevel = 1;

        strengthButtonImage[0].color = new Color(194f / 255f, 243f / 255f, 226f / 255f, 1f);
        strengthButtonImage[1].color = Color.white;
        strengthButtonImage[2].color = Color.white;
    }

    public void OnStrengthButton_1()
    {
        cpuLevel = 2;

        strengthButtonImage[0].color = Color.white;
        strengthButtonImage[1].color = new Color(194f / 255f, 243f / 255f, 226f / 255f, 1f);
        strengthButtonImage[2].color = Color.white;
    }

    public void OnStrengthButton_2()
    {
        cpuLevel = 3;

        strengthButtonImage[0].color = Color.white;
        strengthButtonImage[1].color = Color.white;
        strengthButtonImage[2].color = new Color(194f / 255f, 243f / 255f, 226f / 255f, 1f);
    }

    //ゲームシーンから呼ぶ
    //public static int GetCpuLevel()
    //{
    //    return cpuLevel;
    //}

    //IDの有無のチェックを変更
    public void CheckKeyRoom()
    {
        roomIDinputField.interactable = keyRoomToggle.isOn;
        alignRoomIDTextObj.SetActive(keyRoomToggle.isOn);


        if (keyRoomToggle.isOn && roomIDinputField.text.Length != maxRoomIDCount)
        {
            //multiStartButtonObj.SetActive(false);
            multiStartButtonObj.GetComponent<Button>().interactable = false;
        }
        else
        {
            //multiStartButtonObj.SetActive(true);
            multiStartButtonObj.GetComponent<Button>().interactable = true;
        }
    }



    //４文字であるかをチェックする
    public void CheckInputText()
    {

        if (roomIDinputField.text.Length > maxRoomIDCount)
        {
            roomIDinputField.text = preRoomID;
        }
        else
        {
            preRoomID = roomIDinputField.text;
        }


        if (keyRoomToggle.isOn && preRoomID.Length != maxRoomIDCount)
        {
            //multiStartButtonObj.SetActive(false);
            multiStartButtonObj.GetComponent<Button>().interactable = false;
        }
        else
        {
            //multiStartButtonObj.SetActive(true);
            multiStartButtonObj.GetComponent<Button>().interactable = true;
        }

    }




    public void OnSingleStartButton()
    {
        PlayerPrefs.SetInt("CPULEVEL", cpuLevel);

        singleStartButtonObj.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            singleStartButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
                FadeManager.FadeOut(2);
        });


    }

    public void OnMultiStartButton()
    {
        multiStartButtonObj.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {
            multiStartButtonObj.transform.localScale = new Vector3(1f, 1f, 1f);
        });

        matchingPanel.SetActive(true);


        //IDの入力あり
        if (keyRoomToggle.isOn)
        {
            roomIDText.text = "ルームID「" + roomIDinputField.text + "」";
        }
        else
        {
            roomIDText.text = "ルームID無し";
        }

        //searchingText.text = "対戦相手を探します";
        matchingTextImage.sprite = matchingTextSprite[0];

        okButtonObj.SetActive(true);
        cancelButtonObj.SetActive(true);
    }

    public void OnOkButton()
    {
        //searchingText.text = "対戦相手を探しています";
        matchingTextImage.sprite = matchingTextSprite[1];
        okButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
        quitButtonObj.SetActive(true);
        roadingObj.SetActive(true);

        Debug.Log("roomID" + preRoomID);

        if (keyRoomToggle.isOn)
        {
            //Debug.Log("鍵あり");
            matchingController.StartMatching(preRoomID);
        }
        else
        {
            //Debug.Log("鍵なし");
            matchingController.StartMatching(null);
        }
        
    }

    public void OnCancelButton()
    {
        matchingPanel.SetActive(false);
    }

    public void OnQuitButton()
    {
        matchingPanel.SetActive(false);
        quitButtonObj.SetActive(false);
        roadingObj.SetActive(false);

        matchingController.CancelMatching();
    }

    public void MatchingSuccess()
    {
        quitButtonObj.SetActive(false);

        roadingObj.SetActive(false);
        matchingTextImage.sprite = matchingTextSprite[2];
        matchingTextImage.transform.DOLocalMoveY(20f, 0.4f)
.SetRelative(true)
.SetEase(Ease.OutQuad)
.SetLoops(4, LoopType.Yoyo);
    }
}
