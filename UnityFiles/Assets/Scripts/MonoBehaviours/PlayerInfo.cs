using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int wood;

    [SerializeField]
    private int fruit;

    [SerializeField]
    private Seed[] seeds;
}


[System.Serializable]
public struct Seed
{
    public string name;
    public int amount;
}