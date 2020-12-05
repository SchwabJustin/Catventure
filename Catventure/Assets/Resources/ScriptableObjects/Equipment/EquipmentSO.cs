using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipmentType { Head, Body, Arms, Legs };

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]

public class EquipmentSO : ScriptableObject
{
    public Sprite equipmentSprite;
    public int cookieCost;
    public EquipmentType equipmentType;
}
