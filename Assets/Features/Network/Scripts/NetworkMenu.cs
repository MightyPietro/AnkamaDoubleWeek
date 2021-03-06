using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
namespace WeekAnkama
{
    public class NetworkMenu : MonoBehaviourPun
    {

        [SerializeField] private GameObject _firstLobby;
        [SerializeField] private GameObject _roomLobby;
        [SerializeField] private Text _roomInfos;
        [SerializeField] private Text _inputFieldText;

        private void OnEnable()
        {
            NetworkManager.OnRoomJoined += ShowRoomLobby;
            NetworkManager.OnPlayerEnterRoom += ShowRoomLobby;
            NetworkManager.OnPlayerLeaveRoom += ShowRoomLobby;
        }


         public void JoinRoom()
        {
            NetworkManager.instance.JoinRoom(_inputFieldText.text);
        }
        public void LaunchGameViaRPC()
        {
            photonView.RPC("LaunchGame", RpcTarget.All);
        }

        [PunRPC]
        private void LaunchGame()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void ShowRoomLobby()
        {
            _firstLobby.SetActive(false);
            _roomLobby.SetActive(true);
            _roomInfos.text = PhotonNetwork.CurrentRoom.ToString();
        }

        private void OnDisable()
        {
            NetworkManager.OnRoomJoined += ShowRoomLobby;
            NetworkManager.OnPlayerEnterRoom += ShowRoomLobby;
            NetworkManager.OnPlayerLeaveRoom += ShowRoomLobby;
        }
    }
}

