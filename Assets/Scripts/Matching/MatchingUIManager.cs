using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchingUIManager : MonoBehaviour
{
    [SerializeField] MatchingController matchingController = null;


    [SerializeField] GameObject searchingPanel = null;
    [SerializeField] Image searchingTextImage = null;
    [SerializeField] Image successTextImage = null;


    [SerializeField] GameObject matchingObject = null;

    //ランダムマッチ
    [SerializeField] GameObject randomTextObject = null;

    //ルームマッチ
    [SerializeField] GameObject roomTextObject = null;
    [SerializeField] TMP_InputField inputField = null;
    [SerializeField] TextMeshProUGUI roomNametext = null;


    //ボタン
    [SerializeField] GameObject ButtonObject = null;


    string roomName = null;

    public IEnumerator BlinkingMatchingText()
    {

        float alpha_Sin;
        Color _color = searchingTextImage.color;

        while (true)
        {
            yield return new WaitForEndOfFrame();

            alpha_Sin = Mathf.Sin(Time.time * 3f) / 2 + 0.5f;

            _color.a = alpha_Sin;

            searchingTextImage.color = _color;
        }

    }

    //ランダムマッチングボタン
    public void OnRandomMatchingButton()
    {
        //searchingPanelObject.SetActive(true);
        matchingObject.SetActive(true);
        randomTextObject.SetActive(true);

        
        //StartCoroutine(BlinkingMatchingText());

       
    }

    //ルームマッチングボタン
    public void OnRoomMatchingButton()
    {
        matchingObject.SetActive(true);
        roomTextObject.SetActive(true);
        inputField.gameObject.SetActive(true);

        //対戦相手探すテキスト点滅
        //searchingPanelObject.SetActive(true);
        //StartCoroutine(BlinkingMatchingText());

       
    }

    public void OnOKButton()
    {
        randomTextObject.SetActive(false);        
        roomTextObject.SetActive(false);
        inputField.gameObject.SetActive(false);

        ButtonObject.SetActive(false);

        if (inputField.text != null)
        {
            roomNametext.gameObject.SetActive(true);
            roomNametext.text = "ルームID「" + inputField.text + "」";
        }


        searchingTextImage.gameObject.SetActive(true);

        matchingController.StartMatching(roomName);

        StartCoroutine(BlinkingMatchingText());

    }


    public void InputText()
    {
        roomNametext.text = inputField.text;
        roomName = inputField.text;
    }

    public void MatchingSuccess()
    {
        searchingTextImage.gameObject.SetActive(false);
        successTextImage.gameObject.SetActive(true);
    }
}
