using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float _x1;
    private float _x2;
    private float _destinationTolerance = 0.1f;
    private float _moveSpeed = 8f;
    
    public Transform _endX;
    private float _xDestination;

    public bool _move;
    
    public Slider _progressSlider;

    void Start()
    {
        _x1 = transform.position.x;
        _x2 = _x1 + _moveSpeed;
        _xDestination = _endX.position.x;
        _progressSlider.maxValue = _endX.transform.position.x;
    }

    void FixedUpdate()
    {
        if (_move && Mathf.Abs(_xDestination - _x1) > Mathf.Abs(_destinationTolerance))
        {
            transform.position = new Vector3(Mathf.Lerp(_x1, _x2, Time.deltaTime), transform.position.y, transform.position.z);
            _x1 = transform.position.x;
            _x2 = _x1 + _moveSpeed;

            UpdateProgressSlider();
        }
    }

    void UpdateProgressSlider()
    {
        _progressSlider.value = transform.position.x;
    }
}
