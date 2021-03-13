using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    //デバック用    
    public bool isCountDown;
    //public bool isSolo;
    //public bool onlyGame;

    public float timeLimit;

    public bool showHpValue;
    public int maxHp;
    public int defaultHp;
    public int needCost;
    public int maxCost;
    public int defaultCost;

    public int poisonCount;
    public int darkCount;
    public int clumsyCount;

    public bool onlineDebug;

    public bool calDebug;
    public int[] cal = new int[2];

    public bool specialDebug;
    public int[] specialID = new int[2];
    public bool isStrong;

    public bool debugBattle = false;
    public bool cutStartAnimation = false;

}
