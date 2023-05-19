using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ConfigureConnectionBehavior : MonoBehaviour
{
    public TextMeshProUGUI localIpInfoText;
    public TextMeshProUGUI localIpInfoText2;
    public TMP_InputField ipInputField;
    public TMP_InputField portInputField;
    public Button hostButton;
    public Button clientButton;

    public ServerBehaviour serverManager;
    public ClientBehaviour clientManager;

   [SerializeField] private ConnectInfo connectInfo;

     private string localIPAddr;

    void Awake()
    {
        localIPAddr = NetworkUtility.GetLocalIP();
        this.localIpInfoText.text = "Your IP: " + localIPAddr;
        this.localIpInfoText2.text = "Your IP: " + localIPAddr;
        ApplyConnectInfoToUI();
        this.hostButton.onClick.AddListener(OnClickHost);
        this.clientButton.onClick.AddListener(OnClickClient);
    }

    // 
    private void Start()
    {
#if UNITY_SERVER
            Debug.Log("Server Build.");
            ApplyConnectInfoToNetworkManager(true);
            this.serverManager.Setup(this.connectInfo);
            NetworkUtility.RemoveUpdateSystemForHeadlessServer();
            var tasks = Unity.Netcode.NetworkManager.Singleton.StartServer();
#elif ENABLE_AUTO_CLIENT
            if (NetworkUtility.IsBatchModeRun)
            {
                NetworkUtility.RemoveUpdateSystemForBatchBuild();
                OnClickClient();
            }
#endif
    }

    private void OnClickHost()
    {
        GenerateConnectInfoValueFromUI();
        ApplyConnectInfoToNetworkManager(true);

        if (Unity.Netcode.NetworkManager.Singleton.IsClient)
        {
            Unity.Netcode.NetworkManager.Singleton.Shutdown();
        }

        this.serverManager.Setup(this.connectInfo);
        var result = Unity.Netcode.NetworkManager.Singleton.StartHost();
    }

    private void OnClickClient()
    {
        GenerateConnectInfoValueFromUI();
        ApplyConnectInfoToNetworkManager(false);
        this.clientManager.Setup();
        var result = Unity.Netcode.NetworkManager.Singleton.StartClient();
    }

    public void OnClickReset()
    {
      //  this.connectInfo = ConnectInfo.GetDefault();
        ApplyConnectInfoToUI();
    }

    private void ApplyConnectInfoToUI()
    {
        this.ipInputField.text = this.connectInfo.ipAddr;
        this.portInputField.text = this.connectInfo.port.ToString();
    }

    private void GenerateConnectInfoValueFromUI()
    {
        this.connectInfo.ipAddr = this.ipInputField.text;
        int.TryParse(this.portInputField.text, out this.connectInfo.port);
    }


    private void ApplyConnectInfoToNetworkManager(bool isServer)
    {
        var transport = Unity.Netcode.NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        var unityTransport = transport as UnityTransport;
        if (unityTransport != null)
        {
            if (isServer)
            {
                unityTransport.SetConnectionData(IPAddress.Any.ToString(),
                    (ushort)this.connectInfo.port);
            }
            else
            {
                unityTransport.SetConnectionData(this.connectInfo.ipAddr.Trim(),
                    (ushort)this.connectInfo.port);
            }
        }
    }
}