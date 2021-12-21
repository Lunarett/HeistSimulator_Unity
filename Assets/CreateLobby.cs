using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CreateLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TMP_Text _maxPlayerDisplay;
    [SerializeField] private Slider _slider;
    [SerializeField] private NetworkManager _nm;
    [SerializeField] private PanelManager _panel;
    [SerializeField] private int _panelIndex = 4;
    [SerializeField] private LobbyManager _lobbyManager;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(delegate { UpdateMaxPlayerText(); });
    }

    private void Start()
    {
        UpdateMaxPlayerText();
    }

    public void Create()
    {
        if(PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            _nm.CreateRoom(_input.text, (byte)_slider.value);
        }
    }

    private void UpdateMaxPlayerText()
    {
        _maxPlayerDisplay.text = _slider.value.ToString();
    }

    public override void OnJoinedRoom()
    {
        _panel.SetPanel(_panelIndex);
        _lobbyManager.RefreshPlayers();
    }
}
