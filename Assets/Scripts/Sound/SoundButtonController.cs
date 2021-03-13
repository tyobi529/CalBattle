using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    [SerializeField] Image soundImage = null;

    [SerializeField] Sprite[] soundSprite = new Sprite[2] { null, null };    
    

    //[SerializeField] AudioSource battleAudioSource = null;

    [SerializeField] Page_3Controller page_3Controller = null;

    private void Start()
    {
        //サウンドがない場合
        if (SoundManager.instance == null)
        {
            return;
        }

        int sound = PlayerPrefs.GetInt("SOUND", 0);

        soundImage.sprite = soundSprite[sound];
        //battleAudioSource
    }

    public void OnSoundButton()
    {
        //サウンドがない場合
        if (SoundManager.instance == null)
        {
            return;
        }

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


        if (BattleSoundManager.instance != null)
        {
            BattleSoundManager.instance.ChangeMute();
        }

        if (QuizSoundManager.instance != null)
        {
            QuizSoundManager.instance.ChangeMute();
        }

        if (SpeedSoundManager.instance != null)
        {
            SpeedSoundManager.instance.ChangeMute();
        }

        if (RecipeSoundManager.instance != null)
        {
            RecipeSoundManager.instance.ChangeMute();
        }

        SoundManager.instance.ChangeMute();

        if (page_3Controller != null)
        {
            if (sound == 0)
            {
                page_3Controller.soundToggle.SetIsOnWithoutNotify(false);
            }
            else
            {
                page_3Controller.soundToggle.SetIsOnWithoutNotify(true);
            }
            
        }
        
        
        
    }
}
