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
        [SerializeField] private Text _inputFieldNameText;
        [SerializeField] private Text[] _playersText;
        [SerializeField] private Button[] _valueSelectionButton;
        [SerializeField] private IntVariable _playerValue;


        private void OnEnable()
        {
            NetworkManager.OnRoomJoined += ShowRoomLobby;
            NetworkManager.OnRoomJoined += ChangeRoomPlayerNameViaRPC;
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

        public void ChangePlayerNickName()
        {
            PhotonNetwork.NickName = _inputFieldNameText.text;
        }

        public void ChangeRoomPlayerNameViaRPC()
        {
            photonView.RPC("ChangeRoomPlayerName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }

        public void ResetRoomPlayerNameViaRPC()
        {
            photonView.RPC("ResetRoomPlayerName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }

        public void HideValueSelectionButtonViaRPC(int ID)
        {
            photonView.RPC("HideValueSelectionButton", RpcTarget.AllBuffered, ID);
        }
        public void ShowValueSelectionButtonViaRPC()
        {
            photonView.RPC("ShowValueSelectionButton", RpcTarget.AllBuffered);
        }


        [PunRPC]
        private void HideValueSelectionButton(int ID)
        {
            _valueSelectionButton[ID].interactable = false;
        }

        
        [PunRPC]
        private void ShowValueSelectionButton()
        {
            for (int i = 0; i < _valueSelectionButton.Length; i++)
            {
                if(i == _playerValue.Value - 1)
                {
                    _valueSelectionButton[i].interactable = true;
                    break;
                }
            }
            
        }

        [PunRPC]
        private void ChangeRoomPlayerName(string name)
        {
            for (int i = 0; i < _playersText.Length; i++)
            {
                if(_playersText[i].text == "PLAYER")
                {
                    _playersText[i].text = name;
                    break;
                }
            }
        }

        [PunRPC]
        private void ResetRoomPlayerName(string name)
        {
            for (int i = 0; i < _playersText.Length; i++)
            {
                if (_playersText[i].text == name)
                {
                    _playersText[i].text = "PLAYER";
                    break;
                }
            }
        }

        public void LaunchLocal()
        {
            SceneManager.LoadSceneAsync(1);
        }

        private void OnDisable()
        {
            NetworkManager.OnRoomJoined -= ShowRoomLobby;
            NetworkManager.OnRoomJoined -= ChangeRoomPlayerNameViaRPC;
            NetworkManager.OnPlayerEnterRoom -= ShowRoomLobby;
            NetworkManager.OnPlayerLeaveRoom -= ShowRoomLobby;

        }
    }
}

