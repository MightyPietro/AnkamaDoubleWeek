
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace WeekAnkama
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager instance;
        public delegate void Action();
        public static event Action OnConnectedToServer;
        public static event Action OnRoomJoined;
        public static event Action OnPlayerEnterRoom;
        public static event Action OnPlayerLeaveRoom;
        public static event Action OnRoomCreated;
        public static event Action OnLobbyJoined;
        public static event Action OnRoomsListUpdate;

        public UnityEngine.Events.UnityEvent _OnAwake;

        public bool isSolo;
        [SerializeField] private IntVariable _playerValue;

        private void Awake()
        {
            _OnAwake.Invoke();
            instance = this;
            if (!PhotonNetwork.IsConnected)
            {
                if (!isSolo)
                    ConnectToServer();
                else _playerValue.Value = -1;
            }
        }


        public void ConnectToServer()
        {
            PhotonNetwork.ConnectUsingSettings();

        }

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName, roomOptions);
            OnRoomCreated?.Invoke();
        }
        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            OnRoomsListUpdate?.Invoke();

        }

        public void ConnectToLobby()
        {
            TypedLobby lobby = new TypedLobby("Lobby", LobbyType.Default);
            PhotonNetwork.JoinLobby(lobby);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            ConnectToLobby();
            OnConnectedToServer?.Invoke();
            Debug.Log("Connected to : " + PhotonNetwork.CloudRegion);
        }

        
        public override void OnJoinedLobby()
        {
            OnLobbyJoined?.Invoke();
        }


        public override void OnJoinedRoom()
        {
            OnRoomJoined?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            OnPlayerEnterRoom?.Invoke();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            OnPlayerLeaveRoom?.Invoke();
        }

        public PhotonView pView => photonView;

        public void GoToMenu()
        {
            LeaveRoom();
            SceneManager.LoadSceneAsync(0);
        }
    }

}

