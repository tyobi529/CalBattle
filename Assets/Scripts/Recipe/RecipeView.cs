using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeView : MonoBehaviour
{
    [SerializeField] Text noText;
    [SerializeField] Text nameText;
    [SerializeField] Image dishIcon;
    [SerializeField] Text calText;
    [SerializeField] Text effectText;
    //[SerializeField] Text rareEffectText;
    //[SerializeField] Text supportEffectText;
    [SerializeField] Text mameText;


    [SerializeField] Image[] ingredientImage = new Image[2];

    //未作成
    [SerializeField] Sprite questionSprite;


    [SerializeField] DishInformation dishInformation;

    [SerializeField] Color rareTextColor;


    public void SetRecipe(int cardID)
    {
        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        CardEntity cardEntity = CardData.instance.dishCardEntity[cardID];

        noText.text = "No. " + (cardID + 1);


        //作成済みかどうか
        int cooked = PlayerPrefs.GetInt("COOKED_" + cardID, 0);

        if (cooked == 1)
        {
            nameText.text = cardEntity.name;
            dishIcon.sprite = cardEntity.icon;
            calText.text = cardEntity.cal + " Kcal";


            string[] effectText = new string[3] { null, null, null };
            effectText = dishInformation.DishEffectText(cardID, true);


            this.effectText.text = effectText[0] + $"<color=#EE3CEE>{effectText[1]}</color>" + $"<color=#000000>{effectText[2]}</color>";


            mameText.text = dishInformation.DishInformationText(cardID);
        }
        else
        {
            nameText.text = "?????";
            dishIcon.sprite = questionSprite;
            calText.text = "???" + " Kcal";

            //effectText.alignment = (TextAnchor)TextAlignment.Center;
            effectText.fontSize = 40;
            effectText.text = "??????????";
            //rareEffectText.text = "";
            //supportEffectText.text = "";

            mameText.alignment = (TextAnchor)TextAlignment.Center;
            mameText.fontSize = 40;
            mameText.text = "????????????????";
        }


        for (int i = 0; i < 2; i++)
        {
            //CardEntity ingredientCardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardEntity.ingredientCardID[i]);
            CardEntity ingredientCardEntity = CardData.instance.ingCardEntity[cardEntity.ingredientCardID[i]];
            ingredientImage[i].sprite = ingredientCardEntity.icon;
        }
    }

    
}
