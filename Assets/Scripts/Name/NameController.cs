using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class NameController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image windowImage = null;

    //音量
    [SerializeField] Toggle soundToggle = null;
    [SerializeField] Slider BGMSlider = null;
    [SerializeField] GameObject soundButtonObj = null;


    //名前
    [SerializeField] TextMeshProUGUI messageText = null;
    [SerializeField] float messageSpeed = 0f;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] int maxNameCount = 6;
    [SerializeField] int minNameCount = 2;

    [SerializeField] Button nameDecideButton = null;

    int messageNum = 0;
    public bool isMessage { get; set; } = false;

    string preName = null;

    string playerName = null;



    [SerializeField] GameObject nameObj = null;
    [SerializeField] GameObject soundObj = null;

    [SerializeField] GameObject decideButton = null;

    [SerializeField] GameObject tutorialStartButton = null;


    [SerializeField] GameObject triangle = null;
    float sin = 0f;
    Vector2 firstPos;



    // Start is called before the first frame update
    void Start()
    {

        nameDecideButton.interactable = false;

        isMessage = false;
                
        firstPos = triangle.transform.localPosition;




        windowImage.color = new Color(1f, 1f, 1f, 0f);

        DOTween.ToAlpha(
() => windowImage.color,
color => windowImage.color = color,
1f, // 目標値
2f// 所要時間
).OnComplete(() => {
    ChangeMessage(0);
});


    }


    private void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{


        if (isMessage)
        {
            //isMessage = false;
            //ChangeMessage(messageNum);
            //triangle.SetActive(true);
            //sin = Mathf.Sin(Time.time * 5f);

            //triangle.transform.localPosition = new Vector2(firstPos.x, firstPos.y + 5f * sin);

        }

        if (triangle.activeSelf)
        {
            sin = Mathf.Sin(Time.time * 5f);

            triangle.transform.localPosition = new Vector2(firstPos.x, firstPos.y + 5f * sin);
        }


        //}



    }

    public void OnPointerClick(PointerEventData eventData)
    {


        if (isMessage)
        {
            isMessage = false;

            Debug.Log("クリック検知");            
            ChangeMessage(messageNum);

        }



    }


    public void ChangeText(string name)
    {
        //string playerName = inputField.text;

        Debug.Log(name.Length);

        if (name.Length > maxNameCount || name.Length < minNameCount)
        {
            nameDecideButton.interactable = false;
        }
        else
        {
            nameDecideButton.interactable = true;


        }


        //漢字
        bool correct = true;
        int i = 0;
        foreach (char c in name)
        {
            if (IsKanji(c))
            {
                nameDecideButton.interactable = false;
                inputField.text = preName;
                correct = false;
                break;

            }
            else
            {
                i++;
            }

        }

        if (correct)
        {
            preName = name;
        }



    }


    bool IsKanji(char c)
    {
        //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
        return ('\u4E00' <= c && c <= '\u9FCF')
            || ('\uF900' <= c && c <= '\uFAFF')
            || ('\u3400' <= c && c <= '\u4DBF');

    }



    public void OnNameDecideButton()
    {


        playerName = inputField.text;


        //messageNum = 2;

        //waitDecide = true;
        //nameInput = false;

        ChangeMessage(4);


        ShowPopUp(nameObj, false);

        

               
    }

    public void OnSoundDecideButton()
    {

        ShowPopUp(soundObj, false);
        ChangeMessage(2);

        soundButtonObj.SetActive(true);
        


    }




    void ChangeMessage(int num)
    {
        string message = "";

        //生成後に入力を受け付けるかどうか
        //bool isMessage = false;

        switch (num)
        {
            case 0:
                message = "\"カロバト\"へようこそ！";    
                break;
            case 1:
                message = "最初に音量の設定をしましょう";
                break;
            case 2:
                message = "ミュートの切り替えは右上のボタンでいつでも出来ます";
                break;
            case 3:
                message = "次にあなたの名前を教えてください";
                break;
            case 4:
                message = "名前は「" + playerName + "」でよろしいですか？";
                break;
            case 5:
                message = playerName + " さんよろしくお願いします！";
                break;
            case 6:
                message = "まずはバトルのチュートリアルからはじめましょう！";                                
                break;
        }

        messageNum = num;
        StartCoroutine(ChangeMessage(message));
    }


    //文字列を受けて１文字づつ表示する。
    private IEnumerator ChangeMessage(string message)
    {
        messageText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            messageText.text += message[i];
            yield return new WaitForSeconds(messageSpeed);//任意の時間待つ

        }

        if (messageNum == 1)
        {
            ShowPopUp(soundObj ,true);
        }
        else if (messageNum == 3)
        {
            ShowPopUp(nameObj, true);
        }
        else if (messageNum == 4)
        {
            decideButton.SetActive(true);
        }
        else if (messageNum == 6)
        {
            ShowTutorialStartButton();
        }
        else
        {            
            messageNum++;
            isMessage = true;
        }


        triangle.SetActive(isMessage);

        Debug.Log(messageNum);
        Debug.Log(isMessage);

        //if (messageNum == 3)
        //{
        //    ChangeMessage(3);
        //}

    }


    void ShowPopUp(GameObject gameObject, bool isShow)
    {

        if (isShow)
        {
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            gameObject.SetActive(true);
            gameObject.transform.DOScale(1f, 0.3f);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);            
            gameObject.SetActive(false);
            gameObject.transform.DOScale(0f, 0.3f);
        }

    }


    public void OnOkButton()
    {
            PlayerPrefs.SetString("NAME", playerName);
            PlayerPrefs.Save();
            ChangeMessage(5);
            decideButton.SetActive(false);
        

    }

    public void OnCancelButton()
    {
        //ShowNameObj(true);
        decideButton.SetActive(false);
        ChangeMessage(3);
    }

    void ShowTutorialStartButton()
    {
        tutorialStartButton.transform.localScale = new Vector3(0f, 0f);
        tutorialStartButton.SetActive(true);

        tutorialStartButton.transform.DOScale(1f, 0.3f);
    }

    public void OnTutorialStartButton()
    {
        SoundManager.instance.isTutorial = true;
        FadeManager.FadeOut(2);


    }


    ///音量のオンオフ
    public void ToggleSound()
    {
        int sound = PlayerPrefs.GetInt("SOUND", 0);

        if (sound == 0)
        {
            PlayerPrefs.SetInt("SOUND", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
        }

        PlayerPrefs.Save();



        SoundManager.instance.ChangeVolume();
    }

    public void VolumeChanged()
    {
        PlayerPrefs.SetFloat("VOLUME", BGMSlider.value);

        PlayerPrefs.Save();

        SoundManager.instance.ChangeVolume();

    }

}
