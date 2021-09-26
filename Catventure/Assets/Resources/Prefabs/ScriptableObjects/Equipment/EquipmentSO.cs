using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipmentType { Head, Body, Arms, Legs, Weapon };

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]

public class EquipmentSO : ScriptableObject
{
    public Sprite equipmentSprite;
    public Sprite equipmentSpriteRightArmOrLeg;
    public int cookieCost;
    public EquipmentType equipmentType;
}
