using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private BackPackController _playerBackPack;


    private void Awake()
    {
        Instance = this;
    }

    public void OnItemDropToBackPack(PhysicalItem item)
    {
        _playerBackPack.TryEquipItem(item);
    }

    public void OnItemThrowFromBackPack()
    {

    }
}
