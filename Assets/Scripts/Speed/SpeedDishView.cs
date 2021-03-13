using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeedDishView : MonoBehaviour
{
    //料理
    [SerializeField] Image dishImage = null;
    [SerializeField] TextMeshProUGUI dishNameText = null;

    [SerializeField] Image[] dishIngredientImage = new Image[2];


    [SerializeField] Image suitImage = null;
    //[SerializeField] GlowImage glowSuitImage = null;

    public GameObject uraImageObj;

    [SerializeField] Color[] dishBGColor = new Color[3];


    [SerializeField] ParticleSystem correctEffect = null;


    public void SetSpeedDishView(CardModel cardModel, Sprite suitSprite)
    {
        dishImage.sprite = cardModel.icon;

        dishNameText.text = cardModel.name;
        suitImage.sprite = suitSprite;

    }


    public void ChangeIngredientImage(int index, Sprite ingredientIcon)
    {
        dishIngredientImage[index].sprite = ingredientIcon;
    }

    public void ChangeDishColor(int colorNum)
    {
        suitImage.color = dishBGColor[colorNum];

        if (colorNum == 2)
        {
            DOVirtual.DelayedCall(1, () =>
            {
                suitImage.color = dishBGColor[0];
            });

        }
    }


}
