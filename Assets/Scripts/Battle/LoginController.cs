using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourではなくMonoBehaviourPunCallbacksを継承して、Photonのコールバックを受け取れるようにする
public class LoginController : MonoBehaviourPunCallbacks
{
    [SerializeField] Setting setting = null;

    [SerializeField] GameObject singleGameManager = null;
    //以下テスト用

    private void Start()
    {
        Debug.Log("Isconnetcted" + PhotonNetwork.IsConnected);


        
        //ネット対戦
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.IsMessageQueueRunning = true;

            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                PhotonNetwork.Instantiate("GameManager", v, Quaternion.identity);
            }
        }
        //ネット対戦のデバック用
        //GameSceneだけで動く
        else if (setting.onlineDebug)
        {
            // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        }
        //ソロ対戦
        else
        {
            singleGameManager.SetActive(true);
        }

    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));


            PhotonNetwork.Instantiate("GameManager", v, Quaternion.identity);
        }

    }




}