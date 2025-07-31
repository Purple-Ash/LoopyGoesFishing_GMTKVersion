using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/FishData", order = 1)]
public class FishData : ScriptableObject
{
    [SerializeField]
    public Sprite image;

    [SerializeField]
    public string name;

    [SerializeField]
    public string description;

    [SerializeField]
    public float price;

    [SerializeField]
    public int numBad;

    [SerializeField]
    public int numNormal;

    [SerializeField]
    public int numGood;
}
