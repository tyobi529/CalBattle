using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedCardGenerator : MonoBehaviour
{
    [SerializeField] SpeedGameManager speedGameManager = null;

    [SerializeField] GameObject speedDishCardPrefab;
    [SerializeField] GameObject speedCardPrefab;
    [SerializeField] GameObject speedTitleCardPrefab;

    [SerializeField] Transform[] fieldTransform = new Transform[3];
    [SerializeField] Transform[] handTransform = new Transform[4];

    [SerializeField] Transform[] beforeFieldTransform = new Transform[3];
    [SerializeField] Transform[] beforeHandTransform = new Transform[4];

    //トランプの柄
    [SerializeField] Sprite[] suitSprite = new Sprite[4];


    int[] deck = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    int[] fieldDeck = new int[27] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };

    int cardIndex = 0;
    int fieldCardIndex = 0;


    int[] suitDeck = new int[4] { 0, 1, 2, 3 };
    int suitIndex = 0;

    int cardID = 0;


    [SerializeField] Transform[] titleCardTransform = new Transform[2] { null, null };


    [SerializeField] GameObject[] titleCardLeft = new GameObject[4];
    [SerializeField] GameObject[] titleCardRight = new GameObject[4];


    // Start is called before the first frame update
    void Start()
    {
        cardID = Random.Range(0, 27);

        ShuffleHandCard();
        ShuffleFieldCard();


    }


 

    public void GiveCardToHand(int index, bool needCheck)
    {

        int cardID = deck[cardIndex];

        if (cardIndex == 8)
        {
            ShuffleHandCard();
            cardIndex = 0;
        }
        else
        {
            cardIndex++;
        }

        speedGameManager.speedCardController[index] = CreateSpeedCard(false, cardID, 0, 0, index);

        SpeedSoundManager.instance.ChangeCardSE();

        if (needCheck)
        {
            speedGameManager.CheckCard();
        }
        
    }


    public void GiveCardToField(int index)
    {

        int cardID = fieldDeck[fieldCardIndex];

        if (fieldCardIndex == 26)
        {
            ShuffleFieldCard();
            fieldCardIndex = 0;
        }
        else
        {
            fieldCardIndex++;
        }

        speedGameManager.speedDishController[index] = CreateSpeedDish(true, cardID, 0, 0, index);
    }




    SpeedCardController CreateSpeedCard(bool isDish, int cardID, int cost, int condition, int index)
    {


        GameObject card = Instantiate(speedCardPrefab, beforeHandTransform[index], false);

        card.GetComponent<SpeedCardController>().Init(isDish, cardID, cost, condition, index);

        int num = Random.Range(0, 4);

        card.GetComponent<SpeedCardView>().SetSpeedHandCardView(card.GetComponent<SpeedCardController>().model, suitSprite[num]);



        card.transform.DOMove(handTransform[index].position, 0.5f).OnComplete(() =>
        {

            card.transform.SetParent(handTransform[index]);
            card.GetComponent<SpeedCardMovement>().SetParentRect();
            card.transform.DORotate(new Vector3(0f, 90f, 0f), 0.2f).OnComplete(() =>
            {

                card.GetComponent<SpeedCardView>().uraImageObj.SetActive(false);
                card.transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f);
            });
        });


        return card.GetComponent<SpeedCardController>();


    }


    SpeedCardController CreateSpeedDish(bool isDish, int cardID, int cost, int condition, int index)
    {

        GameObject card = Instantiate(speedDishCardPrefab, beforeFieldTransform[index], false);

        card.GetComponent<SpeedCardController>().DishInit(isDish, cardID, cost, condition, index);

        int num = Random.Range(0, 4);

        card.GetComponent<SpeedDishView>().SetSpeedDishView(card.GetComponent<SpeedCardController>().model, suitSprite[num]);

        card.transform.DOMove(fieldTransform[index].position, 0.5f).OnComplete(() =>
        {

            card.transform.SetParent(fieldTransform[index]);
            //card.GetComponent<SpeedCardMovement>().SetParentRect();

            card.transform.DORotate(new Vector3(0f, 90f, 0f), 0.2f).OnComplete(() =>
            {

                card.GetComponent<SpeedDishView>().uraImageObj.SetActive(false);
                card.transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f);
            });
        });


        //GameObject card = Instantiate(speedFieldCardPrefab, fieldTransform[index], false);

        //card.GetComponent<SpeedCardController>().DishInit(isDish, cardID, cost, condition, index);

        return card.GetComponent<SpeedCardController>();

    }


    void ShuffleHandCard()
    {
        for (int i = 8; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck[i];
            deck[i] = deck[j];
            deck[j] = tmp;
        }

    }

    void ShuffleFieldCard()
    {
        for (int i = 26; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = fieldDeck[i];
            fieldDeck[i] = fieldDeck[j];
            fieldDeck[j] = tmp;
        }

    }

    void ShuffleSuit()
    {
        for (int i = 3; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = suitDeck[i];
            suitDeck[i] = suitDeck[j];
            suitDeck[j] = tmp;
        }
    }



    public void SetTitleCard()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int spriteNum = suitDeck[suitIndex];

                titleCardLeft[i].transform.GetChild(j).GetComponent<SpeedTitleCardController>().SetView(cardID, suitSprite[spriteNum]);

                if (suitIndex == 3)
                {
                    ShuffleSuit();
                    suitIndex = 0;
                }
                else
                {
                    suitIndex++;
                }

                if (cardID == 26)
                {
                    cardID = 0;
                }
                else
                {
                    cardID++;
                }
            }
  
        }


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int spriteNum = suitDeck[suitIndex];

                titleCardRight[i].transform.GetChild(j).GetComponent<SpeedTitleCardController>().SetView(cardID, suitSprite[spriteNum]);

                if (suitIndex == 3)
                {
                    ShuffleSuit();
                    suitIndex = 0;
                }
                else
                {
                    suitIndex++;
                }

                if (cardID == 26)
                {
                    cardID = 0;
                }
                else
                {
                    cardID++;
                }
            }

        }




    }


    public void GenerateTitleCard(int index)
    {
        
        GameObject card = Instantiate(speedTitleCardPrefab, titleCardTransform[index]);
        card.transform.position = titleCardTransform[index].position;
        SpeedTitleCardController speedTitleCardController = card.GetComponent<SpeedTitleCardController>();

        int spriteNum = suitDeck[suitIndex];
        speedTitleCardController.SetView(cardID, suitSprite[spriteNum]);

        if (suitIndex == 3)
        {
            ShuffleSuit();
            suitIndex = 0;
        }
        else
        {
            suitIndex++;
        }

        if (cardID == 26)
        {
            cardID = 0;
        }
        else
        {
            cardID++;
        }

        //for (int i = 0; i < 20; i++)
        //{
        //    GameObject card = Instantiate(speedTitleCardPrefab, titleCardTransform);
        //    card.transform.position = titleCardTransform.position;
        //    SpeedTitleCardController speedTitleCardController = card.GetComponent<SpeedTitleCardController>();

        //    int spriteNum = suitDeck[suitIndex];
        //    speedTitleCardController.SetView(i, suitSprite[spriteNum]);

        //    if (spriteNum == 3)
        //    {
        //        ShuffleSuit();
        //        spriteNum = 0;
        //    }
        //    else
        //    {
        //        spriteNum++;
        //    }

        //}
    }
}
