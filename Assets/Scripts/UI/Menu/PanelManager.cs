using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PanelManager : MonoBehaviour
{
	[Header("Panels")]
	[SerializeField] private int _startingPanelIndex = 0;
	[SerializeField] private GameObject[] _panels;

	private void Start()
	{
		if (PhotonNetwork.NickName != string.Empty || PhotonNetwork.NickName != null)
		{
			SetPanel(_startingPanelIndex);
		}
		else
		{
			SetPanel(_startingPanelIndex + 1);
		}
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void SetPanel(int panelIndex)
	{
		for (int i = 0; i < _panels.Length; i++)
		{
			_panels[i].SetActive(i == panelIndex);
		}
	}

	public void CloseAllPanels()
	{
		foreach (GameObject go in _panels)
		{
			go.SetActive(false);
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
