using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Times1 : MonoBehaviour
{
    private bool _triggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
                if (players[i] != other.gameObject)
                    Destroy(players[i]);
            
            Destroy(gameObject);
        }
    }
}
