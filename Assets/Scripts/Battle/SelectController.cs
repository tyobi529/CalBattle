using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectController : MonoBehaviour
{
    public CardController[] selectCardController { get; set; } = new CardController[2] { null, null };

    [SerializeField] CardGenerator cardGenerator = null;

    [SerializeField] Transform[] mixFieldTransform = new Transform[3] { null, null, null };

    [SerializeField] GamePlayerManager player = null;
    


    [SerializeField] UIManager uiManager = null;


    public GameObject[] cursorObj = new GameObject[4] { null, null, null, null };
    [SerializeField] Sprite[] numberSprite = new Sprite[2] { null, null };


    public GameObject[] messageTextObj = new GameObject[2] { null, null };

    [SerializeField] TextMeshProUGUI bottomText = null;



    //合成予測を出す
    public void SelectCard(CardController cardController, bool isSelected)
    {

        //選択した時
        if (isSelected)
        {
            //何も選択されていない
            if (selectCardController[0] == null)
            {
                selectCardController[0] = cardController;
            }
            //２つ目に選択
            else if (selectCardController[1] == null)
            {
                selectCardController[1] = cardController;

            }
            //２つ目と入れ替え
            else
            {
                selectCardController[1].model.isSelected = false;

                selectCardController[1] = cardController;
            }

        }
        //キャンセルした時
        else
        {
            //１枚目に選択してた時
            if (selectCardController[0] == cardController)
            {

                //２枚目に選択してたカードなし
                if (selectCardController[1] == null)
                {
                    selectCardController[0] = null;
                }
                //２枚目に選択したカードを１枚目にする
                else
                {
                    selectCardController[0] = selectCardController[1];
                    selectCardController[1] = null;
                }
            }
            //２枚目に選択してた時
            else
            {
                selectCardController[1] = null;
            }

        }

        //選択カーソル
        for (int i = 0; i < 4; i++)
        {
            cursorObj[i].SetActive(false);
        }


        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {
                cursorObj[selectCardController[i].model.index].SetActive(true);
                cursorObj[selectCardController[i].model.index].transform.GetChild(1).GetComponent<Image>().sprite = numberSprite[i];

            }
        }

        GenerateFieldCard();
    }




    public void GenerateFieldCard()
    {
        CleanField();

        messageTextObj[0].SetActive(false);
        messageTextObj[1].SetActive(false);

        CardController[] mixCardController = new CardController[2] { null, null };

        CardController eatCardController;        


        for (int i = 0; i < 2; i++)
        {
            mixCardController[i] = null;
        }

        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {                
                mixCardController[i] = cardGenerator.CreateCard(false, selectCardController[i].model.cardID, selectCardController[i].model.cost, selectCardController[i].model.condition, mixFieldTransform[i]);
                mixCardController[i].model.cal = selectCardController[i].model.cal;
                mixCardController[i].GetComponent<CardView>().SetCookingCard(mixCardController[i].model);
                mixCardController[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }


        //合成チェック
        if (mixCardController[1] != null)
        {
            if (mixCardController[0].model.kind == mixCardController[1].model.kind)
            {
                //合成不可                
                messageTextObj[1].SetActive(true);
                return;
            }
            else
            {

                //合成
                int mixCardID = cardGenerator.SpecialMix(mixCardController[0], mixCardController[1]);
                bool isStrong = false;


                if (mixCardController[0].model.condition == 2 || mixCardController[1].model.condition == 2)
                {
                    isStrong = true;
                }

                //チュートリアル
                if (GameManager.instance.tutorial)
                {
  

                    if (GameManager.instance.tutorialNum == 0)
                    {
                        messageTextObj[0].SetActive(true);
                        messageTextObj[0].GetComponent<TextMeshProUGUI>().text = "食材を１つ" + "\nえらぼう！";
                        return;
                    }
                    //レア以外を使っている場合
                    else if (GameManager.instance.tutorialNum == 2)
                    {
                        if (!isStrong)
                        {
                            messageTextObj[0].SetActive(true);
                            messageTextObj[0].GetComponent<TextMeshProUGUI>().text = "レア食材を" + "\n材料にしよう！";
                            return;
                        }
                    }
                }


                eatCardController = cardGenerator.CreateEatCard(true, mixCardID, 0, 0, mixFieldTransform[2]);

                eatCardController.model.isStrong = isStrong;
  

                eatCardController.GetComponent<EatCardView>().SetEatCard(eatCardController.model);


                //コストがあってBadでない
                if (player.cost >= 3 && mixCardController[0].model.condition > 0 && mixCardController[1].model.condition > 0)
                {                                        
                    //eatCardController.model.canSelected = true;
                    eatCardController.GetComponent<CanvasGroup>().blocksRaycasts = true;

                    eatCardController.GetComponent<EatCardView>().SetErrorMessage(2);

                    eatCardController.GetComponent<EatCardView>().StartBlinking();
                }
                else
                {    
                    eatCardController.GetComponent<EatCardView>().StopBlinking();
                    //eatCardController.model.canSelected = false;
                    eatCardController.GetComponent<CanvasGroup>().blocksRaycasts = false;

                    if (player.cost < 3)
                    {                        
                        eatCardController.GetComponent<EatCardView>().SetErrorMessage(0);
                    }
                    else
                    {                        
                        eatCardController.GetComponent<EatCardView>().SetErrorMessage(1);
                    }
                }

            }
        }
        else if (mixCardController[0] != null)
        {
            //チュートリアル中に単体で押せないように
            if (GameManager.instance.tutorial)
            {

                //bottomText.text = "好きな食材を" + "\nえらぼう！";

                if (GameManager.instance.tutorialNum == 1 || GameManager.instance.tutorialNum == 2)
                {
                    messageTextObj[0].SetActive(true);

                    messageTextObj[0].GetComponent<TextMeshProUGUI>().text = "食材を２つ" + "\nえらぼう！";
                    return;
                }
            }


            eatCardController = cardGenerator.CreateEatCard(false, mixCardController[0].model.cardID, mixCardController[0].model.cost, mixCardController[0].model.condition, mixFieldTransform[2]);

            eatCardController.model.cal = mixCardController[0].model.cal;
            eatCardController.model.cost = mixCardController[0].model.cost;
            eatCardController.GetComponent<EatCardView>().SetEatCard(eatCardController.model);

            
            eatCardController.GetComponent<EatCardView>().StartBlinking();


            //eatCardController.model.canSelected = true;
            eatCardController.GetComponent<CanvasGroup>().blocksRaycasts = true;

            eatCardController.GetComponent<EatCardView>().SetErrorMessage(2);
        }
        else
        {
            messageTextObj[0].GetComponent<TextMeshProUGUI>().text = "好きな食材を" + "\nえらぼう！";
            messageTextObj[0].SetActive(true);
            return;            
        }



    }


    public void ResetSelect()
    {
        //選択カーソル
        for (int i = 0; i < 4; i++)
        {
            cursorObj[i].SetActive(false);
        }

        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {
                selectCardController[i].model.isSelected = false;
                selectCardController[i] = null;
            }
        }

        CleanField();
    }

    public void CleanField()
    {

        foreach (Transform field in mixFieldTransform)
        {
            if (field.childCount != 0)
            {
                foreach (Transform child in field)
                {
                    Destroy(child.gameObject);
                }
                //Destroy(field.GetChild(0).gameObject);
            }
        }



    }



}
