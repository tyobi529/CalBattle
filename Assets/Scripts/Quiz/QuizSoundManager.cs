using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source = null;

    [SerializeField] AudioClip correctSE = null;
    [SerializeField] AudioClip wrongSE = null;
    [SerializeField] AudioClip questionSE = null;


    public static QuizSoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

        ChangeVolume();
    }

    public void CorrectSE()
    {
        source.PlayOneShot(correctSE);
    }

    public void WrongSE()
    {
        source.PlayOneShot(wrongSE);
    }

    public void QuestionSE()
    {
        source.PlayOneShot(questionSE);
    }



    public void ChangeVolume()
    {
        int sound = PlayerPrefs.GetInt("SOUND", 0);

        if (sound == 0)
        {
            source.mute = true;
        }
        else
        {
            source.mute = false;
        }


        source.volume = PlayerPrefs.GetFloat("VOLUME", 0f);

    }


    public void ChangeMute()
    {
        int sound = PlayerPrefs.GetInt("SOUND", 0);

        if (sound == 0)
        {
            source.mute = true;
        }
        else
        {
            source.mute = false;
        }
    }

}
