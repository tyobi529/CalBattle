using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BGController : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    // スクロール速度
    [SerializeField] float scrollSpeed = -0.03f;
    // 背景終了位置
    private float deadLine;
    // 背景開始位置
    private float startLine;

    [SerializeField] GameObject[] bg = new GameObject[2] { null, null };

    // Use this for initialization
    void Start()
    {
        deadLine = -bg[0].GetComponent<RectTransform>().sizeDelta.x;
       
        startLine = bg[0].GetComponent<RectTransform>().sizeDelta.x;

        //deadLine = -bg[0].transform.local


    }

    // Update is called once per frame
    void Update()
    {
        // 背景を移動する
        for (int i = 0; i < 2; i++)
        {
            bg[i].transform.Translate(this.scrollSpeed, 0, 0);

            // 画面外に出たら、画面右端に移動する
            //if (transform.position.x < this.deadLine)
            if (bg[i].GetComponent<RectTransform>().anchoredPosition.x < this.deadLine)
            {
                bg[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(this.startLine, 0);
            }
        }

    }

}
