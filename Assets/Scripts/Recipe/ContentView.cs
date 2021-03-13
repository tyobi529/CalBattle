using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

public class ContentView : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI noText = null;
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] Image icon = null;
    [SerializeField] Button button = null;

    //作成済み
    [SerializeField] GameObject madeBG = null;
    [SerializeField] Sprite goldSprite = null;
    //作成まだ
    [SerializeField] Sprite questionSprite = null;


    //食材
    [SerializeField] Text ingredientNameText = null;
    [SerializeField] Text effectText = null;
    [SerializeField] Text calText = null;
    [SerializeField] Image frameImage = null;

    int cardID = -1;

    [SerializeField] Color redColor;
    [SerializeField] Color yellowColor;
    [SerializeField] Color greenColor;
    [SerializeField] Color blueColor;



    public void SetDishContent(int cardID, bool isComplete)
    {
        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        CardEntity cardEntity = CardData.instance.dishCardEntity[cardID];

        this.cardID = cardID;
        noText.text = "No. " + (cardID + 1); 

        //作成済みかどうか
        int cooked = PlayerPrefs.GetInt("COOKED_" + cardID, 0);

        if (cooked == 1)
        {
            madeBG.SetActive(true);
            if (isComplete)
            {
                madeBG.GetComponent<Image>().sprite = goldSprite;
            }
            nameText.text = cardEntity.name;
            icon.sprite = cardEntity.icon;
        }
        else
        {
            nameText.text = "??????";
            icon.sprite = questionSprite;
        }

        button.onClick.AddListener(OnDishContent);
    }    

    public void SetIngredientContent(int cardID)
    {
        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
        CardEntity cardEntity = CardData.instance.ingCardEntity[cardID];

        ingredientNameText.text = cardEntity.name;
        icon.sprite = cardEntity.icon;

        if (cardEntity.kind == KIND.RED)
        {
            frameImage.color = redColor;
        }
        else if (cardEntity.kind == KIND.YELLOW)
        {
            frameImage.color = yellowColor;            
        }
        else if (cardEntity.kind == KIND.GREEN)
        {
            frameImage.color = greenColor;
        }

        calText.text = cardEntity.cal + "Kcal";
        effectText.color = blueColor;
        effectText.text = IngredientEffect(cardID);


    }

    public void OnDishContent()
    {
        BookUI bookUI = GameObject.Find("Book").GetComponent<BookUI>();

        for (int i = 0; i < 3; i++)
        {
            bookUI.dishList[i].SetActive(false);
        }

        bookUI.CurrentPage = cardID + 4;
    }


    string IngredientEffect(int cardID)
    {
        string effectText = null;

        switch(cardID)
        {
            case 0:
                effectText = "ギャンブル";
                break;
            case 1:
                effectText = "追加ダメージ";
                break;
            case 2:
                effectText = "レア食材";
                break;
            case 3:
                effectText = "回復";
                break;
            case 4:
                effectText = "限定条件";
                break;
            case 5:
                effectText = "料理コスト";
                break;
            case 6:
                effectText = "状態異常";
                break;
            case 7:
                effectText = "読み";
                break;
            case 8:
                effectText = "食材弱体";
                break;
        }

        return effectText;
    }

}
