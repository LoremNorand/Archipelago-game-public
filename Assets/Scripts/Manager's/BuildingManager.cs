using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Магазины")]
    [SerializeField] internal Building[] shops;
    [Header("Дороги")]
    [SerializeField] internal Road[] roads; 
    internal int roadNum = 0;
}
