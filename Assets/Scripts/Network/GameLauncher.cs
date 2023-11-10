using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Network {
    public class GameLauncher : MonoBehaviourPunCallbacks {
        private static readonly int MaxPlayersPerRoom = 2;
        private const string _gameVersion = "1.0";
        private void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start() {
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void StartMatching() {
            Match();
        }

        private void Match() {
            if (PhotonNetwork.IsConnected) {
                RoomOptions options = new RoomOptions { MaxPlayers = MaxPlayersPerRoom };
                PhotonNetwork.JoinRandomOrCreateRoom(
                    expectedMaxPlayers: (byte)MaxPlayersPerRoom,
                    roomOptions:options
                );
            }
        }

        #region Callbacks
        public override void OnConnectedToMaster() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        #endregion
    }
}