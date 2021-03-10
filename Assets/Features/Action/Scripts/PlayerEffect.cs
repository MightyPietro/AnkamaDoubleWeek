using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEffectTypes { PA, PM, Vulnerability}

[CreateAssetMenu(menuName = "Assets/Player Effects")]
public class PlayerEffect : ScriptableObject
{
    public float value;
    public PlayerEffectTypes effectType;
}
