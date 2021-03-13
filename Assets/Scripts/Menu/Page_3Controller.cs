using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Page_3Controller : MonoBehaviour
{
    [SerializeField] GameObject popUp = null;
    //各項目のPopUp
    [SerializeField] GameObject changeNameObj = null;

    [SerializeField] GameObject changeSoundObj = null;

    [SerializeField] GameObject tutorialObj = null;

    [SerializeField] GameObject deleteDataObj = null;



    //名前
    [SerializeField] TMP_InputField inputField;

    [SerializeField] int maxNameCount = 6;
    [SerializeField] int minNameCount = 2;    

    [SerializeField] Button nameDecideButton = null;

    string preName = null;

    //サウンド
    public Toggle soundToggle = null;
    [SerializeField] Slider BGMSlider = null;


    //ヘッダーのアイコン
    [SerializeField] Image soundImage = null;
    [SerializeField] Sprite[] soundSprite = new Sprite[2];


    //データの消去
    [SerializeField] TextMeshProUGUI deleteDataText = null;
    [SerializeField] GameObject deleteButtonObj = null;
    bool canDelete = false;
    float deleteTime = 0f;

    [SerializeField] GameObject deleteCancelButtonObj = null;
    [SerializeField] GameObject deleteDecideButtonObj = null;
    //bool isCountDown = false;

    [SerializeField] GameObject blockPanel = null;



    private void Start()
    {
        
        int sound = PlayerPrefs.GetInt("SOUND", 0);
        Debug.Log("sound" + sound);
        if (sound == 1)
        {            
            soundToggle.SetIsOnWithoutNotify(false);
            
        }
        else
        {
            soundToggle.SetIsOnWithoutNotify(true);
        }

        float volume = PlayerPrefs.GetFloat("VOLUME", 0f);
        BGMSlider.value = volume;

        Debug.Log("Volume" + volume);

        Debug.Log("Sound" + sound);
        soundImage.sprite = soundSprite[sound];
        
    }


    private void Update()
    {
        if (canDelete)
        {
            deleteTime += Time.deltaTime;

            if (deleteTime > 5f)
            {
                deleteButtonObj.GetComponent<Button>().interactable = true;
                canDelete = false;
                deleteTime = 0f;

            }
        }

    }


    public void ShowPopUp(bool isShow)
    {
        if (isShow)
        {
            popUp.transform.localScale = new Vector3(0f, 0f, 0f);
            popUp.SetActive(true);
            popUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
        }
        else
        {
            popUp.transform.localScale = new Vector3(1f, 1f, 1f);            
            popUp.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).OnComplete(() => {
                popUp.SetActive(false);
            });
        }
  
    }


    public void OnTitleButton()
    {
        FadeManager.FadeOut(0);
    }

    public void OnChangeNameButton()
    {
        changeNameObj.SetActive(true);
        changeSoundObj.SetActive(false);
        tutorialObj.SetActive(false);
        deleteDataObj.SetActive(false);
        ShowPopUp(true);
        canDelete = false;
    }

    public void OnChangeSoundButton()
    {
        changeSoundObj.SetActive(true);
        changeNameObj.SetActive(false);
        tutorialObj.SetActive(false);
        deleteDataObj.SetActive(false);
        ShowPopUp(true);
        canDelete = false;
    }

    public void OnTutorialButton()
    {
        changeSoundObj.SetActive(false);
        changeNameObj.SetActive(false);
        tutorialObj.SetActive(true);
        deleteDataObj.SetActive(false);
        ShowPopUp(true);
        canDelete = false;

    }

    public void OnDeleteDataButton()
    {
        deleteDataObj.SetActive(true);
        changeSoundObj.SetActive(false);
        changeNameObj.SetActive(false);
        tutorialObj.SetActive(false);

        deleteButtonObj.SetActive(true);
        deleteCancelButtonObj.SetActive(false);
        deleteDecideButtonObj.SetActive(false);

        deleteDataText.text = "ゲームの全てのデータを\n削除しますか？";

        ShowPopUp(true);

        canDelete = true;
        deleteTime = 0f;
        deleteButtonObj.GetComponent<Button>().interactable = false;
    }

    public void OnStartTutorialButton()
    {
        SoundManager.instance.isTutorial = true;
        FadeManager.FadeOut(2);
    }



    public void OndeleteButton()
    {
        deleteButtonObj.SetActive(false);
        deleteDecideButtonObj.SetActive(true);
        deleteCancelButtonObj.SetActive(true);

        deleteDataText.text = "本当に削除しますか？";
    }

    //消去決定
    public void OnDeleteDecideButton()
    {
        PlayerPrefs.DeleteAll();

        deleteDataText.text = "データを消去しました\nタイトルに戻ります";

        deleteDecideButtonObj.SetActive(false);
        deleteCancelButtonObj.SetActive(false);
        blockPanel.SetActive(true);

        DOVirtual.DelayedCall(3, () =>
        {
            FadeManager.FadeOut(0);
        });
    }



    public void OnBatsuButton()
    {        
        ShowPopUp(false);
        canDelete = false;        
    }

    /// <summary>
    /// 名前
    /// </summary>
    public void ChangeText(string name)
    {
        //string playerName = inputField.text;


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
        string playerName = inputField.text;

        PlayerPrefs.SetString("NAME", playerName);
        PlayerPrefs.Save();


        //チェック
        playerName = PlayerPrefs.GetString("NAME", "あなた");

        Debug.Log("名前:" + playerName);

        FadeManager.FadeOut(1);

    }


    ///サウンド
    ///音量のオンオフ
    public void ToggleSound()
    {
        int sound = PlayerPrefs.GetInt("SOUND", 0);

        if (sound == 0)
        {
            PlayerPrefs.SetInt("SOUND", 1);
            soundImage.sprite = soundSprite[1];
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
            soundImage.sprite = soundSprite[0];
        }

        PlayerPrefs.Save();



        SoundManager.instance.ChangeVolume();
    }



    //ボタンで操作する
    public void OnSoundButton()
    {
        if (soundToggle.isOn)
        {
            soundToggle.isOn = false;
        }
        else
        {
            soundToggle.isOn = true;
        }
    }


    public void VolumeChanged()
    {
        PlayerPrefs.SetFloat("VOLUME", BGMSlider.value);

        PlayerPrefs.Save();

        SoundManager.instance.ChangeVolume();

    }
}
