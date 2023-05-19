using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GoRoomIP : BaseUI
{

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string st; 
    public void GoBT()
    {

        st = "";
        st = inputField.text;
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        
        var unityTranport = transport as UnityTransport;
        if (unityTranport != null)
        {
            unityTranport.SetConnectionData( st.Trim() , (ushort)7777);
        }
        
        GameManager.Instance.pauseGame = false;
        NetworkManager.Singleton.StartClient();
        UIManager.Instance.startPanel.Hide();
        Hide();
        
    }
    
    
}
