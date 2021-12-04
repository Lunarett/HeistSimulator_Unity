using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUsername : MonoBehaviour
{
	[SerializeField] private PanelManager _panelManager;
	[SerializeField] private int _nextPanelID;
	[Space]
	[Header("References & Properties")]
	[SerializeField] private TMP_Text _usernameDisplayText;
	[SerializeField] private TMP_InputField _usernameInputField;
	[SerializeField] private TMP_Text _errorMessageText;
	[SerializeField] private GameObject _ErrorMessageObject;
	[Space]
	[SerializeField] private int _minCharacters;
	[Space]
	[Header("Error Messages")]
	[SerializeField] private string _emptyMessage = "Error: You need to enter a name!";
	[SerializeField] private string _exceededMessage = "Error: You have exceeded the minimum amount of characters";

	private static string _username;

	public string GetUsername { get => _username; }

	private void Start()
	{
		if(_username != null)
		{
			_usernameDisplayText.text = _username;
			_panelManager.SetPanel(_nextPanelID);
		}
	}

	public void ConfirmName()
	{
		string name = _usernameInputField.text;

		if(name == string.Empty)
		{
			_ErrorMessageObject.SetActive(true);
			_errorMessageText.text = _emptyMessage;
			return;
		}

		_username = _usernameInputField.text;
		_usernameDisplayText.text = _usernameInputField.text;

		_panelManager.SetPanel(_nextPanelID);
	}
}
