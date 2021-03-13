using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

// MonoBehaviourではなくMonoBehaviourPunCallbacksを継承して、Photonのコールバックを受け取れるようにする
public class MatchingController : MonoBehaviourPunCallbacks
{
    //[SerializeField] MatchingUIManager matchingUIManager;
    //[SerializeField] GameObject searchingPanelObject;
    [SerializeField] Page_0Controller page_0Controller;

    string roomName = null;

    //ルームオプションのプロパティー
    static RoomOptions RoomOPS = new RoomOptions()
    {
        MaxPlayers = 2, //0だと人数制限なし
        IsOpen = true, //部屋に参加できるか
        IsVisible = true, //この部屋がロビーにリストされるか
    };


    private void Start()
    {
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        //テスト
        //PhotonNetwork.ConnectUsingSettings();

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        
    }






    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        if (roomName == null)
        {
            //("ルームの名前",ルームオプションの変数,新規ルームを一覧したいロビー。nullで無視)            
            PhotonNetwork.JoinRandomRoom();

            Debug.Log("ランダム");
        }
        else
        {
            //ルームID指定部屋はランダムで入室できないように
            RoomOptions keyRoomOPS = RoomOPS;
            keyRoomOPS.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(roomName, keyRoomOPS, null);

            Debug.Log("プライベート");
        }

        Debug.Log("ルーム作成");
    }

    //失敗した時（ルームがない時呼ばれる）
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("入室失敗");
        //ルームを作成する。
        RoomOptions keyRoomOPS = RoomOPS;
        keyRoomOPS.IsVisible = true;
        //PhotonNetwork.CreateRoom(null, RoomOPS); //JoinOrCreateroomと同じ引数が使用可能。nullはルーム名を作成したくない場合roomNameを勝手に割り当てる
        PhotonNetwork.CreateRoom(null, keyRoomOPS);
    }

    //ルーム作成失敗したときの動作。
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        Debug.Log("作成失敗");

    }


    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        //Debug.Log("マッチング");

        Room myroom = PhotonNetwork.CurrentRoom;　//myroom変数にPhotonnetworkの部屋の現在状況を入れる。
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;　//playerをphotonnetworkのローカルプレイヤーとする
        Debug.Log("ルーム名:" + myroom.Name);
        Debug.Log("PlayerNo" + player.ActorNumber);
        Debug.Log("プレイヤーID" + player.UserId);

        Debug.Log("ルームマスター" + player.IsMasterClient); //ルームマスターならTrur。最初に部屋を作成した場合は、基本的にルームマスターなはず。



        //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        if (player.IsMasterClient)
        {
            //var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            var v = new Vector3(0f, 0f, 0f);
            PhotonNetwork.Instantiate("MatchingManager", v, Quaternion.identity);
        }


    }

    // ロビーから出た時
    public override void OnLeftLobby()
    {
        Debug.Log("ロビーから退出");
    }


    // 部屋から退室した時
    public override void OnLeftRoom()
    {

        if (PhotonNetwork.InLobby)
        {
            // 退室
            PhotonNetwork.LeaveLobby();

        }

        Debug.Log("部屋から退出");
    }

    public IEnumerator MoveToGameScene()
    {
        //matchingUIManager.MatchingSuccess();
        page_0Controller.MatchingSuccess();
        
        yield return new WaitForSeconds(2);


        PhotonNetwork.IsMessageQueueRunning = false;
        //SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        FadeManager.FadeOut(2);
    }


    public void StartMatching(string roomName)
    {
        this.roomName = null;
        this.roomName = roomName;

        Debug.Log(roomName);
        PhotonNetwork.ConnectUsingSettings();        
    }


    public void CancelMatching()
    {

        PhotonNetwork.Disconnect();

    }





}