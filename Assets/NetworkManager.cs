using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private Button _button;
	[SerializeField] private TMP_Text _buttonText;

	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		ToggleButton(PhotonNetwork.IsConnectedAndReady);
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected To Server");
		PhotonNetwork.AutomaticallySyncScene = true;
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
}
