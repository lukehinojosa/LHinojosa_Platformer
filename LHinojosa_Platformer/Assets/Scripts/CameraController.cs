using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _x1;
    private float _x2;
    private float _destinationTolerance = 0.1f;
    private float _moveSpeed = 2f;
    
    public float _xDestination;

    public bool _move = true;

    void Start()
    {
        _x1 = transform.position.x;
        _x2 = _x1 + _moveSpeed;
    }

    void FixedUpdate()
    {
        if (_move && (_xDestination - _x1) > Mathf.Abs(_destinationTolerance))
        {
            transform.position = new Vector3(Mathf.Lerp(_x1, _x2, Time.deltaTime), transform.position.y, transform.position.z);
            _x1 = transform.position.x;
            _x2 = _x1 + _moveSpeed;
        }
    }
}
