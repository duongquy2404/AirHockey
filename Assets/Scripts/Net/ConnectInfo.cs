using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ConnectInfo")]
public class ConnectInfo : ScriptableObject
{
    [SerializeField] public string ipAddr;
    [SerializeField] public int port;
    
}
