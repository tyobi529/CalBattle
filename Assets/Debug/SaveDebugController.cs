using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDebugController : MonoBehaviour
{

    [SerializeField] string playerName = null;

    //プレイヤー名
    string name = null;
    //作成料理
    int[] cooked = new int[27];

    //クイズのスコア
    int[] oteQuizScore = new int[5];
    int[] jikQuizScore = new int[5];

    //スピードのスコア
    int[] speedMadeNum = new int[3];

    //バトル勝利数
    int winCount = -1;
    //CPUに勝利したかどうか
    int[] winCPU = new int[3];

    //チュートリアルクリア
    int tutorial = 0;


    [SerializeField] int clearNum = 0;
    

    void Start()
    {

        ShowAllData();

        for (int i = 0; i < clearNum; i++)
        {
            PlayerPrefs.SetInt("MISSION_" + i, 1);

        }

        PlayerPrefs.Save();
    }


    public void SetName()
    {
        Debug.Log("名前セット");

        PlayerPrefs.SetString("NAME", playerName);

        ShowAllData();

    }

    public void ClearTutorial()
    {
        PlayerPrefs.SetInt("TUTORIAL", 1);

        PlayerPrefs.Save();

        ShowAllData();
    }


    public void CompleteData()
    {
        Debug.Log("最大限入力");


        //料理
        for (int i = 0; i < 27; i++)
        {
            PlayerPrefs.SetInt("COOKED_" + i, 1);

        }

        //クイズ
        for (int i = 0; i < 5; i++)
        {
            int oteScore = Random.Range(1000 * (5 - i), 2000 * (5 - i));
            int jikScore = Random.Range(1000 * (5 - i), 2000 * (5 - i));

            PlayerPrefs.SetInt("OTEQUIZSCORE_" + i, 2000);
            PlayerPrefs.SetInt("JIKQUIZSCORE_" + i, 9999);

        }

        //スピード
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("SPEEDMADENUM_" + i, 20 - i);

        }

        //勝利数
        PlayerPrefs.SetInt("WINCOUNT", 99);

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("WINCPU_" + i, 1);
        }


        PlayerPrefs.Save();

        ShowAllData();
    }

    public void ShowAllData()
    {
        Debug.Log("------------------------------------------------------------");
        Debug.Log("全データの表示");
        
        name = PlayerPrefs.GetString("NAME", "なし");        
        for (int i = 0; i < 27; i++)
        {
            cooked[i] = PlayerPrefs.GetInt("COOKED_" + i, 0);
        }

        //クイズ
        for (int i = 0; i < 5; i++)
        {
            oteQuizScore[i] = PlayerPrefs.GetInt("OTEQUIZSCORE_" + i, 0);
            jikQuizScore[i] = PlayerPrefs.GetInt("JIKQUIZSCORE_" + i, 0);
        }

        //スピード
        for (int i = 0; i < 3; i++)
        {
            speedMadeNum[i] = PlayerPrefs.GetInt("SPEEDMADENUM_" + i, 0);
        }



        Debug.Log("プレイヤー名" + name);

        string dishNum = "";
        for (int i = 0; i < 27; i++)
        {
            if (cooked[i] == 1)
            {
                dishNum += i + " ";
            }
        }

        Debug.Log("作成料理 " + dishNum);

        string oteText = "おてがるクイズ：";
        string jikText = "じっくりクイズ：";
        for (int i = 0; i < 5; i++)
        {
            oteText += (i + 1).ToString() + "位" + oteQuizScore[i].ToString() + " ";
            jikText += (i + 1).ToString() + "位" + jikQuizScore[i].ToString() + " ";
        }

        Debug.Log(oteText);
        Debug.Log(jikText);


        
        string speedText = "スピード：";
        for (int i = 0; i < 3; i++)
        {
            speedText += (i + 1).ToString() + "位" + speedMadeNum[i].ToString() + " ";
        }

        Debug.Log(speedText);

        winCount = PlayerPrefs.GetInt("WINCOUNT", 0);

        Debug.Log("勝利数" + winCount);

        for (int i = 0; i < 3; i++)
        {
            winCPU[i] = PlayerPrefs.GetInt("WINCPU_" + i, 0);
            Debug.Log("CPU" + i.ToString() + "は" + winCPU[i]);
        }


        tutorial = PlayerPrefs.GetInt("TUTORIAL", 0);

        Debug.Log("チュートリアル" + tutorial);

    }

    //名前以外ランダムに入れる
    public void SetRandomData()
    {
        Debug.Log("ランダムにセット");


        //料理
        for (int i = 0; i < 27; i++)
        {
            int a = Random.Range(0, 2);
            
            PlayerPrefs.SetInt("COOKED_" + i, a);
            
        }

        //クイズ
        for (int i = 0; i < 5; i++)
        {
            int oteScore = Random.Range(1000 * (5 - i), 2000 * (5 - i));
            int jikScore = Random.Range(1000 * (5 - i), 2000 * (5 - i));

            PlayerPrefs.SetInt("OTEQUIZSCORE_" + i, oteScore);
            PlayerPrefs.SetInt("JIKQUIZSCORE_" + i, jikScore);

        }

        //スピード
        for (int i = 0; i < 3; i++)
        {
            int speed = Random.Range(5 * (3 - i), 10 * (3 - i));

            PlayerPrefs.SetInt("SPEEDMADENUM_" + i, speed);

        }

        //勝利数
        int winCount = Random.Range(0, 99);
        PlayerPrefs.SetInt("WINCOUNT", winCount);

        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(0, 2);
            PlayerPrefs.SetInt("WINCPU_" + i, a);
        }


        PlayerPrefs.Save();

        ShowAllData();
    }


    public void SetAllDish()
    {
        Debug.Log("全料理作成");

        for (int i = 0; i < 27; i++)
        {
            PlayerPrefs.SetInt("COOKED_" + i, 1);

        }

        PlayerPrefs.Save();

        ShowAllData();


    }


    public void ResetName()
    {
        Debug.Log("名前削除");
        PlayerPrefs.DeleteKey("NAME");        

        name = PlayerPrefs.GetString("NAME", "なし");
        

        ShowAllData();
    }




}
