using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[Header("FMOD UI Sounds")]
	[FMODUnity.EventRef]
	[SerializeField] private string _startButtonSound;
	[FMODUnity.EventRef]
	[SerializeField] private string _clickButtonSound;

	[SerializeField] private float _buttonDelay;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartButton()
	{
		FMODUnity.RuntimeManager.PlayOneShot(_startButtonSound, gameObject.transform.position);

		StartCoroutine(DelayLoadLevel(_buttonDelay));
		Time.timeScale = 1;
	}

	public void SettingsButton()
	{
		FMODUnity.RuntimeManager.PlayOneShot(_clickButtonSound, gameObject.transform.position);
	}

	public void ExitButton()
	{
		FMODUnity.RuntimeManager.PlayOneShot(_clickButtonSound, gameObject.transform.position);

		StartCoroutine(DelayExitGame(_buttonDelay));
	}

	IEnumerator DelayLoadLevel(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		
		FMODPlay music = GetComponent<FMODPlay>();

		if (music != null)
			music.StopAudio();

		SceneManager.LoadScene(1);
	}

	IEnumerator DelayExitGame(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Application.Quit();
	}
}
