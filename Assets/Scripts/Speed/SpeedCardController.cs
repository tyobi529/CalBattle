using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedCardController : MonoBehaviour
{
    public CardModel model; //データ（model）関することを操作
    

    public int fieldIndex { get; set; } = -1;    



    public void Init(bool isDish, int cardID, int cost, int condition, int index)
    {

        model = new CardModel(isDish, cardID, cost, condition);

        fieldIndex = index;

        //SetSpeedHandCardView();

    }

    public void DishInit(bool isDish, int cardID, int cost, int condition, int index)
    {

        model = new CardModel(isDish, cardID, cost, condition);

        fieldIndex = index;

        //SetSpeedFieldCardView();

    }




   


}
