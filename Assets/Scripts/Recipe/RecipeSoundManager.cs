using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source = null;

    [SerializeField] AudioClip bookSE = null;


    public static RecipeSoundManager instance;

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

    public void BookSE()
    {
        source.PlayOneShot(bookSE);
    }
}
