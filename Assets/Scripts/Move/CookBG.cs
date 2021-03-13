using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBG : MonoBehaviour
{
    //float alpha_Sin;
    [SerializeField] float speed;
    float angle;

    // Update is called once per frame
    void Update()
    {
        //alpha_Sin = Mathf.Sin(Time.time * 5f) / 2f + 0.5f;

        angle = Time.deltaTime * speed;

        transform.Rotate(0f, 0f, angle);
    }
}
