using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetLobbySlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _players;
    [SerializeField] private TMP_Text _maxPlayers;

    private NetworkManager _networkMng;
    
    public void UpdateValues(string name, int players, int maxPlayers, NetworkManager networkMng)
    {
        _name.text = name;
        _players.text = players.ToString();
        _maxPlayers.text = maxPlayers.ToString();

        _networkMng = networkMng;
    }

    public void JoinRoom()
    {
        _networkMng.JoinRoom(_name.text);
    }
}
