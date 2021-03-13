using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RareIcon : MonoBehaviour
{
    Image rareImage;
    Color _color;
    float alpha_Sin;


    private void OnEnable()
    {
        rareImage = GetComponent<Image>();
        _color = rareImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        alpha_Sin = Mathf.Sin(Time.time * 5f) / 2f + 0.5f;

        _color.a = alpha_Sin;

        rareImage.color = _color;
    }
}
