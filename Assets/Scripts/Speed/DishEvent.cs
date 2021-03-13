using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DishEvent : EventTrigger
{
    //[SerializeField] SpeedCardController speedCardController = null;
  
    public override void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter called.");

        if (eventData.pointerDrag == null)
        {
            //Debug.Log("なし");
        }
        else
        {
            //Debug.Log("あり");
            GetComponent<SpeedDishView>().ChangeDishColor(1);

        }

        //SpeedCardController ingredientCardController = eventData.pointerDrag.GetComponent<SpeedCardController>();

        //Debug.Log(ingredientCardController.model.name);
    }


    public override void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("でた");

        if (eventData.pointerDrag == null)
        {
            //Debug.Log("なし");
        }
        else
        {
            //Debug.Log("あり");
            GetComponent<SpeedDishView>().ChangeDishColor(0);

        }




    }



}
