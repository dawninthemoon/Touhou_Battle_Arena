using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

namespace Network {
    public class NetworkManager : MonoBehaviourPunCallbacks {
        private static readonly int MaxPlayersPerRoom = 2;
        private int _updatedProperties;
        public TMP_Text StatusText;

        private void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start() {
            Connect();
        }

        void Update() {
            // 현재 어떤 상태인지. (정의로 올라가다보면, Enum으로 ClientState가 선언되어있다.)
            StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        }

        #region Public Methods

        public void Connect() {
            // 처음에 Photon Online Server에 접속하는 게 가장 중요함!!
            // Photon Online Server에 접속하기.
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Disconnect() {
            // 연결 끊기.
            PhotonNetwork.Disconnect();
        }

        public void StartMatch() {
            RoomOptions options = new RoomOptions { MaxPlayers = MaxPlayersPerRoom };
            PhotonNetwork.JoinRandomOrCreateRoom(
                expectedMaxPlayers: (byte)MaxPlayersPerRoom,
                roomOptions:options
            );
        }

        public void CancelMatch() {
            PhotonNetwork.LeaveRoom();
        }

        public void CreateRoom() {
            // 방 생성하고, 참가.
            // 방 이름, 최대 플레이어 수, 비공개 등을 지정 가능.
            //PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
        }

        public void JoinRoom() {
            // 방 참가하기.
            // 방 이름으로 입장 가능.
            //PhotonNetwork.JoinRoom(roomInput.text);
        }

        public void JoinOrCreateRoom() {
            // 방 참가하는데, 방이 없으면 생성하고 참가.
            string id = "1234";
            PhotonNetwork.JoinOrCreateRoom(id, new RoomOptions { MaxPlayers = 2 }, null);
        }

        public void JoinRandomRoom() {
            // 방 랜덤으로 참가하기.
            PhotonNetwork.JoinRandomRoom();
        }

        public void LeaveRoom() {
            // 방 떠나기.
            PhotonNetwork.LeaveRoom();
        }

        #endregion


        #region Photon Callbacks
        public override void OnConnectedToMaster() {
            print("서버 접속 완료.");

            // 현재 플레이어 닉네임 설정.
            //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        }

        public override void OnDisconnected(DisconnectCause cause) {
            print("연결 끊김.");
        }
        public override void OnJoinedLobby() {
            print("로비 접속 완료.");
        }

        public override void OnCreatedRoom() {
            print("방 만들기 완료.");
        }

        public override void OnJoinedRoom() {
            print("방 참가 완료.");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            if (PhotonNetwork.IsMasterClient) {
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
                    TeamColor color = ExTeamColor.GetRandomColor();
                    TeamColor opponentColor = ExTeamColor.GetOpponentColor(color);

                    string teamColorID = "TeamColor";
                    Player otherPlayer = PhotonNetwork.PlayerListOthers[0];

                    PhotonNetwork.LocalPlayer.SetCustomProperties(
                        new ExitGames.Client.Photon.Hashtable { {teamColorID, color}}
                    );
                    otherPlayer.SetCustomProperties(
                        new ExitGames.Client.Photon.Hashtable { {teamColorID, opponentColor}}
                    );
                }
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {
            ++_updatedProperties;
            if ((_updatedProperties == 2) && PhotonNetwork.IsMasterClient) {
                PhotonNetwork.LoadLevel("MultiGameScene");
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            print("방 만들기 실패.");
        }

        public override void OnJoinRoomFailed(short returnCode, string message) {
            print("방 참가 실패.");
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            print("방 랜덤 참가 실패.");
        }
        #endregion


        [ContextMenu("정보")]
        private void Info() {
            if (PhotonNetwork.InRoom) {
                print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
                print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
                print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

                string playerStr = "방에 있는 플레이어 목록 : ";
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
                print(playerStr);
            }
            else {
                print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
                print("방 개수 : " + PhotonNetwork.CountOfRooms);
                print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
                print("로비에 있는지? : " + PhotonNetwork.InLobby);
                print("연결됐는지? : " + PhotonNetwork.IsConnected);
            }
        }
    }
}