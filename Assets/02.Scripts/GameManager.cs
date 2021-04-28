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
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public Button exitButton;
    
    private void Awake()
    {
        Vector3 pos = new Vector3(Random.Range(-200.0f, 200.0f), 5.0f, Random.Range(-200.0f, 200.0f));

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }
    
    void Start()
    {
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


}
