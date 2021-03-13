using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source = null;

    [SerializeField] AudioClip[] attackSE = new AudioClip[3] { null, null, null };
    [SerializeField] AudioClip[] specialSE = new AudioClip[2] { null, null };
    [SerializeField] AudioClip tapButtonSE = null;
    [SerializeField] AudioClip[] resultSE = new AudioClip[2] { null, null };
    [SerializeField] AudioClip turnSE = null;

    [SerializeField] AudioClip tapCardSE = null;
    [SerializeField] AudioClip tapCardCancelSE = null;

    [SerializeField] AudioClip cookSE = null;
    [SerializeField] AudioClip cookEndSE = null;

    [SerializeField] AudioClip powerUpSE = null;

    [SerializeField] AudioClip startSE = null;
    [SerializeField] AudioClip fireSE = null;

    public static BattleSoundManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {

        Debug.Log("SOUND" + PlayerPrefs.GetInt("SOUND", 0));
        Debug.Log("SOUND" + PlayerPrefs.GetInt("VOLUME", 0));


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


    public void StopSE()
    {
        source.Stop();
    }

    public void AttackSE(int num)
    {
        source.PlayOneShot(attackSE[num]);
    }

    //0:good
    //1:bad
    public void SpecialSE(int num)
    {
        source.PlayOneShot(specialSE[num]);
    }

    public void TapButtonSE()
    {
        source.PlayOneShot(tapButtonSE);
    }


    public void ResultSE(int num)
    {
        //１大きく入力しているため
        source.PlayOneShot(resultSE[num - 1]);
    }


    public void StartSE()
    {
        source.PlayOneShot(startSE);
    }

    public void FireSE()
    {
        source.PlayOneShot(fireSE);
    }

    public void TurnSE()
    {
        source.PlayOneShot(turnSE);
    }

    public void TapCardSE()
    {
        source.PlayOneShot(tapCardSE);
    }

    public void TapCardCancelSE()
    {
        source.PlayOneShot(tapCardCancelSE);
    }


    public void PowerUpSE()
    {
        source.PlayOneShot(powerUpSE);
    }

    public void CookSE()
    {
        source.PlayOneShot(cookSE);
    }

    public void CookEndSE()
    {
        source.PlayOneShot(cookEndSE);
    }

}
