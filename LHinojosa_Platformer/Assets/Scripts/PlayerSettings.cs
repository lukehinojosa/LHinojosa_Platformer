using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Platformer/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public float _moveSpeed = 20f;
    public float _jumpSpeed = 10f;
}
