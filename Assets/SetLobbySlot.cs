using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SetLobbySlot : MonoBehaviour
{
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _players;
	[SerializeField] private TMP_Text _maxPlayers;
	[SerializeField] private Image _pingImage;
	[SerializeField] private TMP_Text _pingText;
	[Space]
	[Header("Ping States")]
	[SerializeField] private Sprite _ping4;
	[SerializeField] private Sprite _ping3;
	[SerializeField] private Sprite _ping2;
	[SerializeField] private Sprite _ping1;
	[Space]
	[SerializeField] private int _goodPing = 50;
	[SerializeField] private int _okPing = 150;
	[SerializeField] private int _badPing = 200;

	private NetworkManager _networkMng;

	private void Start()
	{
		_pingText.text = PhotonNetwork.GetPing().ToString();
		SetPingImage();
	}

	private void SetPingImage()
	{
		if (PhotonNetwork.GetPing() < _goodPing) _pingImage.sprite = _ping4;
		else if (PhotonNetwork.GetPing() > _goodPing && PhotonNetwork.GetPing() < _okPing) _pingImage.sprite = _ping3;
		else if (PhotonNetwork.GetPing() > _okPing && PhotonNetwork.GetPing() < _badPing) _pingImage.sprite = _ping2;
		else if (PhotonNetwork.GetPing() > _badPing) _pingImage.sprite = _ping1;
	}

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
