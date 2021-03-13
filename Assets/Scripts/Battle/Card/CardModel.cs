using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータとその処理
public class CardModel
{
    public string name { get; set; }

    public int cal { get; set; }

    public int cost { get; set; }

    //public bool isRare { get; set; }
    public int condition { get; set; } = 1;

    public Sprite icon { get; set; }
    public Sprite glowIcon { get; set; }


    public KIND kind { get; set; }


    public int cardID { get; set; }

    public int[] partnerID { get; set; }
    public int[] specialMixID { get; set; }


    public bool isSelected { get; set; }

    public bool isStrong { get; set; } = false;

    public string effectText;


    public int index { get; set; } = -1;


    public int[] ingredientCardID { get; set; }


    public CardModel(bool isDish, int cardID, int cost, int condition)
    {
        CardEntity cardEntity = null;

        if (isDish)
        {
            //cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
            cardEntity = CardData.instance.dishCardEntity[cardID];

            this.ingredientCardID = cardEntity.ingredientCardID;
        }
        else
        {
            //cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
            cardEntity = CardData.instance.ingCardEntity[cardID];

            this.cost = cost;
            this.condition = condition;
            partnerID = cardEntity.partnerID;
            specialMixID = cardEntity.specialMixID;
        }


        this.cardID = cardID;

        name = cardEntity.name;

        cal = cardEntity.cal;

        this.kind = cardEntity.kind;

        icon = cardEntity.icon;
        glowIcon = cardEntity.glowIcon;

        isSelected = false;

    }


}
