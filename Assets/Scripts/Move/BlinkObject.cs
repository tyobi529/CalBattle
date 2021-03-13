using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkObject : MonoBehaviour
{
    Color _color;
    float alpha_Sin;
    [SerializeField] float speed;


    private void OnEnable()
    {
        _color = transform.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
            alpha_Sin = Mathf.Sin(Time.time * speed) / 2 + 0.5f;

            _color.a = alpha_Sin;

            transform.GetComponent<Image>().color = _color;
        
    }
}
