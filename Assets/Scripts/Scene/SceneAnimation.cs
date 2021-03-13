using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneAnimation : MonoBehaviour
{
    [SerializeField] float baseWidth = 9.0f;
    [SerializeField] float baseHeight = 16.0f;

    static public SceneAnimation instance;

    
    [SerializeField] Image[] sceneObjImage = new Image[3] { null, null, null };

    [SerializeField] GameObject smartPhone = null;

    [SerializeField] GameObject normalObj = null;
    [SerializeField] GameObject rotateObj = null;

    [SerializeField] Image rotateTextImage = null;
    [SerializeField] Sprite[] rotateSprite = new Sprite[2] { null, null };
    [SerializeField] GameObject[] yajirushi = new GameObject[2] { null, null };


    [SerializeField] Color[] forkColor = new Color[3];

    [SerializeField] Camera camera = null;

    string preSceneName = "Title";
    string nextSceneName = null;




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

        CameraResize();
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

    }




    public void SceneMovement()
    {
        if (nextSceneName == "Recipe" || nextSceneName == "Speed")
        {
            RotateAnimation(0);

            
        }
        else if (preSceneName == "Recipe" || preSceneName == "Speed")
        {
            RotateAnimation(1);
        }
        else
        {
            NormalAnimation();
        }

    }


    public void NormalAnimation()
    {
        //FadeManager.FadeIn();

 
        DOVirtual.DelayedCall(0f, () =>
        {
            DOTween.ToAlpha(
      () => sceneObjImage[0].color,
      color => sceneObjImage[0].color = color,
      1f, // 目標値
      0.3f// 所要時間
      );

            sceneObjImage[0].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

        });

        DOVirtual.DelayedCall(0.5f, () =>
        {
            DOTween.ToAlpha(
      () => sceneObjImage[1].color,
      color => sceneObjImage[1].color = color,
      1f, // 目標値
      0.3f// 所要時間
      );

            sceneObjImage[1].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

        });

        DOVirtual.DelayedCall(1f, () =>
        {
            DOTween.ToAlpha(
      () => sceneObjImage[2].color,
      color => sceneObjImage[2].color = color,
      1f, // 目標値
      0.3f// 所要時間
      );

            sceneObjImage[2].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

        }).OnComplete(() =>
        {

            transform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() => {

                transform.GetComponent<CanvasGroup>().blocksRaycasts = false;

                if (SceneManager.GetActiveScene().name == "Battle")
               {
                    GameManager.instance.StartBattle();
                    
                }
            });
        });


    }


    public void RotateAnimation(int num)
    {
        if (num == 0)
        {
            //smartPhone.transform.DOLocalRotate(new Vector3(0, 0, -90), 1f).SetDelay(0.5f).OnComplete(() => {
            smartPhone.transform.DOLocalRotate(new Vector3(0, 0, 90), 1f).SetDelay(0.5f).OnComplete(() => {


                transform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                });


            });
        }
        else
        {
            smartPhone.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetDelay(0.5f).OnComplete(() => {

                transform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                });
            });
        }

        

    }


    //シーンが切り替わった時に呼ばれるメソッド
    void OnActiveSceneChanged(Scene scene, Scene nextScene)
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GetComponent<Canvas>().worldCamera = camera;

        

        preSceneName = nextSceneName;
        nextSceneName = nextScene.name;


        
        

        //if (nextSceneName == "Title" || nextSceneName == "Battle" || nextSceneName == "Name" || nextSceneName == "Quiz")
        //{
        //    //FadeManager.FadeIn(0);
        //    CameraResize();
        //}
        //else
        //{
        //    FadeManager.FadeIn(1.5f);
        //}


        


        transform.GetComponent<CanvasGroup>().alpha = 1;
        transform.GetComponent<CanvasGroup>().blocksRaycasts = true;


        if (nextSceneName == "Recipe" || nextSceneName == "Speed")
        {
            FadeManager.FadeIn(2f);


            Screen.orientation = ScreenOrientation.Landscape;


            normalObj.SetActive(false);
            rotateObj.SetActive(true);


            rotateObj.transform.rotation = Quaternion.Euler(0, 0, 90);
            smartPhone.transform.rotation = Quaternion.Euler(0, 0, 90);
            rotateTextImage.sprite = rotateSprite[0];


            for (int i = 0; i < 2; i++)
            {
                yajirushi[i].transform.localScale = new Vector3(-1f, 1f, 1f);

            }



        }
        else if (preSceneName == "Recipe" || preSceneName == "Speed")
        {

            FadeManager.FadeIn(2f);

            Screen.orientation = ScreenOrientation.Portrait;


            normalObj.SetActive(false);
            rotateObj.SetActive(true);

            rotateObj.transform.rotation = Quaternion.Euler(0, 0, -90);
            smartPhone.transform.rotation = Quaternion.Euler(0, 0, 0);
            rotateTextImage.sprite = rotateSprite[1];


            for (int i = 0; i < 2; i++)
            {
                yajirushi[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }



        }
        else
        {
            FadeManager.FadeIn(0);


            normalObj.SetActive(true);
            rotateObj.SetActive(false);
            //NormalAnimation();

            int colorNum = Random.Range(0, 3);


            for (int i = 0; i < 3; i++)
            {
                Color color = forkColor[colorNum];
                color.a = 0f;

                sceneObjImage[i].color = color;
                sceneObjImage[i].transform.rotation = Quaternion.Euler(0, 0, 180);
                sceneObjImage[i].gameObject.SetActive(true);
            }

            //yajirushi[0].transform.rotation = Quaternion.Euler(0, 0, 90);
            //yajirushi[1].transform.rotation = Quaternion.Euler(0, 0, -90);

            //CameraResize();
        }


        if (nextSceneName == "Title" || nextSceneName == "Battle" || nextSceneName == "Name" || nextSceneName == "Quiz")
        {
            //FadeManager.FadeIn(0);
            CameraResize();
        }


    }

    void CameraResize()
    {
        // アスペクト比固定
        var scale = Mathf.Min(Screen.height / this.baseHeight, Screen.width / this.baseWidth);
        var width = (this.baseWidth * scale) / Screen.width;
        var height = (this.baseHeight * scale) / Screen.height;
        this.camera.rect = new Rect((1.0f - width) * 0.5f, (1.0f - height) * 0.5f, width, height);


    }



}
