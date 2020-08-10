using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Level parameters descriptor
[CreateAssetMenu(fileName = "LevelDescriptor", menuName = "TestGame/Level descriptor")]
public class LevelDescriptor : ScriptableObject
{
    //Types of win conditions
    public enum WinConditions { KillCount, Survive }

    public string levelName;

    //Type of win condition and parameter - killcount or survive time
    public WinConditions winCondition;
    public int winParameter;

    //Rate of enemy spawn
    public float spawnRate;
    public float spawnInterval;
}
