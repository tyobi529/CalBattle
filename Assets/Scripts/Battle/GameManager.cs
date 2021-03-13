using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    //ソロプレイはtrue
    public bool single = false;
    //チュートリアルはtrue
    public bool tutorial { get; set; } = false;
    [SerializeField] TutorialManager tutorialManager = null;
    public int tutorialNum = 0;
    bool endTutorial = false;

    public string[] playerName { get; set; } = new string[2] { "かり1", "CPU" };

    GamePlayerManager[] player = new GamePlayerManager[2] { null, null };

    //0:青
    //1:ピンク
    public int[] playerColor { get; set; } = new int[2] { -1, -1 };


    int[,] deck = new int[2, 9] { { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, { 0, 1, 2, 3, 4, 5, 6, 7, 8 } };
    int[] cardCost = new int[2] { 1, 1 };

    UIManager uiManager = null;



    Transform[] boardTransform = new Transform[2] { null, null };
    public Transform[,] handTransform { get; set; } = new Transform[2, 4] { { null, null, null, null }, { null, null, null, null } };


    Transform[] mixFieldTransform = new Transform[3] { null, null, null };



    Transform cookFieldTransform = null;




    //時間管理
    //private int timeLimit = 0;
    public float timeLimit { get; set; } = 0f;
    float timeCount = 0f;
    //float showNumTime = 0f;
    //int showNum = 5;
    bool isCountDown = false;
    bool isWarning = false;


    //選択時間
    float[] selectTime = new float[2] { 0f, 0f };

    int playerID = -1;




    SpecialController specialController = null;


    MessageController messageController = null;

    SelectController selectController = null;

    CardGenerator cardGenerator = null;

    GameObject attackButtons = null;


    private int maxHand = 0;


    int[] playerHp = new int[2] { 0, 0 };

    //int defaultMaxHand;


    int[] cardIndex = new int[2] { 0, 0 };

    //int damageCal;


    CardController[,] mixCardController = new CardController[2, 2] { { null, null }, { null, null } };



    public CardController[] eatCardController { get; set; } = new CardController[2] { null, null };



    //合成に必要なコスト
    private int needCost = 0;
    //貯まる最大コスト
    private int maxCost = 0;


    public int turnCount { get; private set; }



    public int additionalTurn { get; set; }


    //0：効果なし
    //1：色効果
    //2：種類効果
    int effectCount = 0;



    Vector2[] mixCardPosition = new Vector2[2];

    //GameObject vsObj;

    CharacterController characterController = null;

    //設定
    Setting setting;

    //ターンエンドしたかどうか    
    public bool[] isTurnEnd { get; set; } = new bool[2] { false, false };

    //攻撃が終了したかどうか
    public bool[] isAttackEnd { get; set; } = new bool[2] { false, false };

    //バトルが終了したかどうか
    public bool[] isBattleEnd { get; set; } = new bool[2] { false, false };

    //再戦ボタンを押したかどうか
    public bool[] isRestartEnd { get; set; } = new bool[2] { false, false };

    //このターンすでに料理したかどうか
    private bool[] cooked = new bool[2] { false, false };

    public bool isAttacker { get; set; } = false;


    Transform canvasTransform = null;

    CardController[,] handCardController = new CardController[2, 4];

    //int[] selectCardIndex = new int[2] { -1, -1 };

    int[,] selectCardIndex = new int[2, 2];

    Transform[] eatCardFieldTransform = new Transform[2];


    CookingController cookingController;

    //private int attackerIndex;

    //int[] playerSkin = new int[2] { -1, -1 };


    bool isRandom = false;


    //Transform[] eatFieldTransform = new Transform[2];

    GameAnimationController gameAnimationController;


    BattleAI battleAI = null;

    ColorSetting colorSetting = null;

    Transform kitchenTransform;
    int cpuLevel = -1;


    private IEnumerator countDownCoroutine;

    private PauseController pauseController = null;


    public bool surrender { get; set; } = false;

    bool win = false;

    float checkPlayerTime = 0f;


    //作った料理を記録する
    List<int> cookedDishNum = new List<int>();


    BattleSoundManager battleSoundManager = null;

    bool leaving = false;

    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        player[0] = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        player[1] = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas_0").GetComponent<UIManager>();

        for (int i = 0; i < 2; i++)
        {
            boardTransform[i] = GameObject.Find("Board_" + i.ToString()).transform;

            for (int j = 0; j < 4; j++)
            {
                handTransform[i, j] = boardTransform[i].GetChild(j);
            }


        }


        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;


        }



        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        selectController = GameObject.Find("SelectController").GetComponent<SelectController>();

        messageController = GameObject.Find("MessageController").GetComponent<MessageController>();

        cardGenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();

        attackButtons = GameObject.Find("AttackButtons");



  

        characterController = GameObject.Find("Characters").GetComponent<CharacterController>();

        setting = GameObject.Find("Setting").GetComponent<Setting>();

        colorSetting = GameObject.Find("Setting").GetComponent<ColorSetting>();



        playerName[0] = PlayerPrefs.GetString("NAME", "なまえなし");


        canvasTransform = GameObject.Find("Canvas_0").transform;


        cookingController = GameObject.Find("CookingController").GetComponent<CookingController>();

        gameAnimationController = GameObject.Find("GameAnimationController").GetComponent<GameAnimationController>();

        kitchenTransform = GameObject.Find("Kitchen").transform;


        timeCount = timeLimit;

        pauseController = GameObject.Find("PauseController").GetComponent<PauseController>();

        battleSoundManager = GameObject.Find("BattleSoundManager").GetComponent<BattleSoundManager>();
    }

    void Start()
    {        

        if (single)
        {
            playerID = 0;
            battleAI = transform.GetComponent<BattleAI>();
        }
        else
        {
            playerID = PhotonNetwork.LocalPlayer.ActorNumber;

            //IDを１つ減らして扱う
            playerID--;
        }


        if (single)
        {
            if (SoundManager.instance.isTutorial)
            {
                tutorial = true;
            }
            else
            {
                tutorial = false;
            }

        }

        //テスト
        //tutorial = true;


        if (single)
        {
            playerColor[0] = 0;
            playerColor[1] = 1;
        }
        else
        {
            //1p:青
            //2p:ピンク
            playerColor[0] = playerID;
        }

        //if (setting.isSolo)
        //{
        //    photonView.RPC(nameof(SetGame), RpcTarget.AllViaServer);

        //    Debug.Log("1人用");
        //}

        //１p青2pピンク
        //playerSkin[0] = playerID;

        if (playerID == 0)
        {
            //青色
            //playerSkin[0] = 0;
        }


        if (single)
        {
            cpuLevel = PlayerPrefs.GetInt("CPULEVEL", 3);

            if (cpuLevel < 1)
            {
                cpuLevel = 3;
            }


            battleAI.SetCpuLevel(cpuLevel, tutorial);
            SetGame();
        }
        else 
        {
            if (playerID == 1)
            {
                //playerSkin[0] = 1;


                photonView.RPC(nameof(SetGame), RpcTarget.AllViaServer);

                Debug.Log("プレイヤー２ログイン");
            }

        }



    }


    private void Update()
    {
        if (isCountDown)
        {
            //float showNumberTime = 5f;
            //int showNumber = 5;

            timeCount -= Time.deltaTime;

            if (timeCount < -0.2f)
            {
                isCountDown = false;
                //timeCount = timeLimit;
                OnDecideButton();
            }

            //if (timeCount < showNumTime)
            //{
            //    //uiManager.UpdateTime(showNum);
            //    //uiManager.UpdateTime((int)showNumTime);

            //    if ((int)showNumTime == 4)
            //    {
            //        //uiManager.Warning(true);
            //        StartCoroutine(uiManager.warningCoroutine);
            //    }

            //    showNumTime -= 1f;
            //    //showNum -= 1;
            //}

            if (!isWarning)
            {
                if (timeCount < 5f)
                {
                    isWarning = true;
                    StartCoroutine(uiManager.warningCoroutine);
                }
            }

            
            uiManager.UpdateTime(timeCount);


        }


    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (leaving)
        {
            return;
        }

        //相手が切断
        Debug.Log("相手が切断");
        uiManager.disconnectPanel.SetActive(true);

        LeaveGame();
    }


    //初期データの配置
    //再戦時も変わらない部分
    [PunRPC]
    void SetGame()
    {
        if (single)
        {
            string cpuName = null;

            if (tutorial)
            {
                cpuName = "チュートリアル";
            }
            else
            {
                switch (cpuLevel)
                {
                    case 1:
                        cpuName = "COM(よわい)";
                        break;
                    case 2:
                        cpuName = "COM(ふつう)";
                        break;
                    case 3:
                        cpuName = "COM(つよい)";
                        break;
                }
            }


            SharePlayerData(cpuName, playerColor[1]);
        }
        else
        {
            //データ共有
            photonView.RPC(nameof(SharePlayerData), RpcTarget.Others, playerName[0], playerColor[0]);
        }


        needCost = setting.needCost;
        maxCost = setting.maxCost;

        timeLimit = setting.timeLimit;

        //maxHand = setting.maxHand;
        maxHand = 4;

        messageController.SetPlayerName(playerName);
        specialController.SetGameManager(single);

        pauseController.SetPauseMenu(single);


        uiManager.HideDish();


        if (setting.debugBattle)
        {
            StartBattle();
        }



    }


    public void StartBattle()
    {
        Debug.Log("battleStart");
        gameAnimationController.MoveVsBG();
    }

    //ゲーム開始準備
    //再戦時はここからスタート
    [PunRPC]
    public void PrepareGame()
    {

        SoundManager.instance.StartBGM("Battle");


        StopAllCoroutines();
        gameAnimationController.StopAllCoroutines();
        messageController.StopAllCoroutines();


        //チュートリアルはカウントダウンなし
        if (tutorial)
        {
            isCountDown = false;
        }

        uiManager.ShowPauseButton(true);


        selectController.ResetSelect();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < maxHand; j++)
            {
                if (handTransform[i, j].childCount != 0)
                {
                    Destroy(handTransform[i, j].GetChild(0).gameObject);
                }

            }
        }


        
        timeCount = timeLimit;


        isRestartEnd[0] = false;
        isRestartEnd[1] = false;

        uiManager.HideResultPanel();


        surrender = false;
        win = false;
        //drawNum = maxHand;

        cookedDishNum.Clear();
        Debug.Log(cookedDishNum.Count);


        for (int i = 0; i < 2; i++)
        {
            player[i].hp = setting.defaultHp;
            player[i].cost = setting.defaultCost;
            playerHp[i] = player[i].hp;
            player[i].playerIndex = i;

            cardIndex[i] = 0;
            cardCost[i] = 1;

            //isTurnEnd[i] = false;
            //isAttackEnd[i] = false;
            //isBattleEnd[i] = false;
            //isRestartEnd[i] = false;

            ShuffleCard(i);
        }        
        
        selectController.CleanField();

    
        turnCount = 0;



        //uiManager.ShowTurnText(turnCount);

        ResetCondition();

        player[0].darkCount = setting.darkCount;
        player[0].poisonCount = setting.poisonCount;
        player[0].clumsyCount = setting.clumsyCount;


        UpdateStatus();


        gameAnimationController.rainEffect.SetActive(false);

        if (tutorial)
        {
            turnCount = 1;
            //説明パネルの表示
            tutorialManager.ShowTutorial(0);
        }
        else
        {
            //gameAnimationController.MoveTurnObj(false, 0);

            EndStartAnimation();
        }




    }




    public void OnLeaveButton()
    {
        if (single)
        {
            LeaveGame();
        }
        else
        {
            photonView.RPC(nameof(LeaveGame), RpcTarget.All);
        }
        


    }

    [PunRPC]
    public void LeaveGame()
    {
        leaving = true;


        uiManager.ShowLeave();


        DOVirtual.DelayedCall(3, () =>
        {
            //FadeManager.FadeOut(1);
            ReturnMenu();

        });

 
    }

    public void OnRestartGameButton()
    {

        if (single)
        {
            PrepareGame();

        }
        else
        {
            uiManager.WaitingRestart();

            photonView.RPC(nameof(RestartEnd), RpcTarget.AllViaServer, playerID);
        }
   


    }


    //相手に再戦を伝える
    [PunRPC]
    void RestartEnd(int playerID)
    {
        if (this.playerID == playerID)
        {
            isRestartEnd[0] = true;

            if (isRestartEnd[1])
            {
                photonView.RPC(nameof(PrepareGame), RpcTarget.AllViaServer);
            }
        }
        else
        {
            isRestartEnd[1] = true;

        }
    }


    //対戦開始の演出終了後
    public void EndStartAnimation()
    {
        if (single)
        {
            MoveNextTurn();
        }
        else
        {
            photonView.RPC(nameof(PrepareEnd), RpcTarget.AllViaServer, playerID);
        }
        
    }


    //相手に準備完了を伝える
    [PunRPC]
    void PrepareEnd(int playerID)
    {

        if (this.playerID == playerID)
        {
            isTurnEnd[0] = true;
    
            if (isTurnEnd[1])
            {
                //photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);
                photonView.RPC(nameof(MoveNextTurn), RpcTarget.AllViaServer);
            }
        }
        else
        {
            isTurnEnd[1] = true;

        }
    }


    //相手にターンエンドを伝える
    [PunRPC]
    void TurnEnd(int playerID, int[] index, float selectTime)
    {
        
        if (this.playerID == playerID)
        {
            isTurnEnd[0] = true;

            Debug.Log(selectTime);

            this.selectTime[0] = selectTime;

            for (int i = 0; i < 2; i++)
            {
                selectCardIndex[0, i] = index[i];
            }

            if (isTurnEnd[1])
            {
                //isAttacker = false;
                photonView.RPC(nameof(StartCooking), RpcTarget.AllViaServer);
            }
            else
            {
                //isAttacker = true;

                //通信待機メッセージ
                messageController.messagePanel.SetActive(true);
                messageController.WaitingMessage();
            }
        }
        else
        {
            isTurnEnd[1] = true;

            this.selectTime[1] = selectTime;

            for (int i = 0; i < 2; i++)
            {
                selectCardIndex[1, i] = index[i];
            }
        }

    }


    [PunRPC]
    void StartCooking()
    {


        //タイマー消す
        uiManager.ShowTimer(false);
        uiManager.UpdateTime(0);


        for (int i = 0; i < 4; i++)
        {
            selectController.cursorObj[i].SetActive(false);
        }
        

        messageController.messagePanel.SetActive(false);



        //selectController.CleanField();


        GenerateMixCard();


        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(cookingController.Cooking(i, mixCardController, eatCardController));
        }



    }

    //[PunRPC]
    void DestroyUsedCard()
    {

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (selectCardIndex[i, j] != -1)
                {
                    Destroy(handTransform[i, selectCardIndex[i, j]].GetChild(0).gameObject);
                }

                selectCardIndex[i, j] = -1;
            }
        }



    }

    [PunRPC]
    void SharePlayerData(string name, int color)
    {
        playerName[1] = name;
        playerColor[1] = color;

        uiManager.SetName(playerName);

        characterController.SetCharacterSkin(playerColor);

        colorSetting.SetColorObj(playerColor);

    }

    [PunRPC]
    void GenerateHandCard(int playerID, int handIndex, int cardID, int cost, int condition)
    {

        int playerIndex = -1;
        bool isMyCard = false;
        if (this.playerID == playerID)
        {
            playerIndex = 0;
            isMyCard = true;
        }
        //相手のカードは選択不可にする
        else
        {
            playerIndex = 1;
            isMyCard = false;
        }


        if (handTransform[playerIndex, handIndex].childCount != 0)
        {
            Debug.Log("すでに手札あり" + playerIndex + " " + handIndex);
            //cardIndex[playerIndex]--;
            return;
        }

        CardController cardController = cardGenerator.CreateCard(false, cardID, cost, condition, handTransform[playerIndex, handIndex]);        
        cardController.GetComponent<CardView>().SetCard(cardController.model);
        cardController.model.index = handIndex;

        //cardController.model.canSelected = isMyCard;
        cardController.GetComponent<CanvasGroup>().blocksRaycasts = isMyCard;


        cardController.GetComponent<CardView>().ChangeAnimation(isMyCard);

        handCardController[playerIndex, handIndex] = cardController;

        Vector3[] path = new Vector3[2];

        path[0] = new Vector2(handTransform[playerIndex, handIndex].position.x, 0f);

        path[1] = new Vector2(handTransform[playerIndex, handIndex].position.x, handTransform[playerIndex, handIndex].position.y);

        Vector3 scale = new Vector3(1f, 1f, 1f);
        MoveCardTween(cardController, path, scale, 1f);



        bool move = false;
        for (int j = 0; j < maxHand; j++)
        {
            if (handTransform[0, j].childCount == 0)
            {
                break;
            }

            if (j == maxHand - 1)
            {
                //動かす
                move = true;
            }
        }
        if (move)
        {
            for (int i = 0; i < 4; i++)
            {
                handTransform[0, i].GetChild(0).GetComponent<CardView>().ChangeAnimation(true);
            }
        }
        
    }

    public void MoveCardTween(CardController cardController, Vector3[] path, Vector3 scale, float time)
    {
        cardController.transform.position = path[0];

        cardController.transform.DOLocalPath(path, time, PathType.CatmullRom)
                        .SetEase(Ease.OutBounce);

        cardController.transform.DOScale(scale, time);

    }


    void GiveCardToHand(int playerID, int handIndex)
    {
        //playerIndex = 0で読んでいるため
        int playerIndex = playerID;

        //例外
        if (handTransform[playerIndex, handIndex].childCount != 0)
        {
            Debug.Log("すでに生成済み");            
            return;
        }


        int cardID = deck[playerIndex, cardIndex[playerIndex]];
        int cost = cardCost[playerIndex];

        cardCost[playerIndex]++;
        if (cardCost[playerIndex] > 3)
        {
            cardCost[playerIndex] = 1;
        }
        int condition = -1;


        //チュートリアルの操作
        if (tutorial)
        {
            if (tutorialNum == 2)
            {
                condition = 2;
            }
            else
            {
                condition = 1;
            }
        }
        else
        {
            if (cost == 3)
            {
                condition = 2;
            }
            else
            {
                condition = 1;
            }
        }


        if (single)
        {
            GenerateHandCard(playerID, handIndex, cardID, cost, condition);
        }
        else
        {
            photonView.RPC(nameof(GenerateHandCard), RpcTarget.AllViaServer, playerID, handIndex, cardID, cost, condition);
        }
        


        if (cardIndex[playerIndex] == 8)
        {
            ShuffleCard(playerIndex);
            cardIndex[playerIndex] = 0;
        }
        else
        {
            cardIndex[playerIndex]++;

        }

    }




    public void OnDecideButton()
    {
        if (isTurnEnd[0])
        {
            return;
        }



        isCountDown = false;
        uiManager.ShowTimer(false);


        StopCoroutine(uiManager.warningCoroutine);
        Color color = uiManager.warningBGImage.color;
        color.a = 0f;
        uiManager.warningBGImage.color = color;

     

        //全てのカードを選択不可に
        for (int i = 0; i < maxHand; i++)
        {
            handTransform[0, i].GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        bool timeOver = false;

        //料理カードが存在
        if (mixFieldTransform[2].childCount != 0)
        {
            //選択不可のカードなら
            //コスト不足
            if (!mixFieldTransform[2].GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts)
            {

                //２枚選択されているならキャンセルして０のカード選択
                if (selectController.selectCardController[1] != null)
                {
                    timeOver = true;
                    //selectController.ResetSelect();
                    //handTransform[0, 0].GetChild(0).GetComponent<CardController>().OnCardObject();
                }

            }

            
        }
        else
        {
            timeOver = true;
            //selectController.ResetSelect();
            //handTransform[0, 0].GetChild(0).GetComponent<CardController>().OnCardObject();


        }

        if (timeOver)
        {
            selectController.ResetSelect();
            handTransform[0, 0].GetChild(0).GetComponent<CardController>().OnCardObject();
        }


        mixFieldTransform[2].GetChild(0).GetComponent<EatCardView>().StopBlinking();
        mixFieldTransform[2].GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = false;




        //食材のアニメーション止める
        for (int i = 0; i < maxHand; i++)
        {
            handTransform[0, i].GetChild(0).GetComponent<CardView>().ChangeAnimation(false);
        }





        int[] index = new int[2] { -1, -1 };

        for (int i = 0; i < 2; i++)
        {
            if (selectController.selectCardController[i] != null)
            {
                index[i] = selectController.selectCardController[i].model.index;
                selectController.selectCardController[i] = null;
            }

        }

        if (single)
        {
            int[] enemySelectIndex = battleAI.SelectCard();
            for (int i = 0; i < 2; i++)
            {
                selectCardIndex[0, i] = index[i];
                selectCardIndex[1, i] = enemySelectIndex[i];
            }
            

            StartCooking();
        }
        else
        {
            float selectTime = timeLimit - timeCount;
            photonView.RPC(nameof(TurnEnd), RpcTarget.AllViaServer, playerID, index, selectTime);
        }



        if (tutorial)
        {
            gameAnimationController.tutorialTextObj.SetActive(false);
        }

    }



    //生成のみ
    void GenerateMixCard()
    {

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                mixCardController[i, j] = null;
            }

            bool isStrong = false;

            for (int j = 0; j < 2; j++)
            {


                if (selectCardIndex[i, j] != -1)
                {

                    CardController selectCardController = handTransform[i, selectCardIndex[i, j]].GetChild(0).GetComponent<CardController>();



                    int cardID = selectCardController.model.cardID;
                    int cost = selectCardController.model.cost;
                    int condition = selectCardController.model.condition;


                    mixCardController[i, j] = cardGenerator.CreateCard(false, cardID, cost, condition, uiManager.transform);
                    mixCardController[i, j].GetComponent<CardView>().SetCookingCard(mixCardController[i, j].model);

                    //mixCardController[i, j].model.canSelected = false;
                    mixCardController[i, j].GetComponent<CanvasGroup>().blocksRaycasts = false;




                    if (mixCardController[i, j].model.condition == 2)
                    {
                        isStrong = true;
                    }

                    mixCardController[i, j].gameObject.SetActive(false);
                }
            }

            if (mixCardController[i, 1] != null)
            {
                //合成
                int mixCardID = cardGenerator.SpecialMix(mixCardController[i, 0], mixCardController[i, 1]);


                eatCardController[i] = cardGenerator.CreateDishCard(true, mixCardID, 0, 0, canvasTransform);

                eatCardController[i].model.isStrong = isStrong;
                eatCardController[i].GetComponent<CardView>().SetDishCard(eatCardController[i].model, i);
                
            }
            else
            {
                int cardID = mixCardController[i, 0].model.cardID;
                int cost = mixCardController[i, 0].model.cost;
                //bool isRare = mixCardController[i, 0].model.isRare;
                int condition = mixCardController[i, 0].model.condition;

                eatCardController[i] = cardGenerator.CreateCard(false, cardID, cost, condition, eatCardFieldTransform[i]);
                eatCardController[i].GetComponent<CardView>().SetCookingCard(eatCardController[i].model);

                
            }


            eatCardController[i].gameObject.SetActive(false);





        }

        DestroyUsedCard();




    }

    public void EndCooking()
    {
        selectController.CleanField();

        //作った料理の記録
        if (eatCardController[0].model.kind == KIND.DISH)
        {
            cookedDishNum.Add(eatCardController[0].model.cardID);
        }


        for (int i = 0; i < 2; i++)
        {
            if (eatCardController[i].model.kind == KIND.DISH)
            {
                //コスト減少
                player[i].cost -= needCost;

                if (setting.specialDebug)
                {
                    if (single)
                    {
                        eatCardController[i].model.cardID = setting.specialID[i];
                    }
                    else
                    {
                        if (playerID == 0)
                        {
                            eatCardController[i].model.cardID = setting.specialID[i];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                eatCardController[i].model.cardID = setting.specialID[1];
                            }
                            else
                            {
                                eatCardController[i].model.cardID = setting.specialID[0];
                            }
                        }

                        eatCardController[i].model.isStrong = setting.isStrong;
                    }
       

                }
            }

            if (setting.calDebug)
            {
                if (single)
                {
                    eatCardController[i].model.cal = setting.cal[i];
                }
                else
                {
                    if (playerID == 0)
                    {
                        eatCardController[i].model.cal = setting.cal[i];                        
                    }
                    else
                    {
                        if (i == 0)
                        {
                            eatCardController[i].model.cal = setting.cal[1];                            
                        }
                        else
                        {
                            eatCardController[i].model.cal = setting.cal[0];
                        }
                    }                    
                }
            }
        }

        Sprite[] dishSprite = new Sprite[2];

        for (int i = 0; i < 2; i++)
        {

            dishSprite[i] = eatCardController[i].model.icon;
            
        }

        uiManager.ShowDish(dishSprite);


        //まずは時間で判断する
        //一旦ソロはプレイヤーは先行
        if (single)
        {
            isAttacker = true;
        }
        else
        {
            Debug.Log("自分" + selectTime[0]);
            Debug.Log("相手" + selectTime[1]);

            if (selectTime[0] < selectTime[1])
            {
                isAttacker = true;
            }
            else if (selectTime[0] > selectTime[1])
            {
                isAttacker = false;
            }
            else
            {
                //念の為
                if (playerID == 0)
                {
                    isAttacker = true;
                }
                else
                {
                    isAttacker = false;
                }
            }
            
        }



        //0なし
        //-1後攻
        //1先行
        int[] priority = new int[2] { 0, 0 };




        for (int i = 0; i < 2; i++)
        {
            int[] cardID = new int[2] { -1, -1 };


            if (eatCardController[i].model.kind == KIND.DISH)
            {
                cardID[i] = eatCardController[i].model.cardID;

                if (cardID[i] == 21 || cardID[i] == 23)
                {
                    priority[i] = 1;
                }
                else if (cardID[i] == 22)
                {
                    priority[i] = -1;
                }
            }
 
   
        }

        if (priority[0] > priority[1])
        {
            isAttacker = true;
        }
        else if (priority[0] < priority[1])
        {
            isAttacker = false;
        }


        uiManager.ShowCost();


        if (single)
        {
            DamageCalculation();

        }
        else
        {
            if (isAttacker)
            {
                DamageCalculation();
            }
        }


    }

   


    //基礎カロリー、麻痺、ギャンブル効果の計算
    //攻撃側でよぶ
    public void DamageCalculation()
    {
        GamePlayerManager attacker = null;
        GamePlayerManager defender = null;

        if (isAttacker)
        {
            attacker = player[0];
            defender = player[1];
        }
        else
        {
            attacker = player[1];
            defender = player[0];
        }


        int damageCal = eatCardController[attacker.playerIndex].model.cal;

        if (defender.reduceDamage == 1)
        {
            damageCal /= 3;
            damageCal *= 2;
        }
        else if (defender.reduceDamage == 2)
        {
            damageCal /= 2;
        }


        //攻撃前に計算する必要がある料理
        if (eatCardController[attacker.playerIndex].model.kind == KIND.DISH)
        {
            int attackerIndex = attacker.playerIndex;
            int defenderIndex = defender.playerIndex;

            int cardID = eatCardController[attacker.playerIndex].model.cardID;
            bool isStrong = eatCardController[attacker.playerIndex].model.isStrong;


            switch (cardID)
            {
                case 0:
                    int a = UnityEngine.Random.Range(1, 27);
                    if (single)
                    {
                        ShareRandomCardID(a, isStrong);
                    }
                    else
                    {
                        photonView.RPC(nameof(ShareRandomCardID), RpcTarget.AllViaServer, a, isStrong);
                    }
                    return;                    
                case 1:
                    Debug.Log("ギャンブル");
                    damageCal = GamblingAttack(isStrong, damageCal);
                    break;
                case 2:
                    RandomDamage(isStrong);
                    break;
                case 6:
                    ThrowEnemyRare(cardID, isStrong, defenderIndex);
                    break;
                case 7:
                    IncreaseMyRare(cardID, isStrong, attackerIndex);
                    break;
                case 8:
                    StealEnemyRare(cardID, isStrong, defenderIndex);
                    break;
                case 12:
                    damageCal = DishAttack(isStrong, damageCal);
                    break;
                case 13:
                    damageCal = ConditionAttack(isStrong, damageCal);
                    break;
                case 14:
                    damageCal = BadIngredientAttack(isStrong, damageCal, defenderIndex);
                    break;
                case 22:
                    int addDamage = playerHp[0] - player[0].hp;
                    if (isStrong)
                    {
                        addDamage *= 2;                        
                    }
                    else
                    {
                        addDamage /= 2;
                        addDamage *= 3;
                    }
                    damageCal += addDamage;
                    break;
                case 24:
                    WeakenIngredient(cardID, isStrong, defenderIndex);
                    break;
                case 25:
                    WeakenIngredient(cardID, isStrong, defenderIndex);
                    break;
                case 26:
                    WeakenIngredient(cardID, isStrong, defenderIndex);
                    break;
            }




        }

        if (single)
        {
            StartCoroutine(Battle(damageCal));
        }
        else
        {
            photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);
        }
        



    }


    



    [PunRPC]
    void Battle_RPC(int damageCal)
    {

        StartCoroutine(Battle(damageCal));
    }


    IEnumerator Battle(int damageCal)
    {

        characterController.ChangeIdleSprite();

        //メッセージカラー変更
        messageController.ChangeMessageColor(isAttacker);


        GamePlayerManager attacker;
        GamePlayerManager defender;

        int playerIndex = -1;

        if (isAttacker)
        {
            attacker = player[0];
            defender = player[1];
            playerIndex = 0;
        }
        else
        {
            attacker = player[1];
            defender = player[0];
            playerIndex = 1;
        }


        string attackerName = null;

        //「あなたの攻撃」テキスト
        if (isAttacker)
        {
            attackerName = playerName[0];
        }
        else
        {
            attackerName = playerName[1];
        }

        messageController.messagePanel.SetActive(true);



        if (!isRandom)
        {
            messageController.EatMessage(eatCardController[playerIndex].model, isAttacker);
            yield return new WaitForSeconds(2);

            if (eatCardController[playerIndex].model.isStrong)
            {
                messageController.RareDishMessage();
                characterController.ChangeRareSprite(playerIndex);

                BattleSoundManager.instance.PowerUpSE();
                yield return new WaitForSeconds(2);
            }
        }
        else
        {
            isRandom = false;
        }



        int attackNum = -1;

        if (eatCardController[playerIndex].model.kind == KIND.DISH)
        {
            attackNum = 1;


        }
        else
        {
            attackNum = 0;
        }

        messageController.AttackText(attackerName);


        yield return new WaitForSeconds(1);

        //ダメージテキスト
        defender.hp -= damageCal;
        uiManager.ShowDamageText(damageCal, defender.playerIndex, false);
        characterController.ChangeBattleSprite(isAttacker, attackNum);

        //画面揺らす
        //Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //camera.DOShakePosition(0.5f, 1, 1);

        if (damageCal >= 300)
        {
            kitchenTransform.DOShakePosition(0.5f, 100f, 10, 90, false, true);
            //BattleSoundManager.instance.AttackSE(2);
            battleSoundManager.AttackSE(2);
        }
        else if (damageCal >= 150)
        {
            kitchenTransform.DOShakePosition(0.2f, 30f, 10, 90, false, true);
            //BattleSoundManager.instance.AttackSE(1);
            battleSoundManager.AttackSE(1);
        }
        else
        {
            //BattleSoundManager.instance.AttackSE(0);
            battleSoundManager.AttackSE(0);
        }
        

        

        yield return new WaitForSeconds(2);





        //料理
        //if (eatCardController[i].model.kind == KIND.DISH)
        if (eatCardController[playerIndex].model.kind == KIND.DISH)
        {

            int cardID = eatCardController[playerIndex].model.cardID;
            bool isStrong = eatCardController[playerIndex].model.isStrong;

            StartCoroutine(specialController.DishEffect(cardID, isStrong, damageCal, isAttacker));


        }
        //素材
        else
        {


            if (eatCardController[playerIndex].model.cardID != -1)
            {
                attacker.cost += eatCardController[playerIndex].model.cost;
                if (attacker.cost > maxCost)
                {
                    attacker.cost = maxCost;
                }
            }


            CheckStatus();

        }



    }

    public void CheckStatus()
    {

        UpdateStatus();

        //どちらかのHPが０
        if (player[0].hp <= 0)
        {
            win = false;
            ShowResult();
            return;
        }
        else if (player[1].hp <= 0)
        {
            win = true;
            ShowResult();
            return;
        }


        if (single)
        {
            if (isAttacker)
            {
                isAttackEnd[0] = true;
                isAttacker = !isAttacker;                
            }
            else
            {
                isAttackEnd[1] = true;
                isAttacker = !isAttacker;
            }

            if (isAttackEnd[0] && isAttackEnd[1])
            {
                MoveNextTurn();
            }
            else
            {
                DamageCalculation();
            }
        }
        else
        {
            photonView.RPC(nameof(AttackEnd), RpcTarget.AllViaServer, playerID);

        }
    }


    [PunRPC]
    void AttackEnd(int playerID)
    {

        if (this.playerID == playerID)
        {
            isAttackEnd[0] = true;

            if (isAttacker)
            {
                isBattleEnd[0] = true;
            }
            else
            {
                isBattleEnd[1] = true;
            }

            if (isAttackEnd[1])
            {
                if (isBattleEnd[0] && isBattleEnd[1])
                {
                    photonView.RPC(nameof(MoveNextTurn), RpcTarget.AllViaServer);
                }
                else
                {
                    photonView.RPC(nameof(ChangeAttacker), RpcTarget.AllViaServer);
                }                
            }

        }
        else
        {
            isAttackEnd[1] = true;

        }
    }

    [PunRPC]
    void ChangeAttacker()
    {
        isAttacker = !isAttacker;

        for (int i = 0; i < 2; i++)
        {
            isAttackEnd[i] = false;
        }

        if (isAttacker)
        {
            DamageCalculation();
        }


    }



    [PunRPC]
    void BattleEnd(int playerID)
    {
        if (this.playerID == playerID)
        {
            isBattleEnd[0] = true;

            if (isBattleEnd[1])
            {
                //photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);
                photonView.RPC(nameof(MoveNextTurn), RpcTarget.AllViaServer);
            }

        }
        else
        {
            isBattleEnd[1] = true;
        }
    }


    [PunRPC]
    void MoveNextTurn()
    {
        uiManager.HideDish();


        turnCount++;

        characterController.ChangeIdleSprite();

        for (int i = 0; i < 2; i++)
        {
            if (eatCardController[i] != null)
            {
                Destroy(eatCardController[i].gameObject);
            }

        }

        messageController.messagePanel.SetActive(false);

        //uiManager.cookingObject.SetActive(false);

        messageController.ChangeMessageColor(true);


        //時間変化
        if (player[0].clumsyCount > 0)
        {
            timeLimit = 15f;
        }
        else
        {
            timeLimit = setting.timeLimit;
        }

        //gameAnimationController.MoveTurnObj(false);

        if (tutorial)
        {
            if (tutorialNum == 0)
            {
                if (player[0].cost >= 3)
                {
                    endTutorial = true;
                }
            }
            else
            {
                endTutorial = true;
            }

            if (endTutorial)
            {
                endTutorial = false;
                tutorialNum++;
                if (tutorialNum == 2)
                {
                    player[0].cost = 6;
                }
                tutorialManager.ShowTutorial(tutorialNum);
            }
            else
            {
                gameAnimationController.MoveTurnObj(true, 0);
            }
            
        }
        else
        {
            gameAnimationController.MoveTurnObj(false, 0);
        }
        
    }

   //次のターンへ移る
    public void ChangeTurn()
    {

        CheckPoison();
        CheckDark();
        CheckClumsy();

        UpdateStatus();

        //毒で止め
        if (player[0].hp <= 0)
        {
            win = false;
            ShowResult();
            return;
        }
        else if (player[1].hp <= 0)
        {
            win = true;
            ShowResult();
            return;
        }




        //if (photonView.IsMine)
        if (playerID == 0)
        {
            //手札補充
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < maxHand; j++)
                {
                    if (handTransform[i, j].childCount == 0)
                    {
                        GiveCardToHand(i, j);
 
                    }
                }
            }
        }



        //drawNum = 0;

  
        for (int i = 0; i < 2; i++)
        {
            if (player[i].cost < 0)
            {
                player[i].cost = 0;
            }
            else if (player[i].cost > maxCost)
            {
                player[i].cost = maxCost;
            }
        }



        for (int i = 0; i < 2; i++)
        {
            int damage = playerHp[i] - player[i].hp;
            Debug.Log("プレイヤー" + i + "のダメージ：　" + damage);
            playerHp[i] = player[i].hp;
        }

        Debug.Log("*************************************************************");

        Debug.Log("ターン " + turnCount);


        selectController.messageTextObj[0].SetActive(true);
        selectController.messageTextObj[1].SetActive(false);


        uiManager.ShowTurnText(turnCount);




        //キャラクターをIdleにする
        //characterController.ChangeSprite(1, isAttacker);

        for (int i = 0; i < 2; i++)
        {
            cooked[i] = false;
            isTurnEnd[i] = false;
            isAttackEnd[i] = false;
            isBattleEnd[i] = false;

            player[i].reduceDamage = 0;
        }



        characterController.ChangeCookSprite();

        //全てのカードを選択可能に
        for (int i = 0; i < maxHand; i++)
        {
            //handTransform[0, i].GetChild(0).GetComponent<CardController>().model.canSelected = true;
            if (handTransform[0, i].childCount != 0)
            {
                handTransform[0, i].GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
        }



        timeCount = timeLimit;
        //showNumTime = timeLimit - 1f;
        isWarning = false;



        if (tutorial)
        {
            isCountDown = false;

            //コストが３以上貯まる
            if (player[0].cost >= 3)
            {
                endTutorial = true;
            }
        }
        else
        {
            isCountDown = setting.isCountDown;
        }



        
    }







    void ShowResult()
    {
        StopAllCoroutines();
        isCountDown = false;




        StopCoroutine(uiManager.warningCoroutine);
        Color color = uiManager.warningBGImage.color;
        color.a = 0f;
        uiManager.warningBGImage.color = color;
        uiManager.ShowTimer(false);

        //状態異常の解除
        ResetCondition();
        UpdateStatus();



        uiManager.ShowPauseButton(false);

        selectController.ResetSelect();

        if (pauseController.PauseObj.activeSelf)
        {
            pauseController.OnBatsuButton();
        }


        for (int i = 0; i < 2; i++)
        {
            isTurnEnd[i] = false;
            isAttackEnd[i] = false;
            isBattleEnd[i] = false;
            isRestartEnd[i] = false;
        }

        //手札削除

        //selectController.ResetSelect();

        //for (int i = 0; i < 2; i++)
        //{
        //    for (int j = 0; j < maxHand; j++)
        //    {
        //        if (handTransform[i, j].childCount != 0)
        //        {
        //            Destroy(handTransform[i, j].GetChild(0).gameObject);
        //        }

        //    }
        //}

        //合成フィールド削除
        //selectController.CleanField();

        //食べるカード削除
        for (int i = 0; i < 2; i++)
        {
            if (eatCardController[i] != null)
            {
                Destroy(eatCardController[i].gameObject);
            }

        }


        uiManager.HideDish();

        messageController.messagePanel.SetActive(false);

        //サウンド消す
        //SoundManager.instance.MuteVolume();
        //SoundManager.instance.source.mute = true;
        SoundManager.instance.source.volume = 0f;

        gameAnimationController.ShowResultObj(win);


        //作った料理のセーブ
        foreach (int num in cookedDishNum)
        {
            PlayerPrefs.SetInt("COOKED_" + num, 1);
        }

        //勝利カウント
        if (win)
        {
            int winCount = PlayerPrefs.GetInt("WINCOUNT", 0);
            winCount++;
            PlayerPrefs.SetInt("WINCOUNT", winCount);

            if (single)
            {  
                PlayerPrefs.SetInt("WINCPU_" + (cpuLevel - 1), 1);
            }
        }
        


    }


   
    void ShuffleCard(int playerIndex)
    {
        Debug.Log("プレイヤー" + playerIndex + "のカードシャッフル");
        for (int i = 8; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck[playerIndex, i];
            deck[playerIndex, i] = deck[playerIndex, j];
            deck[playerIndex, j] = tmp;
        }



    }



    



    void CheckPoison()
    {
        bool trigger = false;
        //自分
        if (player[0].poisonCount > 0)
        {
            player[0].poisonCount--;
            player[0].hp -= 50;
            uiManager.ShowDamageText(50, player[0].playerIndex, true);
            trigger = true;
            Debug.Log("毒で自分" + 50 + "ダメージ");
        }

        if (player[1].poisonCount > 0)
        {
            player[1].poisonCount--;
            player[1].hp -= 50;
            uiManager.ShowDamageText(50, player[1].playerIndex, true);
            trigger = true;
            Debug.Log("毒で相手" + 50 + "ダメージ");
        }

        if (trigger)
        {
            battleSoundManager.AttackSE(0);
        }




    }

    void CheckDark()
    {
        //自分
        if (player[0].darkCount > 0)
        {
            player[0].darkCount--;
        }

        //相手
        if (player[1].darkCount > 0)
        {
            player[1].darkCount--;
        }


    }

    void CheckClumsy()
    {
        //自分
        if (player[0].clumsyCount > 0)
        {
            player[0].clumsyCount--;            
        }

        //相手
        if (player[1].clumsyCount > 0)
        {
            player[1].clumsyCount--;
        }

        if (player[0].clumsyCount > 0)
        {
            timeLimit = 15f;
        }
        else
        {
            timeLimit = setting.timeLimit;
        }

    }



    //HP、コスト、状態異常の見た目更新
    void UpdateStatus()
    {
        uiManager.ShowHP();
        uiManager.ShowCost();
        uiManager.ShowCondition();
    }

    //状態異常の解除
    private void ResetCondition()
    {
        for (int i = 0; i < 2; i++)
        {
            player[i].poisonCount = 0;
            player[i].darkCount = 0;
            player[i].clumsyCount = 0;
        }
    }


    //public void SurrenderGame()
    //{
    //    if (single)
    //    {
    //        ReturnMenu();
    //        //FadeManager.FadeOut(1);

    //    }
    //    else
    //    {
    //        photonView.RPC(nameof(SurrenderGame_RPC), RpcTarget.All, playerID);
    //    }
        
    //}

    //[PunRPC]
    //void SurrenderGame_RPC(int playerID)
    //{        

    //    //自分で降参
    //    if (this.playerID == playerID)
    //    {
    //        //ReturnMenu();
    //        //FadeManager.FadeOut(1);
    //        ReturnMenu();

    //    }
    //    else
    //    {
    //        player[1].hp = 0;
    //        win = true;            
    //    }

    //    surrender = true;
    //    ShowResult();

    //}

    public void ReturnMenu()
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }

        gameAnimationController.CompleteTween();
        cookingController.CompleteTween();
        uiManager.CompleteTween();

        FadeManager.FadeOut(1);

    }


    int GamblingAttack(bool isStrong, int damageCal)
    {
        int a = UnityEngine.Random.Range(0, 100);

        if (isStrong)
        {
            if (a < 20)
            {
                Debug.Log("2 ダメージが１になる");
                damageCal = 1;
            }
        }
        else
        {
            if (a < 60)
            {
                Debug.Log("0 ダメージが１になる");
                damageCal = 1;
            }
        }

        return damageCal;
    }


    [PunRPC]
    void ShareRandomCardID(int cardID, bool isStrong)
    {
        specialController.randomCardID = cardID;
        

        isRandom = true;
        StartCoroutine(specialController.DishEffect(0, isStrong, 0, isAttacker));
    }



    //ID=6
    [PunRPC]
    void ShareIngredientIndex(int cardID, int[] index)
    {
        switch (cardID)
        {
            case 6:
                for (int i = 0; i < 2; i++)
                {
                    specialController.throwIndex[i] = index[i];
                }
                break;
            case 7:
                for (int i = 0; i < 2; i++)
                {
                    specialController.rareIndex[i] = index[i];
                }
                break;
            case 8:
                for (int i = 0; i < 2; i++)
                {
                    specialController.stealIndex[i] = index[i];
                }
                break;
            case 24:
                for (int i = 0; i < 4; i++)
                {
                    specialController.weakenIndex[i] = index[i];
                }
                break;
            case 25:
                for (int i = 0; i < 4; i++)
                {
                    specialController.weakenIndex[i] = index[i];
                }
                break;
            case 26:
                for (int i = 0; i < 4; i++)
                {
                    specialController.weakenIndex[i] = index[i];
                }
                break;
        }

        
    }

    [PunRPC]
    void ShareSatisfied()
    {
        specialController.isSatisfied = true;
    }


    void RandomDamage(bool isStrong)
    {
        int a = UnityEngine.Random.Range(0, 100);

        if (isStrong)
        {
            if (a < 80)
            {
                if (single)
                {
                    ShareRandomSuccess();
                }
                else
                {
                    photonView.RPC(nameof(ShareRandomSuccess), RpcTarget.AllViaServer);
                }
                
            }
        }
        else
        {
            if (a < 50)
            {
                if (single)
                {
                    ShareRandomSuccess();
                }
                else
                {
                    photonView.RPC(nameof(ShareRandomSuccess), RpcTarget.AllViaServer);
                }
            }
        }
    }

    [PunRPC]
    void ShareRandomSuccess()
    {
        specialController.randomSuccess = true;
    }







    /// <summary>
    /// ダメージ計算でよぶ
    /// </summary>
    void ThrowEnemyRare(int cardID, bool isStrong, int defenderIndex)
    {
        //捨てる数
        int num = -1;
        if (isStrong)
        {
            num = 2;
        }
        else
        {
            num = 1;
        }

        //捨てるレア素材の選択
        List<int> cardIndex = new List<int>();

        //レアな食材のindex
        for (int i = 0; i < 4; i++)
        {
            if (handTransform[defenderIndex, i].childCount != 0)
            {
                if (handTransform[defenderIndex, i].GetChild(0).GetComponent<CardController>().model.condition == 2)
                {
                    cardIndex.Add(i);
                }
            }
        }

        //レアなし
        if (cardIndex.Count != 0)
        {
            int[] index = new int[2] { -1, -1 };

            for (int i = 0; i < num; i++)
            {

                index[i] = cardIndex[UnityEngine.Random.Range(0, cardIndex.Count)];

                cardIndex.Remove(index[i]);

                if (cardIndex.Count == 0)
                {
                    break;
                }
            }

            if (single)
            {
                ShareIngredientIndex(cardID, index);
            }
            else
            {
                photonView.RPC(nameof(ShareIngredientIndex), RpcTarget.AllViaServer, cardID, index);
            }
            


        }
    }

    void IncreaseMyRare(int cardID, bool isStrong, int attackerIndex)
    {
        //レアになる数
        int num = -1;
        if (isStrong)
        {
            num = 2;
        }
        else
        {
            num = 1;
        }

        //レア素材にするものを選択
        List<int> cardIndex = new List<int>();

        //レアでない食材のindex
        for (int i = 0; i < 4; i++)
        {
            if (handTransform[attackerIndex, i].childCount != 0)
            {
                if (handTransform[attackerIndex, i].GetChild(0).GetComponent<CardController>().model.condition != 2)
                {
                    Debug.Log(i);
                    cardIndex.Add(i);
                }
            }
        }


        if (cardIndex.Count != 0)
        {
            int[] index = new int[2] { -1, -1 };

            for (int i = 0; i < num; i++)
            {

                index[i] = cardIndex[UnityEngine.Random.Range(0, cardIndex.Count)];

                cardIndex.Remove(index[i]);

                if (cardIndex.Count == 0)
                {
                    break;
                }
            }

            if (single)
            {
                ShareIngredientIndex(cardID, index);
            }
            else
            {
                photonView.RPC(nameof(ShareIngredientIndex), RpcTarget.AllViaServer, cardID, index);
            }
            


        }

    }


    

    void StealEnemyRare(int cardID, bool isStrong, int defenderIndex)
    {
        //奪う数
        int num = -1;
        if (isStrong)
        {
            num = 2;
        }
        else
        {
            num = 1;
        }

        //奪うレア素材の選択
        List<int> cardIndex = new List<int>();

        //レアな食材のindex
        for (int i = 0; i < 4; i++)
        {
            if (handTransform[defenderIndex, i].childCount != 0)
            {
                if (handTransform[defenderIndex, i].GetChild(0).GetComponent<CardController>().model.condition == 2)
                {
                    Debug.Log(i);
                    cardIndex.Add(i);
                }
            }
        }

        if (cardIndex.Count != 0)
        {
            int[] index = new int[2] { -1, -1 };

            for (int i = 0; i < num; i++)
            {

                index[i] = cardIndex[UnityEngine.Random.Range(0, cardIndex.Count)];

                cardIndex.Remove(index[i]);

                if (cardIndex.Count == 0)
                {
                    break;
                }
            }

            if (single)
            {
                ShareIngredientIndex(cardID, index);
            }
            else
            {
                photonView.RPC(nameof(ShareIngredientIndex), RpcTarget.AllViaServer, cardID, index);
            }
            


        }
    }


    int DishAttack(bool isStrong, int damageCal)
    {
        //お互いが料理した時
        if (eatCardController[1].model.kind == KIND.DISH)
        {
            if (single)
            {
                ShareSatisfied();
            }
            else
            {
                photonView.RPC(nameof(ShareSatisfied), RpcTarget.AllViaServer);
            }
            

            if (isStrong)
            {
                damageCal *= 2;
            }
            else
            {
                damageCal /= 2;
                damageCal *= 3;
            }
        }

        return damageCal;
    }

    int ConditionAttack(bool isStrong, int damageCal)
    {
        //相手が状態異常の時
        if (player[1].poisonCount > 0 || player[1].darkCount > 0 || player[1].clumsyCount > 0)
        {
            if (single)
            {
                ShareSatisfied();
            }
            else
            {
                photonView.RPC(nameof(ShareSatisfied), RpcTarget.AllViaServer);
            }

            if (isStrong)
            {
                damageCal *= 2;
            }
            else
            {
                damageCal /= 2;
                damageCal *= 3;
            }
        }

        return damageCal;
    }

    int BadIngredientAttack(bool isStrong, int damageCal, int defenderIndex)
    {
        //相手が悪い食材を持っている時
        for (int i = 0; i < 4; i++)
        {
            if (handTransform[defenderIndex, i].childCount != 0)
            {
                if (handTransform[defenderIndex, i].GetChild(0).GetComponent<CardController>().model.condition == 0)
                {
                    if (single)
                    {
                        ShareSatisfied();
                    }
                    else
                    {
                        photonView.RPC(nameof(ShareSatisfied), RpcTarget.AllViaServer);
                    }

                    if (isStrong)
                    {
                        damageCal *= 2;
                    }
                    else
                    {
                        damageCal /= 2;
                        damageCal *= 3;
                    }

                    break;
                }
            }
        }

        return damageCal;
    }



    void WeakenIngredient(int cardID, bool isStrong, int defenderIndex)
    {
        //弱体化する数
        int num = -1;
        if (isStrong)
        {
            num = 4;
        }
        else
        {
            num = 1;
        }

        KIND kind;
        if (cardID == 24)
        {
            kind = KIND.RED;
        }
        else if (cardID == 25)
        {
            kind = KIND.YELLOW;
        }
        else
        {
            kind = KIND.GREEN;
        }

        List<int> cardIndex = new List<int>();

        //色食材のIndex
        for (int i = 0; i < 4; i++)
        {
            if (handTransform[defenderIndex, i].childCount != 0)
            {
                if (handTransform[defenderIndex, i].GetChild(0).GetComponent<CardController>().model.kind == kind)
                {
                    cardIndex.Add(i);
                }
            }
        }

        if (cardIndex.Count != 0)
        {
            int[] index = new int[4] { -1, -1, -1, -1 };

            for (int i = 0; i < num; i++)
            {

                index[i] = cardIndex[UnityEngine.Random.Range(0, cardIndex.Count)];

                cardIndex.Remove(index[i]);

                if (cardIndex.Count == 0)
                {
                    break;
                }
            }


            if (single)
            {
                ShareIngredientIndex(cardID, index);
            }
            else
            {
                photonView.RPC(nameof(ShareIngredientIndex), RpcTarget.AllViaServer, cardID, index);
            }
            


        }

    }
}
