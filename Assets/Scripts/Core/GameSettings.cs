using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "TestGame/Game settings")]
public class GameSettings : ScriptableObject
{
    public float shipMaxSpeed;
    public float enemySpeed;
    public float enemyRotationSpeed;
    public float reloadTime;
    public float bulletSpeed;

    public int defaultHealth;
}
