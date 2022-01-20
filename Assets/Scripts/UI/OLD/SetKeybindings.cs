using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetKeybindings : MonoBehaviour
{
    [SerializeField] private Keybinding _keyDescription;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _keybinding;

    private bool _isChanging;

    private void Start()
    {
        UpdateView();
    }

    private void Update()
    {
        if (_isChanging)
        {
            if (Input.anyKeyDown)
            {
                _isChanging = false;
                Keybindings.Set(_keyDescription, FetchKey());
                _text.color = Color.white;
                UpdateView();
            }
        }
    }

    private KeyCode FetchKey()
    {
        var e = System.Enum.GetNames(typeof(KeyCode)).Length;

        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }

    public void KeybindingClicked()
    {
        _text.color = Color.red;
        _isChanging = true;
    }

    private void UpdateView()
    {
        _text.text = _keyDescription.ToString();
        _keybinding.text = Keybindings.Get(_keyDescription).ToString();
    }
}
