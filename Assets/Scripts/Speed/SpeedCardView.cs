using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedCardView : MonoBehaviour
{

    //食材
    //public GlowImage glowImage;
    //public Image iconImage = null;
    public Image iconImage = null;
    [SerializeField] Image ingredientImage;
    public GameObject uraImageObj;


    //トランプの柄
    [SerializeField] Image suitImage = null;




    public void SetSpeedHandCardView(CardModel cardModel, Sprite suitSprite)
    {        

        ingredientImage.sprite = cardModel.icon;
        
        suitImage.sprite = suitSprite;
        
    }


    //public void ShowHint()
    //{
    //    //ingredientImage.color = Color.blue;
    //    StartBlink();
    //}

    public void StartBlink()
    {
        StartCoroutine(BlinkBG());
    }

    public void StopBlink()
    {
        StopAllCoroutines();
        suitImage.color = Color.white;
    }


    IEnumerator BlinkBG()
    {
        Debug.Log("光らせる");
        float alpha_Sin = 0f;
        Color _color = suitImage.color;

        while (true)
        {
            alpha_Sin = Mathf.Sin(Time.time * 5f) / 2f + 0.5f;
            _color.b = alpha_Sin;

            suitImage.color = _color;

            yield return new WaitForSeconds(0.01f);

        }

    }
}
