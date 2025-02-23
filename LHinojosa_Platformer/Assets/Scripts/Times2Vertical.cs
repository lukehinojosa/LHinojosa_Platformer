using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Times2Vertical : MonoBehaviour
{
    private bool _triggered;
    private Vector3 _duplicateOffset;

    void Start()
    {
        _duplicateOffset = new Vector3(0f, 1f, 0f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.GetGravityUp())
                Instantiate(other.gameObject, other.transform.position - _duplicateOffset, other.transform.rotation);
            else
                Instantiate(other.gameObject, other.transform.position + _duplicateOffset, other.transform.rotation);
            
            Destroy(gameObject);
        }
    }
}
