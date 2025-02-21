using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Times2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(other.gameObject, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
    }
}
