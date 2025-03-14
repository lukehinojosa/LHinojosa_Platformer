using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Times2 : MonoBehaviour
{
    private bool _triggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            
            Instantiate(other.gameObject, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
    }
}
