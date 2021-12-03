using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanRearDoorBehaviour : DoorBehaviour
{
    private void Awake()
    {
        if (_targetAngle == _openAngle)
        {
            _isOpen = true;
        }

        else if (_targetAngle == _closeAngle)
        {
            _isOpen = false;
        }
        _currentAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_targetAngle != _currentAngle)
        {
            float rotationAmount = Mathf.Min(_rotationSpeed * Time.deltaTime, Mathf.Abs(_targetAngle - _currentAngle));
            _currentAngle += Mathf.Sign(_targetAngle - _currentAngle) * rotationAmount;

            transform.localRotation = Quaternion.Euler(0, _currentAngle, 0);
        }
    }
}
