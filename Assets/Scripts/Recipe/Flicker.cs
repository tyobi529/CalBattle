using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Flicker : MonoBehaviour
{
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    [SerializeField] BookUI bookUI;

    private void Update()
    {
        Flick();
    }

    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                                        Input.mousePosition.y,
                                        Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);
            GetDirection();
        }
    }

    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;
        //string Direction = null;

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                //右向きにフリック
                //Direction = "right";
                bookUI.OnPreviousButton();
            }
            else if (-30 > directionX)
            {
                //左向きにフリック
                //Direction = "left";                
                bookUI.OnNextButton();
            }

            
        }
        //else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        //{
        //    if (30 < directionY)
        //    {
        //        //上向きにフリック
        //        Direction = "up";
        //    }
        //    else if (-30 > directionY)
        //    {
        //        //下向きのフリック
        //        Direction = "down";
        //    }
        //}
        //else
        //{
        //    //タッチを検出
        //    Direction = "touch";
        //}

        //Debug.Log(Direction);

        //switch (Direction)
        //{
        //    case "left":
        //        bookUI.OnPreviousButton();
        //        break;
        //    case "right":
        //        bookUI.OnNextButton();
        //        break;
        //}
    }
}

