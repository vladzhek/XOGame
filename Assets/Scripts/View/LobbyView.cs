using Data;
using Infastructure;
using Models;
using Photon.Pun;
using Photon.Realtime;
using Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyView : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _createButton;
    [SerializeField] private GameObject _panelStartGame;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _roomIDField;

    [Inject] private WindowService _windowService;
    [Inject] private GridModel _gridModel;
    [Inject] private SyncManager _syncManager;
    [Inject] private StaticDataService _dataService;
    [Inject] private IServerModel _serverModel;
    
    private string roomName;
    private string typePlayer;

    public override void OnEnable()
    {
        base.OnEnable();
        _joinButton.onClick.AddListener(OnJoinButtonClicked);
        _createButton.onClick.AddListener(OnCreateButtonClicked);
        _startGameButton.onClick.AddListener(StartGame);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _joinButton.onClick.RemoveListener(OnJoinButtonClicked);
        _createButton.onClick.RemoveListener(OnCreateButtonClicked);
        _startGameButton.onClick.RemoveListener(StartGame);
    }

    private void Start()
    {
        InjectService.Instance.Inject(this);
        
        gameObject.GetComponent<PhotonView>().ViewID = PhotonNetwork.AllocateViewID(998);
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnJoinButtonClicked()
    {
        roomName = _roomIDField.text;
        _dataService.PlayerType = EPlayerType.Client;
        _dataService.CurrentPlayer = ETurnPlayers.Player2;
        PhotonNetwork.JoinRoom(roomName);
    }

    private void OnCreateButtonClicked()
    {
        roomName = _roomIDField.text;
        _dataService.PlayerType = EPlayerType.Host;
        _dataService.CurrentPlayer = ETurnPlayers.Player1;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });

        Debug.Log($"Room created: {PhotonNetwork.CurrentRoom?.Name}, MaxPlayers: {PhotonNetwork.CurrentRoom?.MaxPlayers}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        _statusText.text = "The network is available";
    }

    public override void OnJoinedRoom()
    {
        _panelStartGame.SetActive(true);
        _startGameButton.gameObject.SetActive(false);
        if (_dataService.PlayerType == EPlayerType.Host)
        {
            _statusText.text = "Waiting Player 2 ";
        }
        if (_dataService.PlayerType == EPlayerType.Client)
        {
            _statusText.text = "Waiting Start Game";
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _statusText.text = "Failed to join room " + roomName + ": " + message;
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _statusText.text = "Player joined in lobby";
            if(_dataService.PlayerType == EPlayerType.Host)
                _startGameButton.gameObject.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _syncManager.SyncGridData(_gridModel.Matrix);
            _serverModel.InitializeStartGame(_dataService.PlayerType, _gridModel.Matrix);
            _serverModel.PlayerTurn(ETurnPlayers.Player1);
        }
    }
    
    private void StartGame()
    {
        photonView.RPC("CloseWindowLobby", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void CloseWindowLobby()
    {
        _windowService.OpenWindow(WindowType.Gameplay);
        _windowService.CloseWindow(WindowType.Lobby);
    }
}