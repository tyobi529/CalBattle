using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    //ヒエラルキーからD&Dしておく
    [SerializeField] AudioClip battleBGM = null;
    [SerializeField] AudioClip menuBGM = null;
    [SerializeField] AudioClip quizBGM = null;
    [SerializeField] AudioClip recipeBGM = null;
    [SerializeField] AudioClip resultBGM = null;
    [SerializeField] AudioClip speedBGM = null;
    [SerializeField] AudioClip timerBGM = null;
    [SerializeField] AudioClip titleBGM = null;


    //使用するAudioSource
    public AudioSource source { get; set; } = null;

    //１つ前のシーン名
    //private string beforeScene = "Menu";

    bool isFadeIn = false;
    bool isFadeOut = false;
    bool isOnlyFadeOut = false;
    float fadeTime = 0f;
    [SerializeField] float fadeDuration = 0f;

    AudioClip currentBGM = null;
    AudioClip nextBGM = null;


    float volume = 0f;

    public bool isTutorial { get; set; } = false;

    string currentSceneName = null;
    string prevSceneName = null;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //使用するAudioSource取得
        source = GetComponent<AudioSource>();



        currentSceneName = SceneManager.GetActiveScene().name;

        currentBGM = SetBGMSource(currentSceneName);
        //source.clip = currentBGM;
    }


    // Use this for initialization
    void Start()
    {
        ChangeVolume();

        //if (currentBGM != battleBGM)
        //{
        //    source.Play();
        //}


        //シーンが切り替わった時に呼ばれるメソッドを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }


    public void SetAudioClip(int bgmID, AudioClip audioClip)
    {
        switch (bgmID)
        {
            case 0:
                battleBGM = audioClip;
                break;
            case 1:
                menuBGM = audioClip;
                break;
            case 2:
                quizBGM = audioClip;
                break;
            case 3:
                recipeBGM = audioClip;
                break;
            case 4:
                resultBGM = audioClip;
                break;
            case 5:
                speedBGM = audioClip;
                break;
            case 6:
                timerBGM = audioClip;
                break;
            case 7:
                titleBGM = audioClip;
                break;

        }
        //battleBGM = audioClip[0];
        //menuBGM = audioClip[1];
        //quizBGM = audioClip[2];
        //recipeBGM = audioClip[3];
        //resultBGM = audioClip[4];
        //speedBGM = audioClip[5];
        //timerBGM = audioClip[6];
        //titleBGM = audioClip[7];
    }

    void Update()
    {
        if (isFadeOut)
        {
            fadeTime += Time.deltaTime;
            source.volume = (float)(volume - fadeTime / fadeDuration);
            if (fadeTime > fadeDuration)
            {
                fadeTime = 0f;
                isFadeOut = false;
                isFadeIn = true;
                //source.volume = 0f;
                source.Stop();
                source.clip = nextBGM;
                currentBGM = nextBGM;
                source.Play();
            }
        }

        if (isFadeIn)
        {
            fadeTime += Time.deltaTime;
            source.volume = (float)(volume * fadeTime / fadeDuration);
            if (fadeTime > fadeDuration)
            {
                Debug.Log("aaa");
                fadeTime = 0f;
                isFadeIn = false;
                source.volume = volume;
            }
        }


        if (isOnlyFadeOut)
        {
            fadeTime += Time.deltaTime;
            source.volume = (float)(volume - fadeTime / fadeDuration);
            if (fadeTime > fadeDuration)
            {
                fadeTime = 0f;
                isOnlyFadeOut = false;
                //source.volume = 0f;
                source.Stop();
                source.clip = nextBGM;
                currentBGM = nextBGM;
                //source.Play();
            }
        }

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

        volume = PlayerPrefs.GetFloat("VOLUME", 0f);
        source.volume = volume;

        Debug.Log("sound" + sound);


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

    AudioClip SetBGMSource(string sceneName)
    {
        AudioClip audioClip = null;

        switch (sceneName)
        {
            case "Title":
                audioClip = titleBGM;
                break;
            case "Menu":
                audioClip = menuBGM;
                break;
            case "Battle":
                audioClip = battleBGM;
                break;
            case "Recipe":
                audioClip = recipeBGM;
                break;
            case "Speed":
                audioClip = speedBGM;
                break;
            case "Quiz":
                audioClip = quizBGM;
                break;
            case "Name":
                audioClip = titleBGM;
                break;
            case "Result":
                audioClip = resultBGM;
                break;
            case "Timer":
                audioClip = timerBGM;
                break;
        }

        return audioClip;

    }


    //シーンが切り替わった時に呼ばれるメソッド
    void OnActiveSceneChanged(Scene scene, Scene nextScene)
    {
        nextBGM = SetBGMSource(nextScene.name);


        //前のシーン名を入れる
        prevSceneName = currentSceneName;
        currentSceneName = nextScene.name;

        //切り替わってもBGMが一緒
        if (currentBGM == nextBGM)
        {
            return;
        }



        Debug.Log("前のシーン" + prevSceneName);
        Debug.Log("前のシーン" + currentSceneName);

        //if (prevSceneName == "Name" && currentSceneName == "Battle")
        //{
        //    isTutorial = true;
        //}
        //else
        //{
        //    isTutorial = false;
        //}


        if (currentSceneName == "Battle")
        {
            source.volume = 0f;
            isOnlyFadeOut = true;
        }
        else
        {
            isFadeOut = true;
        }



    }


    public void StartBGM(string bgmName)
    {
        Debug.Log(bgmName + "開始");

        nextBGM = SetBGMSource(bgmName);

        Debug.Log(nextBGM);

        source.clip = nextBGM;
        currentBGM = nextBGM;
        //source.volume = PlayerPrefs.GetInt("SOUND", 0) * PlayerPrefs.GetFloat("VOLUME", 0f);
        //source.volume = PlayerPrefs.GetFloat("VOLUME", 0f);
        ChangeVolume();
        source.Play();
    }

    public void FadeInOutBGM(string bgmName)
    {
        nextBGM = SetBGMSource(bgmName);

        //source.clip = nextBGM;
        //source.Play();

        isFadeOut = true;
    }


    public void FadeInBGM(string bgmName)
    {
        nextBGM = SetBGMSource(bgmName);

        source.clip = nextBGM;
        currentBGM = nextBGM;

        source.volume = 0f;
        source.Play();

        isFadeIn = true;
    }
}