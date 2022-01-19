using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _player;
	private string msg;

	void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), transform.position, Quaternion.identity);
			msg = "<color=green>Connected</color>";
		}
		else
		{
			Instantiate(_player, transform.position, Quaternion.identity);
			msg = "<color=red>Offline</color>";
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("Debug Mode\n------------------");
		GUILayout.Label("Connection Status " + msg);
	}
}
