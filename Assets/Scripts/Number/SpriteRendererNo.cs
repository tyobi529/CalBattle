using UnityEngine;

/// <summary>
/// SpriteRendererで数字を表現するクラス
/// </summary>
public class SpriteRendererNo : SpriteNo<SpriteRenderer>
{

    [SerializeField]
    private string _sortingLayerName = "Default";

    [SerializeField]
    private int _sortingOrder = 0;

    //=================================================================================
    //初期化
    //=================================================================================

    //新しく作ったSpriteRendererの初期化
    protected override void InitComponent(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sortingLayerName = _sortingLayerName;
        spriteRenderer.sortingOrder = _sortingOrder;
    }

    //=================================================================================
    //更新
    //=================================================================================

    //Spriteを更新
    protected override void UpdateComponent(SpriteRenderer spriteRenderer, Sprite sprite, Color color)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    //=================================================================================
    //設定変更
    //=================================================================================

    /// <summary>
    /// sortingLayerNameの設定を変更する
    /// </summary>
    public void ChangeSortingLayerName(string sortingLayerName)
    {
        _sortingLayerName = sortingLayerName;
        InitComponents();
    }

    /// <summary>
    /// sortingOrderの設定を変更する
    /// </summary>
    public void ChangeSortingOrder(int sortingOrder)
    {
        _sortingOrder = sortingOrder;
        InitComponents();
    }

}