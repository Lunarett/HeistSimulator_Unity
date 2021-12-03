using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private MeshRenderer _renderer;

    [SerializeField] private float _maxOutlineWidth;
    [SerializeField] private Color _outlineColor;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ShowOutline()
    {
        _renderer.material.SetFloat("_Outline", _maxOutlineWidth);
        _renderer.material.SetColor("_OutlineColor", _outlineColor);
    }

    public void HideOutline()
    {
        _renderer.material.SetFloat("_Outline", 0f);
    }
}
