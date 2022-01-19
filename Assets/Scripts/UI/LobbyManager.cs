using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;

public enum CustomEventCode
{
	ForceRefresh = 100,
	CharacterSelectionReady = 101,
	CharacterSelcetionChangedCharacter = 102,
	CharacterSelectionClass = 103
}

public class LobbyManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
	[SerializeField] private GameObject _playerSlotPrefab;
	[SerializeField] private Transform _playerListParent;
	[SerializeField] private Button _readyButton;
	[SerializeField] private TMP_Text _text;
	[SerializeField] private int _seconds;

	private bool _ready;
	private List<GameObject> _spawnedSlots = new List<GameObject>();

	public void ReadyUp()
	{
		Debug.Log("Readying Up!");

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

		_ready = !_ready;
		hash.Add("Ready", _ready);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		RaiseEventOptions options = new RaiseEventOptions();
		options.Receivers = ReceiverGroup.All;
		PhotonNetwork.RaiseEvent((byte)CustomEventCode.ForceRefresh, null, options, SendOptions.SendReliable);
	}

	public void RefreshPlayers()
	{
		Debug.Log("Refreshing Player List");

		if (_spawnedSlots.Count > 0)
		{
			for (int i = 0; i < _spawnedSlots.Count; i++)
			{
				Destroy(_spawnedSlots[i]);
			}
		}

		// Spawn existing player slots
		if (PhotonNetwork.InRoom)
		{
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				GameObject obj = Instantiate(_playerSlotPrefab, _playerListParent);
				obj.transform.SetParent(_playerListParent);
				obj.GetComponent<SetPlayerSlot>().SetPlayers(player);

				_spawnedSlots.Add(obj);
			}
		}

		//Start count down before changing level
		if (IsAllReady())
		{
			StartCoroutine(CountDownStart(_seconds));
		}
	}

	IEnumerator CountDownStart(int seconds)
	{
		float time = seconds;

		while (time > 0)
		{
			_text.text = $"The game will start in {time} seconds!";
			time--;

			if (!IsAllReady())
				yield return null;

			yield return new WaitForSeconds(1);
		}

		PhotonNetwork.LoadLevel(1);

		yield return null;
	}

	private bool IsAllReady()
	{
		foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
		{
			if (!player.CustomProperties.ContainsKey("Ready") || !(bool)player.CustomProperties["Ready"])
			{
				return false;
			}
		}
		return true;
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshPlayers();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayers();
	}

	public override void OnJoinedRoom()
	{
		RefreshPlayers();
	}

	public override void OnLeftRoom()
	{
		RefreshPlayers();
	}

	public void OnEvent(EventData photonEvent)
	{
		Debug.Log("Event Raised");

		if (photonEvent.Code == (byte)CustomEventCode.ForceRefresh)
			RefreshPlayers();
	}
}
