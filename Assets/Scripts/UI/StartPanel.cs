using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class StartPanel : BaseUI
{
    public static StartPanel Instance;
    
    public void ClientButton()
    {
        UIManager.Instance.roomIP.Show();
    }

    public void HostButton()
    {
        
        
        
        
        UIManager.Instance.waitingComponentUI.Show();
        
        GameManager.Instance.isHost = true;
    }
    
    
}