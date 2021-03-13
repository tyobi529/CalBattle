using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI costText;

    //[SerializeField] GlowImage glowIconImage;

    [SerializeField] Image iconImage = null;
    [SerializeField] Image glowIconImage = null;
    //float glowIntensive;

    [SerializeField] Image costBG;


    [SerializeField] GameObject costObj;

    //料理
    //[SerializeField] GlowImage dishIconImage;
    [SerializeField] TextMeshProUGUI nameText;
    //[SerializeField] GameObject nameObj;
    //完成料理
    [SerializeField] Image dishBGImage = null;
    //0:青
    //1:ピンク
    [SerializeField] Sprite[] dishBGSprite = new Sprite[2];


    //レア素材
    [SerializeField] GameObject rareEffectObj;
    //Bad素材
    [SerializeField] GameObject badEffectObj;



    [SerializeField] Animator animator;



    


    //手札の食材の見た目
    public void SetCard(CardModel cardModel)
    {
        //保存
        //glowIntensive = glowIconImage.glowIntensitive;

        //glowIconImage.glowIntensitive = 0f;

        

        

        costText.text = cardModel.cost.ToString();


        if (cardModel.kind == KIND.RED)
        {
            costBG.color = new Color(248f / 255f, 132f / 255f, 132f / 255f, 1f);
            //glowIconImage.glowColor = frameColor[0];
            
        }
        else if (cardModel.kind == KIND.YELLOW)
        {
            costBG.color = new Color(238f / 255f, 248f / 255f, 132f / 255f, 1f);
            //glowIconImage.glowColor = frameColor[1];
        }
        else if (cardModel.kind == KIND.GREEN)
        {
            costBG.color = new Color(132f / 255f, 248f / 255f, 141f / 255f, 1f);
            //glowIconImage.glowColor = frameColor[2];
        }

        iconImage.sprite = cardModel.icon;
        glowIconImage.sprite = cardModel.glowIcon;

        //ChangeAnimation(true);
        

        //glowIconImage.sprite = cardModel.icon;

        //テスト
        //iconImage.sprite = cardModel.icon;

        switch (cardModel.condition)
        {
            case 0:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(true);
                break;
            case 1:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(false);
                break;
            case 2:
                rareEffectObj.SetActive(true);
                badEffectObj.SetActive(false);
                break;
        }


        //isGlow = true;

    }

    public void ChangeAnimation(bool isMyTurn)
    {
        animator.SetBool("isMyTurn", isMyTurn);

        if (isMyTurn)
        {
            //glowIconImage.glowIntensitive = glowIntensive;

            iconImage.enabled = false;
            glowIconImage.enabled = true;


        }
        else
        {
            //glowIconImage.glowIntensitive = 0f;

            iconImage.enabled = true;
            glowIconImage.enabled = false;

        }
    }

    public void Refresh(CardModel cardModel)
    {

        costText.text = cardModel.cost.ToString();

        switch (cardModel.condition)
        {
            case 0:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(true);
                break;
            case 1:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(false);
                break;
            case 2:
                rareEffectObj.SetActive(true);
                badEffectObj.SetActive(false);
                break;
        }
        
        

    }


    public void SetCookingCard(CardModel cardModel)
    {

        //glowIconImage.sprite = cardModel.icon;
        //glowIconImage.glowIntensitive = 0f;

        iconImage.sprite = cardModel.icon;
        glowIconImage.sprite = cardModel.glowIcon;


        switch (cardModel.condition)
        {
            case 0:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(true);
                break;
            case 1:
                rareEffectObj.SetActive(false);
                badEffectObj.SetActive(false);
                break;
            case 2:
                rareEffectObj.SetActive(true);
                badEffectObj.SetActive(false);
                break;
        }

        costObj.SetActive(false);

        //isGlow = false;
    }

    //選択するカード
    public void SetDishCard(CardModel cardModel, int playerIndex)
    {
        //dishIconImage.sprite = cardModel.icon;

        iconImage.sprite = cardModel.icon;


        nameText.text = cardModel.name;

        dishBGImage.sprite = dishBGSprite[GameManager.instance.playerColor[playerIndex]];
    }









    

}
