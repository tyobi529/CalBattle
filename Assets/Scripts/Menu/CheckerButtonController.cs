using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CheckerButtonController : MonoBehaviour
{
    [SerializeField] Image buttonImage = null;
    public GameObject selecter = null;

    [SerializeField] ParticleSystem starEffect = null;

    int checkerIndex = -1;
    int clearNum = -1;

    private Page_2Controller page_2Controller = null;

    [SerializeField] Sprite[] buttonSprite = new Sprite[2];

    public void SetCheckerButton(int index, int clearNum)
    {
        checkerIndex = index;
        this.clearNum = clearNum;
        
        buttonImage.sprite = buttonSprite[clearNum];

        //checkerController = transform.parent.parent.GetComponent<CheckerController>();
        page_2Controller = GameObject.Find("Page2").GetComponent<Page_2Controller>();

        selecter.SetActive(false);

        transform.GetComponent<Button>().onClick.AddListener(OnCheckerButton);
    }

    

    public void OnCheckerButton()
    {

        page_2Controller.OnCheckerButton(checkerIndex, clearNum);
        selecter.SetActive(true);
    }

    public void ChangeClearView()
    {
        starEffect.gameObject.SetActive(true);
        starEffect.Play();

        DOVirtual.DelayedCall(1f, () =>
        {
            buttonImage.sprite = buttonSprite[1];
            clearNum = 1;
            OnCheckerButton();
        });

    }
}
