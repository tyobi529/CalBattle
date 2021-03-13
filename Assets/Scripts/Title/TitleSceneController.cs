using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Coffee.UIEffects;

public class TitleSceneController : MonoBehaviour, IPointerClickHandler
{
    int tutorial = 0;

    [SerializeField] CardGenerator cardGenerator = null;

    [SerializeField] GameObject[] titleTextObj = new GameObject[4] { null, null, null, null };
    [SerializeField] float hopDuration = 3f;
    float time = 0f;

    [SerializeField] TextMeshProUGUI startText = null;

    [SerializeField] GameObject[] card = new GameObject[2] { null, null };

    [SerializeField] GameObject dish = null;

    [SerializeField] GameObject moku = null;
    
    Color _color;
    float alpha_Sin = 0f;
    [SerializeField] float speed = 0f;

    [SerializeField] GameObject tapEffect = null;

    bool isTap = false;

    [SerializeField] RectTransform parentRectTransform;


    [SerializeField] GameObject tapTextObj = null;


    [SerializeField] GameObject kakeruObj = null;


    bool isSecret = false;
    int secretCount = 0;

    [SerializeField] Color goldColor;

    [SerializeField] Image[] syokki = new Image[3] { null, null, null };

    private void Awake()
    {
        tutorial = PlayerPrefs.GetInt("TUTORIAL", 0);
        
        //初回起動
        if (tutorial == 0)
        {
            PlayerPrefs.SetInt("SOUND", 1);
            PlayerPrefs.SetFloat("VOLUME", 0.5f);
            PlayerPrefs.Save();
        }


    }

    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            if (PlayerPrefs.GetInt("MISSION_" + i, 0) == 0)
            {
                return;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            syokki[i].color = goldColor;
        }

        syokki[0].transform.GetComponent<UIShiny>().enabled = true;

    }



    public void ShowTitle()
    {
        //カード決定
        int num = Random.Range(0, 3);
        KIND[] kind = new KIND[2];

        switch (num)
        {
            case 0:
                kind[0] = KIND.RED;
                kind[1] = KIND.YELLOW;
                break;
            case 1:
                kind[0] = KIND.YELLOW;
                kind[1] = KIND.GREEN;
                break;
            case 2:
                kind[0] = KIND.GREEN;
                kind[1] = KIND.RED;
                break;
        }

        for (int i = 0; i < 2; i++)
        {
            int cardID = -1;
            if (kind[i] == KIND.RED)
            {
                cardID = Random.Range(0, 3);
            }
            else if (kind[i] == KIND.YELLOW)
            {
                cardID = Random.Range(3, 6);
            }
            else
            {
                cardID = Random.Range(6, 9);
            }

            card[i].GetComponent<CardController>().TitleCardInit(cardID);
            card[i].GetComponent<Image>().sprite = card[i].GetComponent<CardController>().model.icon;


        }

        int dishID = cardGenerator.SpecialMix(card[0].GetComponent<CardController>(), card[1].GetComponent<CardController>());

        dish.GetComponent<Image>().sprite = CardData.instance.dishCardEntity[dishID].icon;

        time = hopDuration;

        _color = startText.color;
    }


    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if (time > hopDuration)
        {
            time = 0f;
            for (int i = 0; i < 4; i++)
            {
                titleTextObj[i].transform.DOLocalJump(titleTextObj[i].transform.localPosition, 50, 1, 0.3f).SetDelay(i * 0.3f);


            }
        }


        alpha_Sin = Mathf.Sin(Time.time * speed) / 2 + 0.5f;

        _color.a = alpha_Sin;

        startText.color = _color;




    }

    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {        

        if (isTap)
        {
            return;
        }

        isTap = true;
        Vector2 screenPos = GetLocalPosition(eventData.position);

        tapEffect.transform.localPosition = screenPos;
        tapEffect.SetActive(true);

        tapTextObj.transform.DOScale(new Vector2(1.2f, 1.2f), 0.8f);
        tapTextObj.GetComponent<CanvasGroup>().DOFade(0f, 0.8f);


        moku.transform.localScale = new Vector3(0f, 0f, 0f);
        moku.SetActive(true);

        moku.transform.DOScale(1f, 1f);
        card[0].transform.DOMove(new Vector2(0f, 0f), 1f);
        card[1].transform.DOMove(new Vector2(0f, 0f), 1f).OnComplete(() =>
        {
            dish.transform.localScale = new Vector3(0f, 0f, 0f);
            dish.SetActive(true);
            dish.transform.DOScale(1.3f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                moku.SetActive(false);
                card[0].SetActive(false);
                card[1].SetActive(false);
                kakeruObj.SetActive(false);

                dish.transform.DOScale(1f, 0.2f).SetDelay(0.4f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        if (DOTween.instance != null)
                        {
                            //DOTween.KillAll();
                            DOTween.CompleteAll();
                        }


                        if (isSecret)
                        {
                            Secret();
                        }
                        else
                        {
                            //名前の入力がまだならチュートリアルへ
                            if (tutorial == 0)
                            {
                                FadeManager.FadeOut(6);
                            }
                            else
                            {
                                FadeManager.FadeOut(1);
                            }
                        }


                    });




                });

            });
        });

    }

    public void OnSoundButton()
    {
        secretCount++;

        //13回タップで全開放
        if (secretCount == 13)
        {
            Debug.Log("開放");
            isSecret = true;
        }
        else
        {
            isSecret = false;
        }
    }

    void Secret()
    {
        PlayerPrefs.SetInt("TUTORIAL", 1);


        for (int i = 0; i < 16; i++)
        {
            PlayerPrefs.SetInt("MISSION_" + i, 1);

        }

        for (int i = 0; i < 27; i++)
        {
            PlayerPrefs.SetInt("COOKED_" + i, 1);
        }

        PlayerPrefs.Save();

        FadeManager.FadeOut(0);
    }
}
