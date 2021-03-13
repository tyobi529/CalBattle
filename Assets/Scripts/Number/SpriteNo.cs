#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Spriteで数字を表現するクラス
/// </summary>
public abstract class SpriteNo<T> : MonoBehaviour where T : Component
{

    //文字(02みたい設定もあるのでstringで設定)
    [SerializeField]
    private string _text = "";

    //生成済みのコンポーネント
    [SerializeField, HideInInspector]
    protected List<T> _componentList = new List<T>();
    public int Length => _componentList.Count;

    //色
    [SerializeField]
    private Color _color = Color.white;

    //寄せの設定
    public enum LayoutType
    {
        Center, Left, Right
    }
    [SerializeField]
    private LayoutType _layoutType = LayoutType.Center;

    //文字の間隔
    [SerializeField]
    private float _textSpan = 1f;

    //各数字のスプライト
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();

    //=================================================================================
    //初期化
    //=================================================================================
#if UNITY_EDITOR

    //OnValidateが実行されたか
    private bool _isExecutedOnValidate = false;

    //スクリプトがロードされた時やインスペクターの値が変更された時（エディター上のみ）
    private void OnValidate()
    {
        //Hierarchy上でアクティブでなければスルー(Prefabの内容を更新した時に実行されないように)
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        //起動時に実行されるやつはスルー(transform.SetParentでエラーが出るので)
        if (EditorApplication.isPlayingOrWillChangePlaymode && !_isExecutedOnValidate)
        {
            _isExecutedOnValidate = true;
            return;
        }

        //数字のスプライトが10個(0 ~ 9)になるように調整
        while (_spriteList.Count != 10)
        {
            if (_spriteList.Count > 10)
            {
                _spriteList.RemoveAt(_spriteList.Count - 1);
            }
            else
            {
                _spriteList.Add(null);
            }
        }

        //生成済みのコンポーネントを全て削除し、新たに作り直す(OnValidateではDestroy出来ないので1フレーム後)
        EditorApplication.delayCall += () =>
        {
            //シーン再生から戻ってきた時にnullになる事があるのでチェック
            if (this == null)
            {
                return;
            }

            _componentList.Where(component => component != null).ToList().ForEach(component => DestroyImmediate(component.gameObject));
            _componentList.Clear();
            SetText(_text, true);
        };

    }

#endif

    //全Componentの初期化
    protected void InitComponents()
    {
        _componentList.ForEach(component => InitComponent(component));
    }

    //Componentの初期化
    protected abstract void InitComponent(T component);

    //=================================================================================
    //更新
    //=================================================================================

    //全Componentを更新
    private void UpdateComponents()
    {
        UpdateSprites();
        UpdatePositions();
    }

    //スプライト更新
    private void UpdateSprites()
    {
        for (int i = 0; i < _componentList.Count; i++)
        {
            int no = int.Parse(_text[i].ToString());
            UpdateComponent(_componentList[i], _spriteList[no], _color);
        }
    }

    //各Componentを更新
    protected abstract void UpdateComponent(T component, Sprite sprite, Color color);

    //位置更新
    private void UpdatePositions()
    {
        int textNum = _text.Length;

        for (int i = 0; i < _componentList.Count; i++)
        {
            Vector3 position = Vector3.zero;

            if (_layoutType == LayoutType.Center)
            {
                position.x = ((float)i - (textNum - 1) / 2f) * _textSpan;
            }
            else if (_layoutType == LayoutType.Left)
            {
                position.x = i * _textSpan;
            }
            else if (_layoutType == LayoutType.Right)
            {
                position.x = -(textNum - 1 - i) * _textSpan;
            }

            _componentList[i].transform.localPosition = position;
        }
    }

    //=================================================================================
    //設定変更
    //=================================================================================

    /// <summary>
    /// 寄せの設定を変更する
    /// </summary>
    public void ChangeColor(Color color)
    {
        _color = color;
        UpdateSprites();
    }

    /// <summary>
    /// 寄せの設定を変更する
    /// </summary>
    public void ChangeLayout(LayoutType layoutType)
    {
        _layoutType = layoutType;
        UpdatePositions();
    }

    /// <summary>
    /// 文字の間隔を変更する
    /// </summary>
    public void ChangeSpan(float textSpan)
    {
        _textSpan = textSpan;
        UpdatePositions();
    }

    //=================================================================================
    //テキスト設定
    //=================================================================================

    /// <summary>
    /// テキストを数字で設定する
    /// </summary>
    public void Setno(int no)
    {
        if (no < 0)
        {
            Debug.LogError($"{no}は0以上にする必要があります");
            return;
        }
        SetText(no.ToString());
    }

    /// <summary>
    /// テキストを数字で設定する(textFormatが空かどうかの判定をしないよう(負荷的に)に別メソッドに)
    /// </summary>
    public void Setno(int no, string textFormat)
    {
        SetText(no.ToString(textFormat));
    }

    //テキストで数字を設定する
    private void SetText(string text, bool isForcibly = false)
    {
        //テキストが変わってない場合はスルー(isForciblyが有効な時は常に設定)
        if (_text == text && !isForcibly)
        {
            return;
        }
        _text = text;

        //文字数にコンポーネントが足りなければ作成、多ければ削除
        while (_componentList.Count != text.Length)
        {
            if (_componentList.Count > text.Length)
            {
                var component = _componentList[0];
                _componentList.Remove(component);

                if (Application.isPlaying)
                {
                    Destroy(component.gameObject);
                }
                else
                {
                    DestroyImmediate(component.gameObject);
                }
            }
            else
            {
                GameObject child = new GameObject(_componentList.Count.ToString());
                child.transform.SetParent(transform, false);

                var newRenderer = child.AddComponent<T>();
                _componentList.Add(newRenderer);

                InitComponent(newRenderer);
            }

        }

        //全Componentを更新
        UpdateComponents();
    }




}