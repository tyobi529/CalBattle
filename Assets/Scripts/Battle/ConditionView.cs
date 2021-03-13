using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionView : MonoBehaviour
{
    [SerializeField] Image iconImage = null;
    [SerializeField] Image countImage = null;
    [SerializeField] Image bgImage = null;

    [SerializeField] Sprite[] conditionSprite = new Sprite[3];
    [SerializeField] Sprite[] numberSprite = new Sprite[10];


    [SerializeField] Color[] conditionBGColor = new Color[3];



    public void ChangeConditionView(int num, int count)
    {
        iconImage.sprite = conditionSprite[num];

        bgImage.color = conditionBGColor[num];

        countImage.sprite = numberSprite[count];

        
    }

    
}
