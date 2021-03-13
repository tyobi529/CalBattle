using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedTitleCardController : MonoBehaviour
{
    [SerializeField] Image suitImage = null;
    [SerializeField] Image iconImage = null;

    [SerializeField] GameObject uraObj = null;

    float rotateVolume = 0f;
    bool ura = false;

    // Start is called before the first frame update
    void Start()
    {
        float firstRotate = Random.Range(0f, 360f);

        if (firstRotate > 90f && firstRotate < 270f)
        {
            ura = true;            
        }
        else
        {
            ura = false;
        }

        

        uraObj.SetActive(ura);

        transform.localEulerAngles = new Vector3(0f, firstRotate, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0f, -0.03f, 0f);
        transform.Rotate(0f, 3f, 0f);


        //if (transform.localEulerAngles.y > 90f && !ura)
        if (transform.localEulerAngles.y > 90f && transform.localEulerAngles.y < 270f)
        {
            if (!ura)
            {
                //Debug.Log("表から裏");
                ura = true;
                uraObj.SetActive(ura);
            }
    
        }
        else
        {
            if (ura)
            {
                //Debug.Log("裏から表");
                ura = false;
                uraObj.SetActive(ura);
            }

        }

        if (transform.localEulerAngles.y > 360f)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }

    

    }


    public void SetView(int cardID, Sprite suitSprite)
    {
        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        CardEntity cardEntity = CardData.instance.dishCardEntity[cardID];


        iconImage.sprite = cardEntity.icon;

        suitImage.sprite = suitSprite;
    }
}
