using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetting : MonoBehaviour
{

    //シングルトン化（どこからでもアクセスできるようにする）
    public static MenuSetting instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public bool cutMissionClear = false;

}
