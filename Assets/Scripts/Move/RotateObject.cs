using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [SerializeField] float interval;

    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > interval)
        {
            time = 0f;
            transform.Rotate(new Vector3(0f, 0f, -30f), Space.World);
        }
        
    }
}
