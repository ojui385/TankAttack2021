using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "Ojui";

    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    // 룸 목록 저장하기 위한 딕셔너리 자료형
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // 룸을 표시할 프리팹
    public GameObject roomPrefab;
    // Room 프리팹이 차일드화 시킬 부모 객체
    public Transform scrollContent;

    private void Awake()
    {
        // 방장이 혼자 씬을 로딩하면, 나머지 사람들은 자동으로 싱크가 됨
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        // 유저명 지정
        // PhotonNetwork.NickName = userId;

        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }

    // 포톤 서버에 접속했다는 것을 의미
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!!");
        // PhotonNetwork.JoinRandomRoom(); // 랜덤한 룸에 접속 시도

        // 로비에 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined lobby!!!");
    }

    // 랜덤 룸에 접속 시도했으나, 만들어진 룸이 없어서 실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code={returnCode}, msg={message}");

        // 룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;    // 룸 목록에 내 방이 보이도록 설정
        ro.MaxPlayers = 30;

        roomNameText.text = $"Room_{Random.Range(1, 100):000}";

        // 룸을 생성 > 자동 입장됨
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    // 룸 생성 완료 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    // 룸에 입장했을 때 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        // 이 함수를 쓰지 않으면 통신이 끊김 -> 다시 이어줘야함
        // 이 함수는 통신을 끊고 다시 연결함
        // 방장이 씬 로드함
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        // PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }

    // 룸 목록이 변경(갱신)될 때마다 호출되는 콜백함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (var room in roomList)
        {
            // 룸 삭제된 경우 
            if (room.RemovedFromList == true)
            {
                // 딕셔너리에 삭제, roomItem 프리팹 삭제
                roomDict.TryGetValue(room.Name, out tempRoom);
                // RoomItem 프리팹을 삭제
                Destroy(tempRoom);
                // 딕셔너리에서 데이터를 삭제
                roomDict.Remove(room.Name);
            }
            else // 룸 정보가 갱신(변경)
            {
                // 처음 생성된 경우 딕셔너리에 데이터 추가 + roomItem 생성
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    // 룸 정보 표시
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    // 딕셔너리에 데이터 추가
                    roomDict.Add(room.Name, _room);
                }
                // 룸 정보를 갱신
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;

                }
            }
        }
    }

    #region UI_BUTTON_CALLBACK
    public void OnLoginClick()
    {
        if (string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        // 룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;    // 룸 목록에 내 방이 보이도록 설정
        ro.MaxPlayers = 30;

        if (string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(1, 100):000}";
        }

        // 룸을 생성 > 자동 입장됨
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }


    #endregion


}
