using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedDropPlace : MonoBehaviour, IDropHandler
{
    SpeedGameManager speedGameManager = null;
    SpeedCardGenerator speedCardGenerator = null;

    bool[] isFilled = new bool[2] { false, false };

    

    void Start()
    {
        speedGameManager = GameObject.Find("SpeedGameManager").GetComponent<SpeedGameManager>();
        speedCardGenerator = GameObject.Find("SpeedCardGenerator").GetComponent<SpeedCardGenerator>();
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        SpeedCardController ingredientCardController = eventData.pointerDrag.GetComponent<SpeedCardController>();

        SpeedCardController dishCardController = GetComponent<SpeedCardController>();



        if (ingredientCardController.model.cardID == dishCardController.model.ingredientCardID[0])
        {
            GetComponent<SpeedDishView>().ChangeDishColor(0);


            speedGameManager.CorrectEffect(ingredientCardController.transform.position);
    

            dishCardController.model.ingredientCardID[0] = -1;
   
        }
        else if (ingredientCardController.model.cardID == dishCardController.model.ingredientCardID[1])
        {
            GetComponent<SpeedDishView>().ChangeDishColor(0);

            speedGameManager.CorrectEffect(ingredientCardController.transform.position);


            dishCardController.model.ingredientCardID[1] = -1;            
        }
        else
        {            
            GetComponent<SpeedDishView>().ChangeDishColor(2);

            speedGameManager.isWrong = true;
            return;
        }

 
        for (int i = 0; i < 2; i++)
        {
            if (!isFilled[i])
            {
                dishCardController.GetComponent<SpeedDishView>().ChangeIngredientImage(i, ingredientCardController.model.icon);

                speedCardGenerator.GiveCardToHand(ingredientCardController.fieldIndex, true);
                Destroy(ingredientCardController.gameObject);

                isFilled[i] = true;

                break;
            }
        }

        if (isFilled[0] && isFilled[1])
        {
            speedCardGenerator.GiveCardToField(dishCardController.fieldIndex);

            speedGameManager.madeNum++;
            Destroy(this.gameObject);


        }
 

        
    }

    //private Vector2 GetLocalPosition(Vector2 screenPosition)
    //{
    //    Vector2 result = Vector2.zero;

    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

    //    return result;
    //}




}
