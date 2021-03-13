using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source = null;

    [SerializeField] AudioClip[] kiraSE = new AudioClip[2] { null, null };

    [SerializeField] AudioClip changeCardSE = null;

    public static SpeedSoundManager instance;

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

    public void KiraSE(int num)
    {
        source.PlayOneShot(kiraSE[num]);
    }

    public void ChangeCardSE()
    {
        source.PlayOneShot(changeCardSE);
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
