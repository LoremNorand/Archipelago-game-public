using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] internal Building[] shops;
    [Header("������")]
    [SerializeField] internal Road[] roads; 
    internal int roadNum = 0;
}
