using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public TMP_Text messageText;

    [Header("Chatting UI")]
    public TMP_Text chatListText;
    public TMP_InputField msgIF;

    public Button exitButton;

    private PhotonView pv;

    // 싱글턴 변수
    public static GameManager instance = null;
    
    private void Awake()
    {
        instance = this;

        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 50.0f, Random.Range(-150.0f, 150.0f));

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
        // pv = photonView;
        SetRoomInfo();
    }

    void SetRoomInfo()
    {
        Room currentRoom =  PhotonNetwork.CurrentRoom;
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
    }

    // 클론을 지우는 등 cleanUp 작업
    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    // CleanUp 끝난 후에 호출되는 콜백
    // 이미 로비에 있는 상태이므로 씬만 바꿔주면 됨
    public override void OnLeftRoom()
    {
        // Lobby 씬으로 되돌아 가기...
        SceneManager.LoadScene("Lobby");
    }

    // 새로운 유저가 들어왔을 때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        messageText.text += msg;
    }

    // 유저가 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is leaved room";
        messageText.text += msg;
    }

    public void OnSendClick()
    {
        string _msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {msgIF.text}";
        pv.RPC("SendChatMessage", RpcTarget.AllBufferedViaServer, _msg);
    }

    [PunRPC]
    void SendChatMessage(string msg)
    {
        chatListText.text += $"{msg}\n";
    }
}
