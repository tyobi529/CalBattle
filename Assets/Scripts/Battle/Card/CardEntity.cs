using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardEntity", menuName ="Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public new string name;
    public int cal;

    public Sprite icon;
    public Sprite glowIcon;


    public KIND kind;


    public int[] partnerID;
    public int[] specialMixID;


    public int[] ingredientCardID;
    
}


public enum KIND
{
    RED,
    YELLOW,
    GREEN,
    DISH,
}





