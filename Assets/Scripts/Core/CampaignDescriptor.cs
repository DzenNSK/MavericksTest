using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Campaign (levels set) descriptor
[CreateAssetMenu(fileName = "CampaignDescriptor", menuName = "TestGame/Campaign descriptor")]
public class CampaignDescriptor : ScriptableObject
{
    public LevelDescriptor[] levels;
}
