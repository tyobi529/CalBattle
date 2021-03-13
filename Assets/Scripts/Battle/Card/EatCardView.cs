using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EatCardView : MonoBehaviour
{

    //[SerializeField] TextMeshProUGUI[] effectText = new TextMeshProUGUI[2];
    [SerializeField] TextMeshProUGUI effectText = null;

    //料理    
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI calText;


    //[SerializeField] Image nutrientColor;


    [SerializeField] Image iconImage;
    //[SerializeField] GlowImage glowIconImage;

    [SerializeField] GameObject rareEffectObj;



    [SerializeField] FrameGenerator frameGenerator;

    [SerializeField] Image BGImage;

    IEnumerator blinkCoroutine;

    [SerializeField] Image errorMessageImage;
    [SerializeField] Sprite[] errorMessageSprite = new Sprite[2];


    [SerializeField] DishInformation dishInformation = null;

    //[SerializeField] Color rareColor;

    public EatCardView()
    {
        blinkCoroutine = BlinkBG();

    }





    public void SetEatCard(CardModel cardModel)
    {
        GamePlayerManager player = GameObject.Find("Player").GetComponent<GamePlayerManager>();


        nameText.text = cardModel.name;
        calText.text = cardModel.cal + "Kcal";        



        //料理
        if (cardModel.kind == KIND.DISH) 
        {
            //string[] effectText = DecideDishEffectText(cardModel.cardID, cardModel.isStrong);

          

            if (player.darkCount > 0)
            {
                nameText.text = "?????";
                calText.text = "???Kcal";


                //effectText.fontSize = 30;

                this.effectText.text = "\"暗闇\"で料理が見えない";

            }
            else
            {                
                iconImage.sprite = cardModel.icon;

                string[] effectText = new string[3] { null, null, null };
                effectText = dishInformation.DishEffectText(cardModel.cardID, cardModel.isStrong);

                this.effectText.text = effectText[0] + $"<color=#EE3CEE>{effectText[1]}</color>" + $"<color=#000000>{effectText[2]}</color>";

                //for (int i = 0; i < 2; i++)
                //{
                //    this.effectText[i].text = effectText[i];
                //}


            }


        }
        else
        {
            effectText.fontSize = 30;
            effectText.text = "料理コスト +" + cardModel.cost;
            //effectText[1].text = "";

            iconImage.sprite = cardModel.icon;
        }


        rareEffectObj.SetActive(cardModel.isStrong);



    }


    IEnumerator BlinkBG()
    {
        float alpha_Sin = 0f;
        Color _color = BGImage.color;

        while (true)
        {
            alpha_Sin = Mathf.Sin(Time.time * 5f) / 2f + 0.5f;
            _color.b = alpha_Sin;

            //blinkObjectImage.color = _color;
            BGImage.color = _color;

            yield return new WaitForSeconds(0.01f);

        }

    }

    public void StartBlinking()
    {
        frameGenerator.enabled = true;
        StartCoroutine(blinkCoroutine);

    }

    public void StopBlinking()
    {
        frameGenerator.enabled = false;
        StopCoroutine(blinkCoroutine);

        BGImage.color = Color.white;
    }


    //0:コスト不足
    //1:料理不可
    //2:なし
    public void SetErrorMessage(int num)
    {
        if (num == 2)
        {
            errorMessageImage.transform.gameObject.SetActive(false);
        }
        else
        {
            errorMessageImage.transform.gameObject.SetActive(true);
            errorMessageImage.sprite = errorMessageSprite[num];
        }

    }


   




}
