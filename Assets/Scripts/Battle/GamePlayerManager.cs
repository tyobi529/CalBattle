using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    [SerializeField] Setting setting;

    public int playerIndex { get; set; }

    public int maxHp { get; set; }
    public int hp { get; set; }
    public int cost { get; set; }


    public int poisonCount { get; set; } = 0;
    public int darkCount { get; set; } = 0;
    //public int paralysisCount { get; set; } = 0;
    public int clumsyCount { get; set; } = 0;


    public int nextAttack { get; set; } = 0;


    public int reduceDamage { get; set; } = 0;


    //使う度に強くなる料理
    //public bool usedDish { get; set; } = false;

    //ダメージを軽減する料理
    //public int defenceDish { get; set; } = 0;

    private void Awake()
    {
        maxHp = setting.maxHp;
        hp = setting.maxHp;
        cost = setting.defaultCost;


        poisonCount = setting.poisonCount;
        darkCount = setting.darkCount;
        clumsyCount = setting.clumsyCount;
    }

}
