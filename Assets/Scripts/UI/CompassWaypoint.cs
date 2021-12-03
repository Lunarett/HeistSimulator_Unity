using UnityEngine;
using UnityEngine.UI;

public class CompassWaypoint : MonoBehaviour
{
	[SerializeField] private Image _icon;

	[SerializeField] private float _distanceMin;
	[SerializeField] private float _distanceMax;
	[SerializeField] private float _worldSpaceToCompass;

	public void SetDefaultSprite(Sprite sprite)
	{
		_icon.sprite = sprite;
	}

	public void UpdateDistance(float distance)
	{
		_icon.rectTransform.anchoredPosition = new Vector3(0, Mathf.Clamp(distance * _worldSpaceToCompass, _distanceMin, _distanceMax), 0);
	}

	public void FixLocalRotation()
	{
		_icon.transform.rotation = Quaternion.identity;
	}
}
