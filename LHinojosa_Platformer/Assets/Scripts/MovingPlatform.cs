using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float _moveSpeed = 5f;
    private bool _isVisible;

    void Update()
    {
        if (_isVisible)
            transform.Translate(Vector2.left * (_moveSpeed * Time.deltaTime));
    }

    private void OnBecameVisible()
    {
        _isVisible = true;
    }

    private void OnBecameInvisible()
    {
        _isVisible = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>()._addVelocity = -_moveSpeed;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.activeInHierarchy && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>()._addVelocity = 0f;
        }
    }
}
