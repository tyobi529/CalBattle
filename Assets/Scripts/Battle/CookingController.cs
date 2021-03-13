using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CookingController : MonoBehaviour
{
    //料理が作られる
    [SerializeField] Transform[] cookingFieldTransform = new Transform[2];

    [SerializeField] GameObject blackPanel;

    [SerializeField] GameObject cookingObj;

    [SerializeField] GameObject[] Flypan = new GameObject[2];

    //手札の位置から食材を動かす
    [SerializeField] Transform[] startTransform = new Transform[4];
    [SerializeField] Transform[] viaTransform = new Transform[4];


    [SerializeField] GameObject[] mokoObj = new GameObject[2];

    bool[] cooked = new bool[2] { false, false };




    private void Start()
    {
        cookingObj.SetActive(false);
    }


    public IEnumerator Cooking(int playerIndex, CardController[,] mixCardController, CardController[] eatCardController)
    {


        if (eatCardController[playerIndex].model.kind != KIND.DISH)
        {
            Flypan[playerIndex].SetActive(false);
            EndCooking(playerIndex);
            yield break;
        }


        bool isSound = false;

        if (playerIndex == 0)
        {
            isSound = true;
        }
        else
        {
            if (eatCardController[0].model.kind != KIND.DISH)
            {
                isSound = true;
            }
        }



        Flypan[playerIndex].SetActive(true);

        cookingObj.SetActive(true);

 

        StartCoroutine(MoveMoko(playerIndex));

        for (int i = 0; i < 2; i++)
        {


            mixCardController[playerIndex, i].transform.SetParent(cookingFieldTransform[playerIndex]);
            eatCardController[playerIndex].transform.SetParent(cookingFieldTransform[playerIndex]);

            Vector3[] path = new Vector3[3];

            path[0] = startTransform[playerIndex * 2 + i].GetComponent<RectTransform>().localPosition;

            path[1] = viaTransform[playerIndex * 2 + i].GetComponent<RectTransform>().localPosition;

            //path[2] = new Vector2(0f, 0f);
            path[2] = Flypan[i].transform.localPosition;

            eatCardController[playerIndex].transform.localPosition = path[2];
            mixCardController[playerIndex, i].transform.localPosition = path[0];


            mixCardController[playerIndex, i].gameObject.SetActive(true);
            mixCardController[playerIndex, i].transform.localScale = new Vector3(1f, 1f, 1f);

            if (isSound)
            {
                BattleSoundManager.instance.CookSE();
            }


            mixCardController[playerIndex, i].transform.DOLocalPath(path, 0.8f, PathType.CatmullRom)
                            .SetEase(Ease.Linear);



            yield return new WaitForSeconds(0.5f);



            mixCardController[playerIndex, i].transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).OnComplete(() => {
                Destroy(mixCardController[playerIndex, i].gameObject);
            });

            yield return new WaitForSeconds(0.3f);



  

        }


        if (isSound)
        {
            BattleSoundManager.instance.CookEndSE();
        }

        eatCardController[playerIndex].gameObject.SetActive(true);

        eatCardController[playerIndex].transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.4f);

        yield return new WaitForSeconds(0.4f);


        eatCardController[playerIndex].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);

        yield return new WaitForSeconds(1.5f);


        eatCardController[playerIndex].gameObject.SetActive(false);
        

        EndCooking(playerIndex);




    }

    void EndCooking(int playerIndex)
    {
        cooked[playerIndex] = true;



        if (cooked[0] && cooked[1])
        {
            //for (int i = 0; i < 2; i++)
            //{
            //    cookingFieldTransform[i].gameObject.SetActive(false);
            //}

            cookingObj.gameObject.SetActive(false);

            //blackPanel.SetActive(false);

            cooked[0] = false;
            cooked[1] = false;
            GameManager.instance.EndCooking();
        }

        StopAllCoroutines();
    }

    IEnumerator MoveMoko(int playerIndex)
    {
        //int count = timeLimit;
        float xScale = 0f;

        if (playerIndex == 0)
        {
            xScale = 1f;
        }
        else
        {
            xScale = -1f;
        }
        

        while (true)
        {
            xScale = -xScale;
            mokoObj[playerIndex].transform.localScale = new Vector3(xScale, 1f, 1f);
            yield return new WaitForSeconds(0.3f);            
            //count--;
            //uiManager.UpdateTime(count);
        }

        //yield return new WaitForSeconds(1);



    }


    public void CompleteTween()
    {
        if (DOTween.instance != null)
        {
            DOTween.CompleteAll();
        }
    }
}