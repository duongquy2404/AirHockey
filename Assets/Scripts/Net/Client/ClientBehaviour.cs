using TMPro;
using UnityEngine;
using Unity.Networking.Transport;
using UnityEngine.UI;

public class ClientBehaviour : MonoBehaviour
{
    public Button stopButton;
    public GameObject configureObject;
    private bool previewConnected;


    public void Setup()
    {
        Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void ReoveCallbacks()
    {
        Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }

    private void Disconnect()
    {
#if ENABLE_AUTO_CLIENT
            if (NetworkUtility.IsBatchModeRun)
            {
                Application.Quit();
            }
#endif
        configureObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        stopButton.onClick.RemoveAllListeners();
        
        ReoveCallbacks();
    }

    private void OnClickStopButton()
    {
        Unity.Netcode.NetworkManager.Singleton.Shutdown();
        Disconnect();
    }

    private void OnClientConnect(ulong clientId)
    {
        Debug.Log("Connect Client:" + clientId + "::" + Unity.Netcode.NetworkManager.Singleton.LocalClientId);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log("Disconnect Client: " + clientId);
    }
    
    private void OnConnectSelf()
    {
        configureObject.SetActive(false);
        GameManager.Instance.pauseGame = false;
        stopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Disconnect";
        stopButton.onClick.AddListener(this.OnClickStopButton);
        stopButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        var netMgr = Unity.Netcode.NetworkManager.Singleton;
        var currentConnected = netMgr.IsConnectedClient;
        if (currentConnected != previewConnected)
        {
            if (!currentConnected)
            {
                Disconnect();
            }
            else
            {
                OnConnectSelf();
            }
        }

        previewConnected = netMgr.IsConnectedClient;
    }
}