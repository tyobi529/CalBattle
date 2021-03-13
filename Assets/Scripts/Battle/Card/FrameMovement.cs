using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FrameMovement : MonoBehaviour
{
    [SerializeField] float erasedTime;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), erasedTime).OnComplete(() => {
        //    Destroy(this.gameObject);
        //});

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        float color_a = 1f * time / erasedTime;
        transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f - color_a);

        float scale = 1f + 0.2f * (time / erasedTime);
        //Vector3 scale = new Vector3(1f + 0.2f * (time / erasedTime), )
        transform.localScale = new Vector3(scale, scale, scale);

        if (time > erasedTime)
        {
            Destroy(this.gameObject);
        }
    }
}
