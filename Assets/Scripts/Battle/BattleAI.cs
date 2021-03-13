using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAI : MonoBehaviour
{
    bool tutorial = false;
    int cpuLevel = 0;

    int[] selectCardInex = new int[2];

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2] { null, null };
    //[SerializeField] Transform[,] handTransform { get; set; } = new Transform[2, 4] { { null, null, null, null }, { null, null, null, null } };

    [SerializeField] GameManager gameManager = null;

    [SerializeField] CardGenerator cardGenerator;

    public void SetCpuLevel(int cpuLevel, bool tutorial)
    {
        //cpuLevel = Page_0Controller.GetCpuLevel();
        this.cpuLevel = cpuLevel;
        this.tutorial = tutorial;

        Debug.Log("CPUレベル " + cpuLevel);
    }


    public int[] SelectCard()
    {
        int[] selectCardIndex = new int[2] { -1, -1 };

        int cost = player[1].cost;

        //チュートリアルは一枚だけ選択
        if (tutorial)
        {
            selectCardIndex[0] = 0;
            return selectCardIndex;
        }


        if (cpuLevel == 1)
        {
            //合成する数
            int mixNum = -1;

            //ランダム
            if (cost == 6)
            {
                mixNum = 2;
            }
            else if (cost >= 3)
            {
                int num = Random.Range(1, cost);
                if (num == 1)
                {
                    mixNum = 1;
                }
                else
                {
                    mixNum = 2;
                }

            }
            else
            {
                mixNum = 1;
            }

            //単体
            if (mixNum == 1)
            {
                selectCardIndex[0] = Random.Range(0, 4);
            }
            //料理
            else
            {
                //捨てるレア素材の選択
                List<int> cardIndex = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    cardIndex.Add(i);
                }


                for (int i = 0; i < mixNum; i++)
                {

                    selectCardIndex[i] = cardIndex[Random.Range(0, cardIndex.Count)];

                    cardIndex.Remove(selectCardIndex[i]);

                }

                CardController[] cardController = new CardController[2];

                for (int i = 0; i < 2; i++)
                {
                    cardController[i] = gameManager.handTransform[1, selectCardIndex[i]].transform.GetChild(0).GetComponent<CardController>();
                }

                //同じ属性か料理不可
                if (cardController[0].model.kind == cardController[1].model.kind || cardController[0].model.condition == 0 || cardController[1].model.condition == 0)
                {
                    selectCardIndex[1] = -1;
                }

                
            }


  
        }
        else if (cpuLevel == 2)
        {
            //合成する数
            int mixNum = -1;

            //ランダム
            if (cost == 6)
            {
                mixNum = 2;
            }
            else if (cost >= 3)
            {
                int num = Random.Range(1, cost);
                if (num == 1)
                {
                    mixNum = 1;
                }
                else
                {
                    mixNum = 2;
                }

            }
            else
            {
                mixNum = 1;
            }

            //単体
            if (mixNum == 1)
            {
                bool bad = false;
                //悪い食材を優先
                for (int i = 0; i < 4; i++)
                {
                    if (gameManager.handTransform[1, i].GetChild(0).GetComponent<CardController>().model.condition == 0)
                    {
                        selectCardIndex[0] = i;
                        bad = true;
                        break;
                    }
                }

                if (!bad)
                {
                    selectCardIndex[0] = Random.Range(0, 4);
                }
            }
            //料理
            else
            {

                //レア素材を探す
                int rareIndex = -1;
                for (int i = 0; i < 4; i++)
                {
                    if (gameManager.handTransform[1, i].GetChild(0).GetComponent<CardController>().model.condition == 2)
                    {
                        rareIndex = i;
                        break;
                    }
                }

                //捨てるレア素材の選択
                List<int> cardIndex = new List<int>();



                //レアあり
                if (rareIndex != -1)
                {
                    //レア以外入れる
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == rareIndex)
                        {
                            continue;
                        }

                        cardIndex.Add(i);
                    }

                    int a = Random.Range(0, cardIndex.Count);

                    selectCardIndex[1] = cardIndex[a];
                    selectCardIndex[0] = rareIndex;
   
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        cardIndex.Add(i);
                    }

                    for (int i = 0; i < mixNum; i++)
                    {

                        selectCardIndex[i] = cardIndex[Random.Range(0, cardIndex.Count)];

                        cardIndex.Remove(selectCardIndex[i]);
       
                    }
                }



                CardController[] cardController = new CardController[2];

                for (int i = 0; i < 2; i++)
                {
                    cardController[i] = gameManager.handTransform[1, selectCardIndex[i]].transform.GetChild(0).GetComponent<CardController>();
                }

                //同じ属性か料理不可
                if (cardController[0].model.kind == cardController[1].model.kind || cardController[0].model.condition == 0 || cardController[1].model.condition == 0)
                {
                    selectCardIndex[1] = -1;
                }


            }

        }
        else if (cpuLevel == 3)
        {
            //合成する数
            int mixNum = -1;


            //即合成
            if (cost >= 4)
            {
                mixNum = 2;
            }
            else if (cost >= 3)
            {
                int num = Random.Range(1, cost);
                if (num == 1)
                {
                    mixNum = 1;
                }
                else
                {
                    mixNum = 2;
                }
            }
            else
            {
                mixNum = 1;
            }

            //単体
            if (mixNum == 1)
            {
                bool bad = false;
                //悪い食材を優先
                for (int i = 0; i < 4; i++)
                {
                    if (gameManager.handTransform[1, i].GetChild(0).GetComponent<CardController>().model.condition == 0)
                    {
                        selectCardIndex[0] = i;
                        bad = true;
                        break;
                    }
                }

                if (!bad)
                {
                    selectCardIndex[0] = Random.Range(0, 4);
                }
                
            }
            //料理
            else
            {
                CardController[] cardController = new CardController[4];

                for (int i = 0; i < 4; i++)
                {
                    cardController[i] = gameManager.handTransform[1, i].GetChild(0).GetComponent<CardController>();
                }

                //bad以外を入れる
                //List<int> cardIndex = new List<int>();

                //for (int i = 0; i < 4; i++)
                //{
                //    if (cardController[i].model.condition == 0)
                //    {
                //        continue;
                //    }
       
                //    cardIndex.Add(i);
                //}

                
                int maxCal = -1;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = i + 1; j < 4; j++)
                    {

                        if (cardController[i].model.condition == 0 || cardController[j].model.condition == 0)
                        {
                            continue;
                        }

                        if (cardController[i].model.kind == cardController[j].model.kind)
                        {
                            continue;
                        }

                        int mixCardID = cardGenerator.SpecialMix(cardController[i], cardController[j]);

                        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + mixCardID);

                        int cal = cardEntity.cal;

                        if (cal > maxCal)
                        {
                            maxCal = cal;
                            selectCardIndex[0] = i;
                            selectCardIndex[1] = j;
                        }
                    }
                }


                if (maxCal < 0)
                {
                    selectCardIndex[0] = 0;
                }


                cardController[0] = null;
                cardController[1] = null;

                for (int i = 0; i < 2; i++)
                {
                    cardController[i] = gameManager.handTransform[1, selectCardIndex[i]].GetChild(0).GetComponent<CardController>();
                }

                //同じ属性か料理不可
                if (cardController[0].model.kind == cardController[1].model.kind || cardController[0].model.condition == 0 || cardController[1].model.condition == 0)
                {
                    selectCardIndex[1] = -1;
                }


            }
        }



        return selectCardIndex;
        
    }
}
