using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // 마스터서버 접속
    public static void ConnectMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속
    public static void ConnectRoom()
    {

    }

    // 마스터 서버 접속 성공 콜백
    public override void OnConnectedToMaster()
    {
        StateManager.Instance.currentState_net = (int)StateManager.Net.Master;
    }

    // 마스터 서버 접속 실패 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        StateManager.Instance.currentState_net = (int)StateManager.Net.ReConnect;
        PhotonNetwork.ConnectUsingSettings();
    }



}
