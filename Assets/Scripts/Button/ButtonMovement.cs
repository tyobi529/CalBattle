using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonMovement : MonoBehaviour
{
    [SerializeField] bool isMove = false;
    //動かす幅
    [SerializeField] float moveDirection = 10f;
    //周期
    [SerializeField] float speed = 5f;

    [SerializeField ]bool reverse = false;
    Vector3 firstPos;

    //float time = 0f;
    float sin = 0f;

    private void Start()
    {
        firstPos = transform.localPosition;
    }

    private void Update()
    {
        if (isMove)
        {
            sin = Mathf.Sin(Time.time * speed);

            if (reverse)
            {
                sin *= -1f;
            }

            transform.localPosition = new Vector3(firstPos.x + moveDirection * sin, firstPos.y, firstPos.z);
        }

    }

    public void OnButtonAnimation()
    {
        Debug.Log("押した");

        transform.DOPunchScale(
                    punch: Vector3.one * 0.1f,
                    duration: 0.2f,
                    vibrato: 1
                ).SetEase(Ease.OutExpo).OnComplete(() => {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                });


    }
}
