using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject для хранения основных характеристик предметов
/// </summary>
[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data", order = 51)]
public class ItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemType _type;
    [SerializeField] private float _weight;

    public int Id { get => _id; }
    public string Name { get => _name; }
    public Sprite Icon { get => _icon; }
    public ItemType Type { get => _type; }
    public float Weight { get => _weight; }
}
