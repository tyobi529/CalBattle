using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    //ゲーム内で変更するImage
    //public GameObject characterObj;
    [SerializeField] Image[] characterImage = new Image[2];

    //そのゲームで使う自分のSprite
    private Sprite[] playerIdleSprite = new Sprite[2];
    private Sprite[] playerCookSprite = new Sprite[2];
    private Sprite[] playerAttackSprite = new Sprite[2];
    private Sprite[] playerDefenceSprite = new Sprite[2];
    private Sprite playerRareSprite;
    private Sprite[] playerResultSprite = new Sprite[2];

    //そのゲームで使う相手のSprite
    private Sprite[] enemyIdleSprite = new Sprite[2];
    private Sprite[] enemyCookSprite = new Sprite[2];
    private Sprite[] enemyAttackSprite = new Sprite[2];
    private Sprite[] enemyDefenceSprite = new Sprite[2];
    private Sprite enemyRareSprite;
    private Sprite[] enemyResultSprite = new Sprite[2];

    //青
    [SerializeField] Sprite[] blueIdleSprite = new Sprite[2];
    [SerializeField] Sprite[] blueCookSprite = new Sprite[2];
    [SerializeField] Sprite[] blueAttackSprite = new Sprite[2];
    [SerializeField] Sprite[] blueDefenceSprite = new Sprite[2];
    [SerializeField] Sprite blueRareSprite;
    [SerializeField] Sprite[] blueResultSprite = new Sprite[2];

    //ピンク
    [SerializeField] Sprite[] pinkIdleSprite = new Sprite[2];
    [SerializeField] Sprite[] pinkCookSprite = new Sprite[2];
    [SerializeField] Sprite[] pinkAttackSprite = new Sprite[2];
    [SerializeField] Sprite[] pinkDefenceSprite = new Sprite[2];
    [SerializeField] Sprite pinkRareSprite;
    [SerializeField] Sprite[] pinkResultSprite = new Sprite[2];


    //[SerializeField] Sprite[] characterSprite = new Sprite[5];
    //[SerializeField] Image characterImage;


    [SerializeField] Color[] characterColor = new Color[3];


    IEnumerator cookSpriteCoroutine;
    IEnumerator idleSpriteCoroutine;


    private void Start()
    {


        cookSpriteCoroutine = ChangeCookSpriteCoroutine();
        idleSpriteCoroutine = ChangeIdleSpriteCoroutine();

        //ChangeIdleSprite();

        //characterObj.SetActive(false);

        ShowCharacter(false);
    }

    public void ShowCharacter(bool isShow)
    {
        for (int i = 0; i < 2; i++)
        {
            characterImage[i].enabled = isShow;
        }
    }



    public void SetCharacterSkin(int[] playerSkin)
    {
        //自分のスキン
        switch (playerSkin[0])
        {
            case 0:
                playerIdleSprite = blueIdleSprite;
                playerCookSprite = blueCookSprite;
                playerAttackSprite = blueAttackSprite;
                playerDefenceSprite = blueDefenceSprite;
                playerRareSprite = blueRareSprite;
                playerResultSprite = blueResultSprite;
                break;
            case 1:
                playerIdleSprite = pinkIdleSprite;
                playerCookSprite = pinkCookSprite;
                playerAttackSprite = pinkAttackSprite;
                playerDefenceSprite = pinkDefenceSprite;
                playerRareSprite = pinkRareSprite;
                playerResultSprite = pinkResultSprite;
                break;
        }


        //相手のスキン
        switch (playerSkin[1])
        {
            case 0:
                enemyIdleSprite = blueIdleSprite;
                enemyCookSprite = blueCookSprite;
                enemyAttackSprite = blueAttackSprite;
                enemyDefenceSprite = blueDefenceSprite;
                enemyRareSprite = blueRareSprite;
                enemyResultSprite = blueResultSprite;
                break;
            case 1:
                enemyIdleSprite = pinkIdleSprite;
                enemyCookSprite = pinkCookSprite;
                enemyAttackSprite = pinkAttackSprite;
                enemyDefenceSprite = pinkDefenceSprite;
                enemyRareSprite = pinkRareSprite;
                enemyResultSprite = pinkResultSprite;
                break;
        }

    }


    IEnumerator ChangeCookSpriteCoroutine()
    {
        int num = 0;


        while (true)
        {
            characterImage[0].sprite = playerCookSprite[num];

            if (num == 0)
            {
                num = 1;
            }
            else
            {
                num = 0;
            }

            characterImage[1].sprite = enemyCookSprite[num];
            yield return new WaitForSeconds(0.5f);

        }

    }

    public void ChangeCookSprite()
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        StartCoroutine(cookSpriteCoroutine);
    }

    //void StopCookSprite()
    //{
    //    StopCoroutine(cookSpriteCoroutine);
    //}


    IEnumerator ChangeIdleSpriteCoroutine()
    {

        int num = 0;

        while (true)
        {
            characterImage[0].sprite = playerIdleSprite[num];

            if (num == 0)
            {
                num = 1;
            }
            else
            {
                num = 0;
            }

            characterImage[1].sprite = enemyIdleSprite[num];
            yield return new WaitForSeconds(0.8f);

        }

    }


    public void ChangeIdleSprite()
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        StartCoroutine(idleSpriteCoroutine);
    }

    //public void StopIdleSprite()
    //{
    //    StopCoroutine(idleSpriteCoroutine);
    //}


    public void ChangeBattleSprite(bool isAttacker, int num)
    {


        if (isAttacker)
        {
            ChangeAttackSprite(0, num);
            ChangeDefenceSprite(1, num);
        }
        else
        {
            ChangeAttackSprite(1, num);
            ChangeDefenceSprite(0, num);
        }
    }


    void ChangeAttackSprite(int playerIndex, int num)
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        if (playerIndex == 0)
        {
            characterImage[playerIndex].sprite = playerAttackSprite[num];
        }
        else
        {
            characterImage[playerIndex].sprite = enemyAttackSprite[num];
        }
        
    }

    void ChangeDefenceSprite(int playerIndex, int num)
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        if (playerIndex == 0)
        {
            characterImage[playerIndex].sprite = playerDefenceSprite[num];
        }
        else
        {
            characterImage[playerIndex].sprite = enemyDefenceSprite[num];
        }

    }

    //public void ChangeIdleSprite(int playerIndex)
    //{
    //    if (playerIndex == 0)
    //    {
    //        characterImage[playerIndex].sprite = playerIdleSprite;
    //    }
    //    else
    //    {
    //        characterImage[playerIndex].sprite = enemyIdleSprite;
    //    }
    //}

    public void ChangeRareSprite(int playerIndex)
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        if (playerIndex == 0)
        {
            characterImage[playerIndex].sprite = playerRareSprite;
        }
        else
        {
            characterImage[playerIndex].sprite = enemyRareSprite;
        }
    }

    public void ChangeResultSprite(bool isWin)
    {
        StopCoroutine(idleSpriteCoroutine);
        StopCoroutine(cookSpriteCoroutine);

        if (isWin)
        {
            characterImage[0].sprite = playerResultSprite[0];
            characterImage[1].sprite = enemyResultSprite[1];
        }
        else
        {
            characterImage[0].sprite = playerResultSprite[1];
            characterImage[1].sprite = enemyResultSprite[0];
        }

    }

    public void ChangeCharacterColor(int playerIndex, int num)
    {
        if (num == 3)
        {
            characterImage[playerIndex].color = Color.white;
        }
        else
        {
            characterImage[playerIndex].color = characterColor[num];
        }
        
    }


    //public void ChangeSprite(int num, bool isAttacker)
    //{
    //    if (isAttacker)
    //    {
    //        characterImage.transform.localScale = new Vector3(1.0f, 1f, 1f);
    //    }
    //    else
    //    {
    //        characterImage.transform.localScale = new Vector3(-1.0f, 1f, 1f);
    //    }

    //    characterImage.sprite = characterSprite[num];
    //    currentSprite = num;
    //}


}
