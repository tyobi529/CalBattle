using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSetting : MonoBehaviour
{
    //色ごとのデータ
    [SerializeField] Image[] vsBGImage = new Image[2]; 
    [SerializeField] Sprite[] vsBGSprite = new Sprite[2];

    //キャラクター
    [SerializeField] GameObject characterObj;
    //色ごとのデータ
    [SerializeField] GameObject[] characterPrefab = new GameObject[2];


    //料理
    [SerializeField] Image[] cookingFieldImage = new Image[2];
    [SerializeField] Sprite[] cookingBGSprite = new Sprite[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetColorObj(int[] playerColor)
    {        

        //vs
        for (int i = 0; i < 2; i++)
        {
            //色変更
            vsBGImage[i].sprite = vsBGSprite[playerColor[i]];
            GameObject character = Instantiate(characterPrefab[playerColor[i]], characterObj.transform, false);
            if (i == 1)
            {
                character.transform.localScale = new Vector3(-1f, 1f, 1f);
            }



            //料理
            cookingFieldImage[i].sprite = cookingBGSprite[playerColor[i]];
        }

    }
}
