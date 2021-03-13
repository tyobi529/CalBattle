using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameGenerator : MonoBehaviour
{
    [SerializeField] GameObject framePrefab;

    [SerializeField] float generateTime;

    float time = 0f;

    private void OnEnable()
    {
        time = generateTime;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > generateTime)
        {
            time = 0f;
            GameObject frame = Instantiate(framePrefab, transform, false);
            //GameObject frame = Instantiate(framePrefab, fieldTransform, false);
            //frame.transform.position = mixField_2Transform.position;
            //frame.transform.SetSiblingIndex(0);
        }
    }
}
