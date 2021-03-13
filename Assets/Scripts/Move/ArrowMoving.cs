using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMoving : MonoBehaviour
{
    [SerializeField] bool isRight;
    [SerializeField] float moveDirection;
    [SerializeField] float speed;

    private Vector2 firstPos;

    private void Start()
    {
        firstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRight)
        {
            transform.position = new Vector2(firstPos.x + moveDirection * Mathf.Sin(Time.time * speed), firstPos.y);
        }
        else
        {
            transform.position = new Vector2(firstPos.x - moveDirection * Mathf.Sin(Time.time * speed), firstPos.y);
        }
    }
}
