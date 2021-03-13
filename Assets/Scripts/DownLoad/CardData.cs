using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public CardEntity[] ingCardEntity = new CardEntity[9] { null, null, null, null, null, null, null, null, null };
    public CardEntity[] dishCardEntity = new CardEntity[27] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

    //[SerializeField] DownLoadController test = null;

    //[SerializeField] Image[] icon = new Image[3] { null, null, null };
    //[SerializeField] Image[] gIcon = new Image[3] { null, null, null };
    //[SerializeField] Text[] name = new Text[3] { null, null, null };
    //[SerializeField] Text[] cal = new Text[3] { null, null, null };

    static public CardData instance;

    public bool isDownLoad { get; set; } = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void EntityShow()
    {
        for (int i = 0; i < 9; i++)
        {
            Debug.Log("食材" + i);
            Debug.Log(ingCardEntity[i].name);
            Debug.Log(ingCardEntity[i].kind);
            Debug.Log(ingCardEntity[i].icon);
            Debug.Log(ingCardEntity[i].glowIcon);
        }

        for (int i = 0; i < 27; i++)
        {
            Debug.Log("料理" + i);
            Debug.Log(dishCardEntity[i].name);
            Debug.Log(dishCardEntity[i].kind);
            Debug.Log(dishCardEntity[i].icon);
            //Debug.Log(dishCardEntity[i].glowIcon);
        }

    }


}
