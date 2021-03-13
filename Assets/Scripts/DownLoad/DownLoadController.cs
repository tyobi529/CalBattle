using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DownLoadController : MonoBehaviour
{

    string awsUrl = null;



    AssetBundleTable table = null;

    [SerializeField] CardData cardData = null;


    string osName = null;


    [SerializeField] GameObject downLoadObj = null;
    [SerializeField] Image downLoadValue = null;
    [SerializeField] TextMeshProUGUI percentText = null;

    [SerializeField] GameObject loadFailedObj = null;

    [SerializeField] GameObject newDataObj = null;

    [SerializeField] TitleSceneController titleSceneController = null;


    int count = 0;
    int ingCount = 9;
    int dishCount = 27;
    int bgmCount = 8;
    int maxCount = 0;


    float timeOut = 0f;

    Color _color;


    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            osName = "Android";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // iOS
            osName = "iOS";
        }
        else
        {
            osName = "iOS";
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //テスト
        //osName = "Android";

        if (!CardData.instance.isDownLoad)
        {
            _color = percentText.color;

            Debug.Log("ダウンロードまだ");

            SceneAnimation.instance.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;

            maxCount = ingCount + dishCount + bgmCount;

            //osName = "iOS";

            awsUrl = "http://calbattle.s3.amazonaws.com/";

            string assetBundleName = osName + "/" + "assetbundletable_" + osName.ToLower();
            Hash128 hash = Hash128.Parse("0");
            uint crc = 0;


            Uri _uri = new Uri(awsUrl + assetBundleName);

            LoadTable(_uri, assetBundleName, hash, crc);
        }
        else
        {
            Debug.Log("ダウンロード済");
            titleSceneController.ShowTitle();
        }


        


    }


    void LoadEnd(LOAD load)
    {
        switch (load)
        {
            case LOAD.TABLE:
                Debug.Log("table完了");
                GenerateCard(0, false);
                break;
            case LOAD.CARD:
                Debug.Log("食材完了");
                GenerateCard(0, true);
                break;
            case LOAD.DCARD:
                Debug.Log("料理完了");
                GenerateBGM(0);
                break;
            case LOAD.BGM:
                Debug.Log("BGM完了");
                SoundManager.instance.StartBGM("Title");
                break;
        }
    }


    void LoadFailed()
    {
        downLoadObj.SetActive(false);
        loadFailedObj.SetActive(true);
    }

    void LoadSuccess()
    {
        downLoadObj.SetActive(true);
        loadFailedObj.SetActive(false);
    }


    void ShowLoad()
    {
        percentText.color = new Color(0f, 0f, 0f, 1f);
        timeOut = 0f;
        LoadSuccess();
        
        count++;

        float fill = (float)count / (float)maxCount;
        downLoadValue.fillAmount = fill;
        percentText.text = (int)(fill * 100f) + " %";

        if (count >= maxCount)
        {
            titleSceneController.ShowTitle();

            cardData.isDownLoad = true;

            //percentText.color = Color.yellow;
            percentText.color = new Color(248f / 255f, 126f / 255f, 15f / 255f, 1f);

            PlayerPrefs.SetInt("VERSION", table.Version);
            PlayerPrefs.Save();

            //downLoadObj.transform.parent.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() =>
            //{
            //    downLoadObj.transform.parent.gameObject.SetActive(false);
            //});


            downLoadObj.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() =>
            {
                downLoadObj.transform.parent.gameObject.SetActive(false);
            });

            SceneAnimation.instance.transform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(() =>
            {
                //downLoadObj.transform.parent.gameObject.SetActive(false);
                SceneAnimation.instance.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                
            });

            return;
        }

    }


    private void Update()
    {
        timeOut += Time.deltaTime;
        if (timeOut > 5f)
        {
            float alpha_Sin = Mathf.Sin(Time.time * 5f) / 2 + 0.5f;

            _color.a = alpha_Sin;

            percentText.color = _color;

            percentText.text = "通信が不安定です";
            return;
        }
    }


    public enum LOAD
    {
        TABLE,
        CARD,
        DCARD,
        BGM,
    }


    public void OnDownLoadButton()
    {
        newDataObj.SetActive(false);
        downLoadObj.SetActive(true);
        LoadEnd(LOAD.TABLE);
    }

 
    void LoadTable(Uri uri, string assetBundleName, Hash128 hash, uint crc = 0)
    {
        Debug.Log(Caching.ready);

        //構造体生成
        var cachedAssetBundle = new CachedAssetBundle(assetBundleName, hash);


        //全てのtableのキャッシュ削除
        Caching.ClearAllCachedVersions(assetBundleName);


        //if (Caching.IsVersionCached(cachedAssetBundle))
        //{
        //    Debug.Log("キャッシュから");
        //    //キャッシュ存在
        //    string dataPath = AssetBundlePath(cachedAssetBundle);

        //    Debug.Log(dataPath);


        //    var op = UnityEngine.AssetBundle.LoadFromFileAsync(dataPath);
        //    op.completed += (obj) =>
        //    {
        //        // ダウンロード成功
        //        Debug.Log("ダウンロード成功");

        //        AssetBundle bundle = op.assetBundle;

        //        var prefab = bundle.LoadAllAssets();

        //        string text = prefab[0].ToString();

        //        table = JsonUtility.FromJson<AssetBundleTable>(text);

        //        Debug.Log("バージョン" + table.Version);

        //        int preVersion = PlayerPrefs.GetInt("DOWNLOAD", -1);

        //        if (preVersion != table.Version)
        //        {
        //            Debug.Log("新しいバージョンがあります");
        //        }

        //        LoadEnd(LOAD.TABLE);

        //    };
        //}
        //else
        //{
        Debug.Log("サーバーから");

        var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, cachedAssetBundle, crc);

        var op = request.SendWebRequest();
        op.completed += (obj) =>
        {
            if (op.webRequest.isHttpError || op.webRequest.isNetworkError)
            {
                Debug.Log($"ダウンロードに失敗しました!! error:{op.webRequest.error}");
                LoadFailed();
            }
            else
            {
                // ダウンロード成功
                Debug.Log("ダウンロード成功");

                var bundle = DownloadHandlerAssetBundle.GetContent(request);

                var prefab = bundle.LoadAllAssets();

                string text = prefab[0].ToString();

                table = JsonUtility.FromJson<AssetBundleTable>(text);

                Debug.Log("バージョン" + table.Version);

                int preVersion = PlayerPrefs.GetInt("VERSION", -1);

                if (preVersion != table.Version)
                {
                    Debug.Log("新しいバージョンがあります");

                    newDataObj.SetActive(true);
                }
                else
                {
                    Debug.Log("同じバージョン");

                    downLoadObj.SetActive(true);
                    LoadEnd(LOAD.TABLE);
                }

                    

            }

        };

        //}



    }


    void LoadCard(Uri uri, string assetBundleName, Hash128 hash, uint crc, int cardID, bool isDish)
    {
        Debug.Log(Caching.ready);

        //構造体生成
        var cachedAssetBundle = new CachedAssetBundle(assetBundleName, hash);


        //指定バージョン以外削除
        //新しいCRCの場合は古いほうを削除
        Caching.ClearOtherCachedVersions(cachedAssetBundle.name, cachedAssetBundle.hash);


        Debug.Log("cardID " + cardID);

        if (Caching.IsVersionCached(cachedAssetBundle))
        {
            Debug.Log("キャッシュから");
            //キャッシュ存在
            string dataPath = AssetBundlePath(cachedAssetBundle);

            Debug.Log(dataPath);


            var op = UnityEngine.AssetBundle.LoadFromFileAsync(dataPath);
            op.completed += (obj) =>
            {
                Debug.Log("ダウンロード成功");
                AssetBundle bundle = op.assetBundle;


                var prefab = bundle.LoadAllAssets<CardEntity>();


                CardEntity cardEntity = new CardEntity();

                cardEntity = prefab[0] as CardEntity;

                Sprite[] icon = bundle.LoadAllAssets<Sprite>();

                if (icon[0] != null)
                {
                    cardEntity.icon = icon[0];
                }

                if (!isDish)
                {
                    if (icon[1] != null)
                    {
                        cardEntity.glowIcon = icon[1];
                    }

                }


                if (isDish)
                {
                    cardData.dishCardEntity[cardID] = cardEntity;
                }
                else
                {
                    cardData.ingCardEntity[cardID] = cardEntity;

                }

                ShowLoad();
                GenerateCard(cardID + 1, isDish);


            };
        }
        else
        {
            Debug.Log("サーバーから");


            var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, cachedAssetBundle, crc);

            var op = request.SendWebRequest();
            op.completed += (obj) =>
            {
                if (op.webRequest.isHttpError || op.webRequest.isNetworkError)
                {
                    Debug.Log($"ダウンロードに失敗しました!! error:{op.webRequest.error}");
                    LoadFailed();
                }
                else
                {
                    Debug.Log("ダウンロード成功");
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

                    var prefab = bundle.LoadAllAssets<CardEntity>();


                    CardEntity cardEntity = new CardEntity();

                    cardEntity = prefab[0] as CardEntity;

                    Sprite[] icon = bundle.LoadAllAssets<Sprite>();

                    if (icon[0] != null)
                    {
                        cardEntity.icon = icon[0];
                    }

                    if (!isDish)
                    {
                        if (icon[1] != null)
                        {
                            cardEntity.glowIcon = icon[1];
                        }

                    }


                    if (isDish)
                    {
                        cardData.dishCardEntity[cardID] = cardEntity;
                    }
                    else
                    {
                        cardData.ingCardEntity[cardID] = cardEntity;

                    }

                    ShowLoad();
                    GenerateCard(cardID + 1, isDish);

                }

            };

        }

    }




    void GenerateCard(int cardID, bool isDish)
    {
        Debug.Log(cardID);


        if (isDish)
        {
            if (cardID > 26)
            {
                Debug.Log("料理終了");

                cardData.EntityShow();

                LoadEnd(LOAD.DCARD);

                return;
            }
        }
        else
        {
            if (cardID > 8)
            {
                Debug.Log("食材終了");

                //GenerateCard(0, true);

                LoadEnd(LOAD.CARD);

                return;
            }

        }


        string assetBundleName = null;

        Hash128 hash = Hash128.Parse("0");
        uint crc = 0;        

        if (isDish)
        {
            assetBundleName = osName + "/" + table.dishCardRecords[cardID].AssetBundleName;
            hash = table.dishCardRecords[cardID].Hash;
            crc = table.dishCardRecords[cardID].CRC;
        }
        else
        {
            assetBundleName = osName + "/" + table.ingCardRecords[cardID].AssetBundleName;
            hash = table.ingCardRecords[cardID].Hash;
            crc = table.ingCardRecords[cardID].CRC;
        }

        Uri _uri = new Uri(awsUrl + assetBundleName);

        LoadCard(_uri, assetBundleName, hash, crc, cardID, isDish);

    }




    void GenerateBGM(int bgmID)
    {
        if (bgmID > 7)
        {
            LoadEnd(LOAD.BGM);
            return;
        }

        string assetBundleName = osName + "/" + table.bgmRecords[bgmID].AssetBundleName;

        Hash128 hash = table.bgmRecords[bgmID].Hash;
        uint crc = table.bgmRecords[bgmID].CRC;
        Uri _uri = new Uri(awsUrl + assetBundleName);
        
        LoadBGM(_uri, assetBundleName, hash, crc, bgmID);

    }






    void LoadBGM(Uri uri, string assetBundleName, Hash128 hash, uint crc, int bgmID)
    {
        Debug.Log(Caching.ready);

        //構造体生成
        var cachedAssetBundle = new CachedAssetBundle(assetBundleName, hash);


        //指定バージョン以外削除
        //新しいCRCの場合は古いほうを削除
        Caching.ClearOtherCachedVersions(cachedAssetBundle.name, cachedAssetBundle.hash);

        if (Caching.IsVersionCached(cachedAssetBundle))
        {
            Debug.Log("キャッシュから");
            //キャッシュ存在
            string dataPath = AssetBundlePath(cachedAssetBundle);

            Debug.Log(dataPath);


            var op = UnityEngine.AssetBundle.LoadFromFileAsync(dataPath);
            op.completed += (obj) =>
            {
                Debug.Log("ダウンロード成功");
                AssetBundle bundle = op.assetBundle;


                var prefab = bundle.LoadAllAssets<AudioClip>();



                SoundManager.instance.SetAudioClip(bgmID, prefab[0]);

                
                GenerateBGM(bgmID + 1);

                ShowLoad();
            };
        }
        else
        {
            Debug.Log("サーバーから");


            var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, cachedAssetBundle, crc);

            var op = request.SendWebRequest();
            op.completed += (obj) =>
            {
                if (op.webRequest.isHttpError || op.webRequest.isNetworkError)
                {
                    Debug.Log($"ダウンロードに失敗しました!! error:{op.webRequest.error}");
                    LoadFailed();
                }
                else
                {
                    Debug.Log("ダウンロード成功");
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

                    var prefab = bundle.LoadAllAssets<AudioClip>();

                    SoundManager.instance.SetAudioClip(bgmID, prefab[0]);

                    
                    GenerateBGM(bgmID + 1);

                    ShowLoad();
                }

            };

        }

    }


    string AssetBundlePath(CachedAssetBundle cachedAssetBundle)
    {
        string assetDir = Caching.currentCacheForWriting.path;
        string assetBundleName = cachedAssetBundle.name;
        string hash = cachedAssetBundle.hash.ToString();

        return $"{assetDir}/{assetBundleName}/{hash}/__data";

    }






    [Serializable]
    public class AssetBundleRecord : ISerializationCallbackReceiver
    {
        [SerializeField]
        string assetBundleName;
        [SerializeField]
        uint crc;
        [SerializeField]
        string hashString;
        [NonSerialized]
        Hash128 hash;

        public Hash128 Hash => hash;
        public string AssetBundleName => assetBundleName;
        public uint CRC => crc;

        public AssetBundleRecord(string assetBundleName, uint crc, string hashString)
        {
            //OSごとにフォルダ
            this.assetBundleName = assetBundleName;
            this.crc = crc;
            this.hashString = hashString;
            ConvertHashFromString();
        }

        void ConvertHashFromString()
        {
            try
            {
                hash = Hash128.Parse(hashString);
            }
            catch (Exception e)
            {
                hash = new Hash128();
                Debug.LogError(e.Message);
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            ConvertHashFromString();
        }
    }

    [Serializable]
    public class AssetBundleTable : ISerializationCallbackReceiver
    {
        [SerializeField]
        int version;
        //[SerializeField]
        public AssetBundleRecord[] ingCardRecords;
        public AssetBundleRecord[] dishCardRecords;
        public AssetBundleRecord[] bgmRecords;
        [NonSerialized]
        Dictionary<string, AssetBundleRecord> cachedDictionary = new Dictionary<string, AssetBundleRecord>();
        public int Version => version;

        public void OnAfterDeserialize()
        {



        }

        public void OnBeforeSerialize()
        {
            //throw new NotImplementedException();
        }
    }


}



