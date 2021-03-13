using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialController : MonoBehaviour
{
    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] MessageController messageController = null;


    int damageCal = 0;
    bool isStrong = false;
    bool isAttacker = false;
    GamePlayerManager attacker = null;
    GamePlayerManager defender = null;

    string effectMessage = null;


    [SerializeField] UIManager uiManager = null;

    string attackerName = null;
    string defenderName = null;

    GameManager gameManager = null;


    public int randomCardID { get; set; } = -1;

    public bool randomSuccess { get; set; } = false;
    
    public int[] stealIndex { get; set; } = new int[2] { -1, -1 };
    public int[] rareIndex { get; set; } = new int[2] { -1, -1 };
    public int[] throwIndex { get; set; } = new int[2] { -1, -1 };

    public int[] weakenIndex { get; set; } = new int[4] { -1, -1, -1, -1 };

    public bool isSatisfied { get; set; } = false;

    private CardController[] throwCardController = new CardController[2] { null, null };

    [SerializeField] BattleSoundManager battleSoundManager = null;
    


    public void SetGameManager(bool single)
    {
        if (single)
        {
            gameManager = GameObject.Find("SingleGameManager").GetComponent<GameManager>();
        }
        else 
        {
            gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        }
        

        for(int i = 0; i < 2; i++)
        {            
            stealIndex[i] = -1;
            rareIndex[i] = -1;
            throwIndex[i] = -1;            
        }

        for (int i = 0; i < 4; i++)
        {
            weakenIndex[i] = -1;
        }
    }

    //料理の効果
    public IEnumerator DishEffect(int cardID, bool isStrong, int damageCal, bool isAttacker)
    {
        messageController.messagePanel.SetActive(true);
        messageController.ChangeMessageColor(isAttacker);


        this.isStrong = isStrong;
        this.damageCal = damageCal;
        this.isAttacker = isAttacker;


        effectMessage = null;


        if (isAttacker)
        {
            attacker = player[0];
            defender = player[1];
            attackerName = gameManager.playerName[0];
            defenderName = gameManager.playerName[1];
        }
        else
        {
            attacker = player[1];
            defender = player[0];
            attackerName = gameManager.playerName[1];
            defenderName = gameManager.playerName[0];
        }


        if (cardID == 0)
        {
            messageController.EatMessage(gameManager.eatCardController[attacker.playerIndex].model, isAttacker);
            yield return new WaitForSeconds(2f);
        }


        //攻撃できなくても効果が出るもの
        switch (cardID)
        {
            case 0:
                RandomEffect();
                break;
            case 1:
                GamblingAttack();
                break;
            case 2:
                RandomDamage();
                break;
            case 3:
                EnemyCostBonus();
                break;
            case 4:
                MyConditionBonus();
                break;
            case 5:
                TurnCountBonus();                
                break;
            case 6:
                ThrowEnemyRare();
                break;
            case 7:
                IncreaseMyRare();
                break;
            case 8:
                StealEnemyRare();
                break;
            case 9:
                HealHp();
                break;
            case 10:
                DamageHealHp();
                break;
            case 11:
                HealCondition();
                break;
            case 12:
                DishAttack();
                break;
            case 13:
                ConditionAttack();
                break;
            case 14:
                BadIngredientAttack();
                break;
            case 15:
                StealEnemyCost();
                break;
            case 16:
                IncreaseMyCost();
                break;
            case 17:
                ReduceEnemyCost();
                break;
            case 18:
                Clumsy();
                break;
            case 19:
                Dark();
                break;
            case 20:
                Poison();                
                break;
            case 21:
                ReduceDamage();
                break;
            case 22:
                DamageCounter();
                break;
            case 23:
                InvalidateEffect();
                break;
            case 24:
                WeakenIngredient("赤食材");
                break;
            case 25:
                WeakenIngredient("黄食材");
                break;
            case 26:
                WeakenIngredient("緑食材");
                break;
            default:
                Debug.Log("料理範囲外");
                break;
        }



        if (effectMessage != null)
        {
            messageController.EffectMessage(effectMessage);
            effectMessage = null;
            yield return new WaitForSeconds(2f);            
        }


        //ランダム
        if (cardID == 0)
        {
            gameManager.eatCardController[attacker.playerIndex].model.cardID = randomCardID;
            randomCardID = -1;
            if (isAttacker || gameManager.single)
            {
                gameManager.DamageCalculation();
            }
        }
        else
        {
            gameManager.CheckStatus();
        }
        

    }



    //ランダムでダメージ
    void RandomDamage()
    {

        if (randomSuccess)
        {
            defender.hp -= 200;
            uiManager.ShowDamageText(200, defender.playerIndex, false);


            effectMessage = defenderName + " に大ダメージ！";

            randomSuccess = false;

        }
        else
        {
            attacker.hp -= 200;
            uiManager.ShowDamageText(200, attacker.playerIndex, false);



            effectMessage = attackerName + " に大ダメージ！";
        }

        battleSoundManager.AttackSE(1);


    }


    void GamblingAttack()
    {
        Debug.Log("確率で1になる");

        if (damageCal == 1)
        {

            effectMessage = "ダメージが１になってしまった！";
            battleSoundManager.SpecialSE(1);

        }
    }




   

    
    //相手のレアを捨てる
    void ThrowEnemyRare()
    {
        bool trigger = false;

        //親変更
        for (int i = 0; i < 2; i++)
        {
            if (throwIndex[i] != -1)
            {
                trigger = true;

                if (gameManager.handTransform[defender.playerIndex, throwIndex[i]].childCount != 0)
                {
                    throwCardController[i] = gameManager.handTransform[defender.playerIndex, throwIndex[i]].GetChild(0).GetComponent<CardController>();

                    throwCardController[i].transform.SetParent(uiManager.transform);

                    throwCardController[i].transform.DOScale(new Vector3(0f, 0f, 0f), 2f).OnComplete(() => {
                        Destroy(throwCardController[i].gameObject);
                    });
                }


            }

        }


        for (int i = 0; i < 2; i++)
        {
            throwIndex[i] = -1;
        }

        if (trigger)
        {             
            effectMessage = defenderName + " のレア食材が無くなった！";
            if (!isAttacker)
            {
                battleSoundManager.SpecialSE(1);
            }

            
        }
        else
        {
            effectMessage = defenderName + " はレア食材を持っていなかった！";

        }


    }


    //１枚レアにする
    void IncreaseMyRare()
    {
        bool trigger = false;

        for (int i = 0; i < 2; i++)
        {
            Debug.Log(rareIndex[i] + "をレアに");
        }
        

        //親変更
        for (int i = 0; i < 2; i++)
        {
            if (rareIndex[i] != -1)
            {
                trigger = true;

                CardController cardController = null;

                if (gameManager.handTransform[attacker.playerIndex, rareIndex[i]].childCount != 0)
                {
                    cardController = gameManager.handTransform[attacker.playerIndex, rareIndex[i]].GetChild(0).GetComponent<CardController>();
                }


                cardController.model.condition = 2;
                cardController.GetComponent<CardView>().Refresh(cardController.model);   
            }

        }


        for (int i = 0; i < 2; i++)
        {
            rareIndex[i] = -1;
        }

        if (trigger)
        {
            effectMessage = attackerName + " の食材がレアに変わった！";
            if (isAttacker)
            {
                battleSoundManager.SpecialSE(0);
            }
            
        }

    }

    //相手のレアを奪う
    void StealEnemyRare()
    {
        Debug.Log("相手の食材を奪う");

        bool trigger = false;
        //親変更
        for (int i = 0; i < 2; i++)
        {
            if (stealIndex[i] != -1)
            {

                trigger = true;

                Vector3[] path = new Vector3[2];


                int recieveIndex = -1;
                //空いてる手札を探す
                for (int j = 0; j < 4; j++)
                {
                    if (gameManager.handTransform[attacker.playerIndex, j].childCount == 0)
                    {
                        recieveIndex = j;

                        CardController cardController = gameManager.handTransform[defender.playerIndex, stealIndex[i]].GetChild(0).GetComponent<CardController>();
                        cardController.transform.SetParent(gameManager.handTransform[attacker.playerIndex, recieveIndex]);

                        //奪ったレアのindex更新
                        cardController.model.index = j;

                        path[0] = gameManager.handTransform[defender.playerIndex, stealIndex[i]].position;
                        path[1] = gameManager.handTransform[attacker.playerIndex, recieveIndex].position;

                        Vector3 scale = new Vector3(1f, 1f, 1f);
                        gameManager.MoveCardTween(cardController, path, scale, 1f);

                        break;
                    }


                }

  
            }
            
        }


        for (int i = 0; i < 2; i++)
        {
            stealIndex[i] = -1;
        }


        if (trigger)
        {
            if (isAttacker)
            {
                effectMessage = "レア食材を奪った!";
                battleSoundManager.SpecialSE(0);
            }
            else
            {
                effectMessage = "レア食材を奪われてしまった！";
                battleSoundManager.SpecialSE(1);
            }
        }
        else
        {
            effectMessage = defenderName + " はレア食材を持っていなかった！";

        }




    }

    

    //相手のコストを下げる
    void ReduceEnemyCost()
    {
        Debug.Log("コスト減少" + isStrong);

        if (isStrong)
        {
            defender.cost -= 4;
        }
        else
        {
            defender.cost -= 2;
        }
 

        if (defender.cost < 0)
        {
            defender.cost = 0;
        }

        effectMessage = defenderName + "のコストが下がった！";

        if (!isAttacker)
        {
            battleSoundManager.SpecialSE(1);
        }

        uiManager.ShowCost();


    }






    //自分のコスト増加
    void IncreaseMyCost()
    {
        Debug.Log("コスト増加" + isStrong);

        if (isStrong)
        {
            attacker.cost += 4;
        }
        else
        {
            attacker.cost += 2;
        }


        if (attacker.cost > 6)
        {
            attacker.cost = 6;
        }

        effectMessage = attackerName + "のコストが上がった！";

        if (isAttacker)
        {
            battleSoundManager.SpecialSE(0);
        }

        uiManager.ShowCost();

    }

    void StealEnemyCost()
    {
        Debug.Log("コスト奪う" + isStrong);

        int stealCost = -1;

        if (isStrong)
        {
            stealCost = 4;
        }
        else
        {
            stealCost = 2;
        }

        if (defender.cost < stealCost)
        {
            stealCost = defender.cost;
        }


        attacker.cost += stealCost;
        if (attacker.cost > 6)
        {
            attacker.cost = 6;
        }

        defender.cost -= stealCost;

        if (isAttacker)
        {
            effectMessage = "料理コストを奪った！";
            battleSoundManager.SpecialSE(0);
        }
        else
        {
            effectMessage = "料理コストを奪われた！";
            battleSoundManager.SpecialSE(1);

        }

        uiManager.ShowCost();
    }




    //相手のコスト分ダメージ増加
    void EnemyCostBonus()
    {
        Debug.Log("相手のコスト分ダメージ増加" + isStrong);

        int addDamage = defender.cost * 30;

        if (isStrong)
        {
            addDamage /= 2;
            addDamage *= 3;
        }

        Debug.Log("追加ダメージ" + addDamage);

        defender.hp -= addDamage;
        uiManager.ShowDamageText(addDamage, defender.playerIndex, false);

        effectMessage = defenderName + " のコスト分の追加ダメージ！";

        if (damageCal >= 300)
        {
            battleSoundManager.AttackSE(2);
        }
        else if (damageCal >= 150)
        {
            battleSoundManager.AttackSE(1);
        }
        else
        {
            battleSoundManager.AttackSE(0);
        }



    }


    //自分の状態異常分ダメージ増加
    void MyConditionBonus()
    {
        Debug.Log("自分の状態異常分ダメージ増加" + isStrong);

        int addDamage = (attacker.poisonCount + attacker.darkCount + attacker.clumsyCount) * 50;

        if (isStrong)
        {
            addDamage /= 2;
            addDamage *= 3;
        }

        Debug.Log("追加ダメージ" + addDamage);

        defender.hp -= addDamage;
        uiManager.ShowDamageText(addDamage, defender.playerIndex, false);

        effectMessage = attackerName + " の状態異常分の追加ダメージ！";


        if (damageCal >= 300)
        {
            battleSoundManager.AttackSE(2);
        }
        else if (damageCal >= 150)
        {
            battleSoundManager.AttackSE(1);
        }
        else
        {
            battleSoundManager.AttackSE(0);
        }


    }

    void TurnCountBonus()
    {
        Debug.Log("経過ターン分ダメージ増加" + isStrong);

        int addDamage = gameManager.turnCount * 15; 

        if (isStrong)
        {
            addDamage /= 2;
            addDamage *= 3;
        }

        Debug.Log("追加ダメージ" + addDamage);

        defender.hp -= addDamage;
        uiManager.ShowDamageText(addDamage, defender.playerIndex, false);

        effectMessage = "経過ターン分の追加ダメージ！";


        if (damageCal >= 300)
        {
            battleSoundManager.AttackSE(2);
        }
        else if (damageCal >= 150)
        {
            battleSoundManager.AttackSE(1);
        }
        else
        {
            battleSoundManager.AttackSE(0);
        }

    }










    //お互い料理した時、カロリー増加
    void DishAttack()
    {
        if (isSatisfied)
        {
            effectMessage = "お互いが料理しているため" + "\nカロリーが増加した！";
            isSatisfied = false;
        }

        
    }

    //状態異常特攻
    void ConditionAttack()
    {
        Debug.Log("状態異常特攻" + isStrong);

        if (isSatisfied)
        {
            effectMessage = defenderName + " が状態異常であるため" + "\nカロリーが増加した！";
            isSatisfied = false;
        }

    }

    void BadIngredientAttack()
    {
        if (isSatisfied)
        {
            effectMessage = defenderName + " が悪い食材を持っているため" + "\nカロリーが増加した！";
            isSatisfied = false;
        }
    }






  

    //相手を毒に
    void Poison()
    {
        Debug.Log("毒付与" + isStrong);

        if (isStrong)
        {
            defender.poisonCount += 5;
        }
        else
        {
            defender.poisonCount += 3;
        }

        if (defender.poisonCount > 9)
        {
            defender.poisonCount = 9;
        }

        uiManager.ShowCondition();

        effectMessage = defenderName + " は毒になった！";

        if (!isAttacker)
        {
            battleSoundManager.SpecialSE(1);
        }
    }


    //相手を暗闇に
    void Dark()
    {
        Debug.Log("暗闇付与" + isStrong);

        if (isStrong)
        {
            defender.darkCount += 5;
        }
        else
        {
            defender.darkCount += 3;
        }

        if (defender.darkCount > 9)
        {
            defender.darkCount = 9;
        }

        uiManager.ShowCondition();

        effectMessage = defenderName + " は暗闇になった！";

        if (!isAttacker)
        {
            battleSoundManager.SpecialSE(1);
        }

    }

    //相手を暗闇に
    void Clumsy()
    {
        Debug.Log("不器用付与" + isStrong);

        if (isStrong)
        {
            defender.clumsyCount += 5;
        }
        else
        {
            defender.clumsyCount += 3;
        }

        if (defender.clumsyCount > 9)
        {
            defender.clumsyCount = 9;
        }

        uiManager.ShowCondition();

        effectMessage = defenderName + " は不器用になった！";

        if (!isAttacker)
        {
            battleSoundManager.SpecialSE(1);
            //gameManager.timeLimit = 15f;
        }

    }


    void ReduceDamage()
    {
        Debug.Log("このターン受けるダメージ減少");

        if (isStrong)
        {
            attacker.reduceDamage = 2;
        }
        else
        {
            attacker.reduceDamage = 1;
        }

        effectMessage = "このターン" + attackerName + "が受けるダメージが減少する！";

        if (isAttacker)
        {
            battleSoundManager.SpecialSE(0);
        }
    }

    void DamageCounter()
    {
        Debug.Log("カウンター");


        effectMessage = attackerName + " は受けたダメージを返した！";
    }

    void InvalidateEffect()
    {
        Debug.Log("相手の効果を無効にする");
                
        

        if (isStrong)
        {
            gameManager.eatCardController[defender.playerIndex].model.cardID = -1;
            effectMessage = defenderName + " の効果が無効になった！";
            battleSoundManager.SpecialSE(1);
        }
        else
        {
            if (gameManager.eatCardController[defender.playerIndex].model.kind == KIND.DISH)
            {
                gameManager.eatCardController[defender.playerIndex].model.cardID = -1;

                effectMessage = defenderName + " の料理効果が無効になった！";
                battleSoundManager.SpecialSE(1);


            }
        }

        

    }


    //与えたダメージで回復
    void DamageHealHp()
    {
        Debug.Log("与えたダメージで回復" + isStrong);

        int healCal = -1;

        if (isStrong)
        {
            healCal = damageCal;
        }
        else
        {
            healCal = damageCal / 2;
        }

        if (attacker.maxHp < attacker.hp + healCal)
        {
            healCal = attacker.maxHp - attacker.hp;
        }

        attacker.hp += healCal;

        uiManager.ShowHealText(healCal, isAttacker);

        if (isAttacker)
        {
            effectMessage = "HPを吸収した！";
            battleSoundManager.SpecialSE(0);
        }
        else
        {
            effectMessage = "HPを吸収された！";
            battleSoundManager.SpecialSE(1);
        }
        

    }



    //状態異常解除
    void HealCondition()
    {
        Debug.Log("状態異常解除" + isStrong);

        if (attacker.poisonCount == 0 && attacker.darkCount == 0 && attacker.clumsyCount == 0)
        {
            return;
        }

        if (isStrong)
        {
            defender.poisonCount += attacker.poisonCount;
            if (defender.poisonCount > 9)
            {
                defender.poisonCount = 9;
            }

            defender.darkCount += attacker.darkCount;
            if (defender.darkCount > 9)
            {
                defender.darkCount = 9;
            }

            defender.clumsyCount += attacker.clumsyCount;
            if (defender.clumsyCount > 9)
            {
                defender.clumsyCount = 9;
            }
        }
 

        attacker.poisonCount = 0;
        attacker.darkCount = 0;
        attacker.clumsyCount = 0;

        if (isStrong)
        {
            if (isAttacker)
            {
                effectMessage = "状態異常があいてにうつった！";
                battleSoundManager.SpecialSE(0);
            }
            else
            {
                effectMessage = "状態異常をうつされてしまった！";

                //if (defender.clumsyCount > 0)
                //{
                //    gameManager.timeLimit = 15f;
                //}
                battleSoundManager.SpecialSE(1);
            }
        }
        else
        {
            effectMessage = attackerName + " の状態異常が治った！";
            battleSoundManager.SpecialSE(0);
        }

        //if (isAttacker)
        //{
        //    gameManager.timeLimit = setting.timeLimit;
        //}
       
    }


    //HP回復
    void HealHp()
    {
        Debug.Log("回復" + isStrong);

        int healCal = 0;

        if (isStrong)
        {
            healCal = 200;
        }
        else
        {
            healCal = 100;
        }

        if (attacker.maxHp < attacker.hp + healCal)
        {
            healCal = attacker.maxHp - attacker.hp;
        }

        attacker.hp += healCal;

        uiManager.ShowHealText(healCal, isAttacker);

        effectMessage = attackerName + " のHPが回復した！";

  
        battleSoundManager.SpecialSE(0);
        

    }

    

    //ランダム効果
    void RandomEffect()
    {
        Debug.Log("ランダム");



        effectMessage = "ランダムで効果が発動する！";

    }

    //相手の食材を弱体化
    void WeakenIngredient(string kind)
    {
        bool trigger = false;

        for (int i = 0; i < 4; i++)
        {
            if (weakenIndex[i] != -1)
            {
                trigger = true;

                CardController cardController = null;


                if (gameManager.handTransform[defender.playerIndex, weakenIndex[i]].childCount != 0)
                {
                    cardController = gameManager.handTransform[defender.playerIndex, weakenIndex[i]].GetChild(0).GetComponent<CardController>();
                }

                //if (isAttacker)
                //{
                //    if (gameManager.handTransform[1, weakenIndex[i]].childCount != 0)
                //    {
                //        cardController = gameManager.handTransform[1, weakenIndex[i]].GetChild(0).GetComponent<CardController>();
                //    }                    
                //}
                //else
                //{
                //    if (gameManager.handTransform[0, weakenIndex[i]].childCount != 0)
                //    {
                //        cardController = gameManager.handTransform[0, weakenIndex[i]].GetChild(0).GetComponent<CardController>();
                //    }                    
                //}

                //一律でバットに
                cardController.model.condition = 0;

                cardController.GetComponent<CardView>().Refresh(cardController.model);
 
            }

        }


        for (int i = 0; i < 4; i++)
        {
            weakenIndex[i] = -1;
        }

        if (trigger)
        {
            effectMessage = defenderName + " の" + kind + "が悪くなった！";
            battleSoundManager.SpecialSE(1);
        }
        else
        {            
            effectMessage = defenderName + " は" + kind + "を持っていなかった！";
        }
        
        
    }


}
