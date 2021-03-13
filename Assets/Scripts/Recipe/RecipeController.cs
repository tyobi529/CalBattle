using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using DG.Tweening;


public class RecipeController : MonoBehaviour
{
    [SerializeField] BookUI bookUI = null;

    [SerializeField] GameObject book = null;
    
    [SerializeField] Transform bookLayout = null;

    [SerializeField] GameObject contentPrefab = null;
    [SerializeField] GameObject recipePrefab = null;
    [SerializeField] GameObject backPrefab = null;

    [SerializeField] GameObject ingredientContentPrefab = null;

    [SerializeField] Transform ingredientPageTransfrom = null;
    [SerializeField] Transform[] contentPageTransform = new Transform[3] { null, null, null };

    [SerializeField] Text cookedNumText = null;
    private int cookedNum = 0;

    float naviTime = 0f;
    bool showNavi = false;

    [SerializeField] GameObject hand = null;
    [SerializeField] GameObject yajirushi = null;



    void Start()
    {
        //FadeManager.FadeIn();

        //Screen.orientation = ScreenOrientation.AutoRotation;
        //Screen.orientation = ScreenOrientation.AutoRotation;

        //本の表紙
        for (int i = 0; i < 27; i++)
        {
            int cooked = PlayerPrefs.GetInt("COOKED_" + i, 0);
            if (cooked == 1)
            {
                cookedNum++;
            }            
        }

        bool isComplete = false;
        if (cookedNum == 27)
        {
            cookedNumText.text = $"<size=65><color=#FBF241>{cookedNum}</color></size>" + " / 27";
            isComplete = true;
        }
        else
        {
            cookedNumText.text = $"<size=65>{cookedNum}</size>" + " / 27";
        }
        


        Screen.orientation = ScreenOrientation.LandscapeLeft;

        for (int i = 0; i < 9; i++)
        {
            GameObject ingredientContent = Instantiate(ingredientContentPrefab, ingredientPageTransfrom, false);
            ingredientContent.GetComponent<ContentView>().SetIngredientContent(i);
        }

        //一覧ページ生成
        for (int i = 0; i < 27; i++)
        {
            //GameObject content = Instantiate(contentPrefab, bookLayout, false);
            GameObject content;

            if (i < 9)
            {
                content = Instantiate(contentPrefab, contentPageTransform[0], false);
            }
            else if (i < 18)
            {
                content = Instantiate(contentPrefab, contentPageTransform[1], false);
            }
            else
            {
                content = Instantiate(contentPrefab, contentPageTransform[2], false);
            }

            content.GetComponent<ContentView>().SetDishContent(i, isComplete);
        }


        //詳細ページ生成
        for (int i = 0; i < 27; i++)
        {
            GameObject recipe = Instantiate(recipePrefab, bookLayout, false);

            recipe.GetComponent<RecipeView>().SetRecipe(i);
        }

        Instantiate(backPrefab, bookLayout, false);

        //book.SetActive(true);

        bookUI.CountPageNum();

        

    }

    private void Update()
    {
        if (bookUI.CurrentPage == 0)
        {
            naviTime += Time.deltaTime;

            if (naviTime > 5f && !showNavi)
            {
                showNavi = true;
                naviTime = 0f;
                StartCoroutine(StartNavi());
                hand.SetActive(true);
                yajirushi.SetActive(true);
                StartCoroutine(StartNavi());
            }
        }
        else
        {
            naviTime = 0f;
            showNavi = false;
            StopAllCoroutines();
            hand.SetActive(false);
            yajirushi.SetActive(false);
        }
    }


    IEnumerator StartNavi()
    {

        while (true)
        {
            hand.SetActive(true);

            hand.transform.DOLocalMove(new Vector3(-200f, -130f), 1f).OnComplete(() => {
                hand.SetActive(false);
                hand.transform.localPosition = new Vector3(200f, -130f, 0f);

            });

            DOTween.ToAlpha(
            () => yajirushi.GetComponent<Image>().color,
            color => yajirushi.GetComponent<Image>().color = color,
            0f, // 目標値
            1f// 所要時間
            ).OnComplete(() => {
                DOTween.ToAlpha(
           () => yajirushi.GetComponent<Image>().color,
           color => yajirushi.GetComponent<Image>().color = color,
           1f, // 目標値
           1f// 所要時間
           );
            });

            yield return new WaitForSeconds(2);
   
        }
        

    }





    public void OnToMenuButton()
    {
        FadeManager.FadeOut(1);
    }


}
