using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeScript : ScriptableObject
{
    // This script is an abstract class that can be used to define upgrades in a game.
    // It can be extended by other scripts to implement specific upgrade functionalities.
    // Define any common properties or methods for upgrades here.
    // For example, you might want to include properties like cost, description, or effects.
    public string upgradeName;
    public string description;
    public int cost;
    public Sprite icon;
    public UpgradeScript nextUpgrade; // Reference to the next upgrade in the chain, if applicable
    // Method to apply the upgrade effect
    public abstract void ApplyUpgrade();

    public void ShowNextUpgrade()
    {
        return;
    }
}