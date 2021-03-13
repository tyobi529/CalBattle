using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField] public GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] float messageSpeed;

    private string[] playerName = new string[2];


    [SerializeField] Color[] textColor = new Color[2];    


    public void ChangeMessageColor(bool isAttacker)
    {
        StopAllCoroutines();

        if (isAttacker)
        {
            messageText.color = textColor[0];
        }
        else
        {
            messageText.color = textColor[1];
        }
        
    }

    public void WaitingMessage()
    {
        string message = "相手の選択待ちです";

        //StopAllCoroutines();
        StartCoroutine(ChangeMessage(message));

    }

    //食べたテキスト
    public void EatMessage(CardModel cardModel, bool isMyTurn)
    {

        string message = "";
        string name = "";

        if (isMyTurn)
        {
            name = playerName[0];            
        }
        else
        {
            name = playerName[1];
        }

        //message = name + " は" + "\n「 " + cardModel.name + " 」をたべた！";
        message = name + " は" + $"\n\"{cardModel.name}\"" + " をたべた！";


        //StopAllCoroutines();
        StartCoroutine(ChangeMessage(message));
    }

    //レア素材を含む時
    public void RareDishMessage()
    {
        string message = "料理の効果がアップした！";

        StartCoroutine(ChangeMessage(message));
    }

    //攻撃のテキスト
    public void AttackText(string attackerName)
    {
        string message = "";


        message = attackerName + " のこうげき！";


        StartCoroutine(ChangeMessage(message));
    }

    public void ParalysisText()
    {
        string message = "麻痺で動けなかった";

        //StopAllCoroutines();
        StartCoroutine(ChangeMessage(message));
    }

    //効果のテキスト
    public void EffectMessage(string message)
    {
        //StopAllCoroutines();
        StartCoroutine(ChangeMessage(message));

    }


    //文字列を受けて１文字づつ表示する。
    private IEnumerator ChangeMessage(string message)
    {
        messageText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            messageText.text += message[i];
            yield return new WaitForSeconds(messageSpeed);//任意の時間待つ

        }

    }

    public void SetPlayerName(string[] playerName)
    {
        this.playerName = playerName;
    }
}
