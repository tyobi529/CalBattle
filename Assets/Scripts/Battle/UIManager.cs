using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultObj;
    [SerializeField] Image resultTextImage = null;
    [SerializeField] Sprite[] resultTextSprite = new Sprite[2] { null, null };
    [SerializeField] Sprite[] systemTextSprite = new Sprite[2] { null, null };
    [SerializeField] GameObject restartButtonObj;
    [SerializeField] GameObject quitButtonObj;
    [SerializeField] GameObject roadingObj;
    //[SerializeField] GameObject surrenderTextObj = null;


    [SerializeField] TextMeshProUGUI[] nameText = new TextMeshProUGUI[2];
    [SerializeField] TextMeshProUGUI[] startNameText = new TextMeshProUGUI[2];
    [SerializeField] TextMeshProUGUI[] cookingNameText = new TextMeshProUGUI[2];

    
    [SerializeField] Text[] hpText = new Text[2];


    int[] preHp = new int[2] { 0, 0 };
    [SerializeField] float hpSpeed = 1f;
    [SerializeField] Image[] greenHp = new Image[2] { null, null };
    [SerializeField] Image[] redHp = new Image[2] { null, null };

    private Tween[] redHpTween = new Tween[2] { null, null };

    [SerializeField] Sprite[] costBarSprite = new Sprite[2];

    [SerializeField] GameObject[] costEffectObj = new GameObject[2];      

    [SerializeField] ImageNumber[] costText = new ImageNumber[2];

    int maxCost = 0;
    int[] preCost = new int[2] { 0, 0 };
    [SerializeField] float costSpeed = 0.2f;

    [SerializeField] Image[] costImage = new Image[2] { null, null };


    [SerializeField] TextMeshProUGUI turnText;


    [SerializeField] GameObject[] conditionObj = new GameObject[2];

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];



    [SerializeField] ImageNumber[] damageText = new ImageNumber[2];
    [SerializeField] ImageNumber[] healText = new ImageNumber[2];




    [SerializeField] GameObject tapPrefab;

    [SerializeField] Setting setting;

    [SerializeField] CharacterController characterController;

    private int maxHp;

    //料理を置く場所
    [SerializeField] Image[] dishImage = new Image[2];

    [SerializeField] Transform popUpIngredientsTransform = null;

    [SerializeField] GameObject pauseButtonObj = null;


    public Image warningBGImage = null;

    public IEnumerator warningCoroutine;

    [SerializeField] PauseController pauseController = null;

    [SerializeField] Color largeDamageColor;
    [SerializeField] Color poisonDamageColor;



    [SerializeField] GameObject timer = null;
    [SerializeField] Image timerImage = null;
    [SerializeField] Transform timerHariTransform = null;    
    float maxTime = 0f;
    //float timeLimit { get; set; } = 0f


    public GameObject disconnectPanel = null;


    //シングルトン化（どこからでもアクセスできるようにする）
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }


    private void Start()
    {
        maxHp = setting.maxHp;
        maxCost = setting.maxCost;

        //for (int i = 0; i < 2; i++)
        //{
        //    preHp[i] = setting.defaultHp;
        //    preCost[i] = setting.defaultCost;
        //}

        

        //ポーズの食事セット
        SetIngredientContents();

        warningCoroutine = Warning();

        for (int i = 0; i < 2; i++)
        {
            hpText[i].gameObject.SetActive(setting.showHpValue);
        }

        maxTime = setting.timeLimit;

    }

    private void Update()
    {
        //タップした場所にエフェクト
        if (Input.GetMouseButtonUp(0))
        {
            if (pauseController.pause)
            {
                return;
            }
            GameObject tapObj = Instantiate(tapPrefab, this.transform, false);
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tapObj.transform.position = position;
            tapObj.GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetName(string[] playerName)
    {
        for (int i = 0; i < 2; i++)
        {
            nameText[i].text = playerName[i];
            startNameText[i].text = playerName[i];
            cookingNameText[i].text = playerName[i];
        }
    }

    public void ShowCost()
    {

        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(ShowCost(i));
        }

    }



    IEnumerator ShowCost(int playerIndex)
    {

        int fromCost = preCost[playerIndex];
        int toCost = player[playerIndex].cost;

        int addCost = toCost - fromCost;

        if (addCost != 0)
        {
            float valueFrom = (float)fromCost / (float)maxCost;
            float valueTo = (float)toCost / (float)maxCost;

                        DOTween.To(
            () => valueFrom,
            x =>
            {
                costImage[playerIndex].fillAmount = x;
            },
            valueTo,
            costSpeed * Mathf.Abs(addCost)
            ).SetEase(Ease.Linear);

            //増える
            if (addCost > 0)
            {

                for (int i = fromCost; i < toCost; i++)
                {
                    yield return new WaitForSeconds(costSpeed);

                    int currentCost = i + 1;

                    costText[playerIndex].Setno(currentCost);

                    if (currentCost == 3)
                    {
                        costEffectObj[playerIndex].SetActive(true);  
                        costImage[playerIndex].sprite = costBarSprite[1];
                    }                    
                }

            }
            //減る
            else
            {
                for (int i = fromCost; i > toCost; i--)
                {
                    yield return new WaitForSeconds(costSpeed);

                    int currentCost = i - 1;

                    costText[playerIndex].Setno(currentCost);

                    Debug.Log(currentCost);

                    if (currentCost == 2)
                    {
                        costEffectObj[playerIndex].SetActive(false);
                        costImage[playerIndex].sprite = costBarSprite[0];
                    }
                }
            }


            preCost[playerIndex] = toCost;
        }


        


    }





    public void UpdateTime(float timeCount)
    {


        if (timeCount < 0)
        {
            return;
        }

        timerImage.fillAmount = timeCount / maxTime;
        float angle = 360f * timeCount / maxTime;
        timerHariTransform.rotation = Quaternion.Euler(0, 0, -angle);
    }


    public void ShowTimer(bool isShow)
    {
        //for (int i = 0; i < 2; i++)
        //{
        //    timerBar[i].gameObject.SetActive(isShow);
        //}

        timer.SetActive(isShow);
        //timeLimit = GameManager.instance.timeLimit;
    }

    public void ShowHP()
    {

        if (preHp[0] != player[0].hp)
        {
            float valueFrom = (float)preHp[0] / (float)maxHp;
            //float valueTo = 0f;
            float valueTo = (float)player[0].hp / (float)maxHp;

            if (0f < valueTo && valueTo < 0.02f)
            {
                valueTo = 0.02f;
            }


            greenHp[0].fillAmount = valueTo;

            if (redHpTween[0] != null)
            {
                redHpTween[0].Kill();
            }

            // 赤ゲージ減少
            redHpTween[0] = DOTween.To(
                () => valueFrom,
                x =>
                {
                    redHp[0].fillAmount = x;
                },
                valueTo,
                hpSpeed
            ).SetDelay(0.3f);


            preHp[0] = player[0].hp;
        }

        if (preHp[1] != player[1].hp)
        {

            float valueFrom = (float)preHp[1] / (float)maxHp;
            //float valueTo = 0f;
            float valueTo = (float)player[1].hp / (float)maxHp;

            if (0f < valueTo && valueTo < 0.02f)
            {
                valueTo = 0.02f;
            }

            //float valueFrom = (float)preHp[1] / (float)maxHp;
            //float valueTo = 0f;


            //if (player[1].hp > 0)
            //{
            //    valueTo = (float)player[1].hp / (float)maxHp;
            //}


            ////0.02以下だと見えないため
            //if (valueTo < 0.02f)
            //{
            //    valueTo = 0.02f;
            //}

            greenHp[1].fillAmount = valueTo;

            if (redHpTween[1] != null)
            {
                redHpTween[1].Kill();
            }

            // 赤ゲージ減少
            redHpTween[1] = DOTween.To(
                () => valueFrom,
                x =>
                {
                    redHp[1].fillAmount = x;
                },
                valueTo,
                hpSpeed
            ).SetDelay(0.3f);


            preHp[1] = player[1].hp;
        }



    }


    //完成した料理を示す
    public void ShowDish(Sprite[] dishSprite)
    {
        for (int i = 0; i < 2; i++)
        {
            dishImage[i].enabled = true;
            dishImage[i].sprite = dishSprite[i];
        }
    }

    public void HideDish()
    {
        for (int i = 0; i < 2; i++)
        {
            dishImage[i].enabled = false;
        }
    }
    


    public void ShowLeave()
    {
        //if (surrender)
        //{
        //    surrenderTextObj.SetActive(true);                        
        //}
        //else
        //{
        //    resultTextImage.sprite = systemTextSprite[0];
        //}

        if (DOTween.instance != null)
        {
            DOTween.KillAll();
        }

        resultTextImage.transform.localPosition = new Vector2(0f, 0f);

        resultTextImage.transform.DOLocalMoveY(20f, 0.4f)
  .SetRelative(true)
  .SetEase(Ease.OutQuad)
  .SetLoops(-1, LoopType.Yoyo);

        resultTextImage.sprite = systemTextSprite[0];

        roadingObj.SetActive(false);
        restartButtonObj.SetActive(false);
        quitButtonObj.SetActive(false);
    }

    public void ShowResult(bool win)
    {
        resultObj.SetActive(true);
 

        restartButtonObj.GetComponent<Button>().interactable = true;

        roadingObj.SetActive(false);

        if (win)
        {
            resultTextImage.sprite = resultTextSprite[0];

            characterController.ChangeResultSprite(true);
        }
        else
        {
            resultTextImage.sprite = resultTextSprite[1];

            characterController.ChangeResultSprite(false);
        }

        resultTextImage.transform.DOLocalMoveY(20f, 0.4f)
        .SetRelative(true)
        .SetEase(Ease.OutQuad)
        .SetLoops(-1, LoopType.Yoyo);
    }


    public void WaitingRestart()
    {

        if (DOTween.instance != null)
        {
            DOTween.KillAll();
        }
        resultTextImage.transform.localPosition = new Vector2(0f, 0f);
        resultTextImage.sprite = systemTextSprite[1];
        restartButtonObj.GetComponent<Button>().interactable = false;

        //quitButtonObj.transform.position = new Vector3(0f, quitButtonObj.transform.position.y, 0f);
        roadingObj.SetActive(true);
    }


    public void HideResultPanel()
    {
        if (DOTween.instance != null)
        {
            DOTween.KillAll();
        }

        resultTextImage.transform.localPosition = new Vector2(0f, 0f);
        resultObj.SetActive(false);

    }


    //ターンエンドボタン
    public void OnDecideButton()
    {
        GameManager.instance.OnDecideButton();

    }



    public void ShowCondition()
    {
        for (int i = 0; i < 3; i++)
        {
            conditionObj[0].transform.GetChild(i).gameObject.SetActive(false);
            conditionObj[1].transform.GetChild(i).gameObject.SetActive(false);
  
        }

        for (int i = 0; i < 2; i++)
        {
            bool health = true;

            if (player[i].poisonCount > 0)
            {
                //poisonText[i].text = "毒" + player[i].poisonCount;

                for (int j = 0; j < 3; j++)
                {
                    if (!conditionObj[i].transform.GetChild(j).gameObject.activeSelf)
                    {
                        health = false;
                        conditionObj[i].transform.GetChild(j).gameObject.SetActive(true);
                        conditionObj[i].transform.GetChild(j).GetComponent<ConditionView>().ChangeConditionView(0, player[i].poisonCount);
                        characterController.ChangeCharacterColor(player[i].playerIndex, 0);
                        break;
                    }
                }
            }

            if (player[i].darkCount > 0)
            {
                //darkText[i].text = "闇" + player[i].darkCount;

                for (int j = 0; j < 3; j++)
                {
                    if (!conditionObj[i].transform.GetChild(j).gameObject.activeSelf)
                    {
                        health = false;
                        conditionObj[i].transform.GetChild(j).gameObject.SetActive(true);
                        conditionObj[i].transform.GetChild(j).GetComponent<ConditionView>().ChangeConditionView(1, player[i].darkCount);
                        characterController.ChangeCharacterColor(player[i].playerIndex, 1);
                        break;
                    }
                }
            }

            if (player[i].clumsyCount > 0)
            {
                //paralysisText[i].text = "器" + player[i].clumsyCount;

                for (int j = 0; j < 3; j++)
                {
                    if (!conditionObj[i].transform.GetChild(j).gameObject.activeSelf)
                    {
                        health = false;
                        conditionObj[i].transform.GetChild(j).gameObject.SetActive(true);
                        conditionObj[i].transform.GetChild(j).GetComponent<ConditionView>().ChangeConditionView(2, player[i].clumsyCount);
                        characterController.ChangeCharacterColor(player[i].playerIndex, 2);
                        break;
                    }
                }
            }

            if (health)
            {
                characterController.ChangeCharacterColor(player[i].playerIndex, 3);
            }

        }


    }


    
    //300以上→1.3倍
    //100〜300は変動
    public void ShowDamageText(int damageCal, int playerIndex, bool poison)
    {


        float scale = 1f + (damageCal / 100f) * 0.1f;

        damageText[playerIndex].transform.localScale = new Vector3(scale, scale);

        if (playerIndex == 1)
        {
            if (damageCal >= 150)
            {
                damageText[playerIndex].ChangeColor(largeDamageColor);
            }
            else
            {
                damageText[playerIndex].ChangeColor(Color.white);
            }

            if (poison)
            {
                damageText[playerIndex].ChangeColor(poisonDamageColor);
            }
        }
        else
        {
            damageText[playerIndex].ChangeColor(Color.red);

            if (poison)
            {
                damageText[playerIndex].ChangeColor(poisonDamageColor);
            }
        }


        damageText[playerIndex].Setno(damageCal);
        damageText[playerIndex].gameObject.SetActive(true);

  

        ShowHP();
    }


    public void ShowHealText(int healCal, bool isMyTurn)
    {
        if (isMyTurn)
        {
            healText[0].Setno(healCal);
            healText[0].gameObject.SetActive(true);
        }
        else
        {
            healText[1].Setno(healCal);
            healText[1].gameObject.SetActive(true);
        }

        ShowHP();
    }


    public void ShowTurnText(int turnCount)
    {
        turnText.text = "ターン" + turnCount;
    }



    void SetIngredientContents()
    {
        for (int i = 0; i < 9; i++)
        {
            popUpIngredientsTransform.GetChild(i).GetComponent<ContentView>().SetIngredientContent(i);
        }
    }


    public void ShowPauseButton(bool isShow)
    {
        pauseButtonObj.SetActive(isShow);
    }



    IEnumerator Warning()
    {
        float alpha_Sin = 0f;
        Color _color = warningBGImage.color;

        while (true)
        {
            alpha_Sin = Mathf.Sin(Time.time * 5f) / 2f + 0.5f;

            if (alpha_Sin > 65f / 255f)
            {
                alpha_Sin = 65f / 255f;
            }

            _color.a = alpha_Sin;

            //blinkObjectImage.color = _color;
            warningBGImage.color = _color;

            yield return new WaitForSeconds(0.01f);

        }

 
    }


    public void CompleteTween()
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }
    }

}
