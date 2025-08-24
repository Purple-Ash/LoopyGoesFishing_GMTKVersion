using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/NewFishData", order = 1)]
public class NewFishData : ScriptableObject
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
    public float weight;

    [SerializeField]
    public int infoPrice;

    [SerializeField]
    public string infoPlace;

    [SerializeField]
    public string infoDetails;
}