using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchingManager : MonoBehaviourPunCallbacks
{
    MatchingController matchingController;

    

    // Start is called before the first frame update
    void Start()
    {
        matchingController = GameObject.Find("MatchingController").GetComponent<MatchingController>();

        //二人目のプレイヤーならば
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            photonView.RPC(nameof(MoveToGameScene), RpcTarget.AllViaServer);

        }

    }

    





    //マッチング成功後にゲームシーンへ移る
    [PunRPC]
    void MoveToGameScene()
    {
        StartCoroutine(matchingController.MoveToGameScene());
        
    }
}
