using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _buttonText;
    [Space]
    [SerializeField] private PanelManager _panelManager;
    [SerializeField] private int _panelIndex = 4;

    private RoomOptions _options = new RoomOptions();
    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    public Dictionary<string, RoomInfo> CachedRoomList { get => _cachedRoomList; }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        ToggleButton(PhotonNetwork.IsConnectedAndReady);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
        ToggleButton(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ToggleButton(false);
    }

    public void ToggleButton(bool isActive)
    {
        _button.interactable = isActive;
        float alpha = isActive ? 1 : 0.5f;

        _buttonText.color = new Color(_buttonText.color.r, _buttonText.color.g, _buttonText.color.b, alpha);
    }

    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinedRoom()
    {
        _panelManager.SetPanel(_panelIndex);
    }

    public void CreateRoom(string roomName, byte maxPlayers)
    {
        _options.MaxPlayers = maxPlayers;

        if (PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.CreateRoom(roomName, _options, TypedLobby.Default);
        }
    }
    
    public void LeaveRoom()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Left Room");
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
