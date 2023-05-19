using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ServerBehaviour : MonoBehaviour
{
    public Button stopButton;
    public GameObject configureObject;

    public GameObject serverInfoRoot;
    //  public Text serverInfoText;

    [SerializeField] private ConnectInfo cachedConnectInfo;


    public void Setup(ConnectInfo connectInfo)
    {
        this.cachedConnectInfo = connectInfo;
        Unity.Netcode.NetworkManager.Singleton.OnServerStarted += this.OnStartServer;
        Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnect;
        Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback += this.OnClientDisconnect;
    }


    private void RemoveCallBack()
    {
        Unity.Netcode.NetworkManager.Singleton.OnServerStarted -= this.OnStartServer;
        Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnect;
        Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback -= this.OnClientDisconnect;
    }

    private void OnClientConnect(ulong clientId)
    {
        Debug.Log("Connect Client " + clientId);


        if (clientId != 0)
        {
            GameManager.Instance.pauseGame = false;
            UIManager.Instance.waitingComponentUI.gameObject.SetActive(false);
        }
        
        if (GameManager.Instance.punk is null)
            SpawnNetworkPrefab(this.networkedPrefab);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log("Disconnect Client " + clientId);
    }

    private void OnStartServer()
    {
        Debug.Log("Start Server");
        var clientId = Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        configureObject.SetActive(false);
        stopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Host";
        stopButton.onClick.AddListener(OnClickDisconnectButton);
        stopButton.gameObject.SetActive(true);
        UIManager.Instance.waitingComponentUI.gameObject.SetActive(true);
    }

    private void OnClickDisconnectButton()
    {
        Unity.Netcode.NetworkManager.Singleton.Shutdown();
        this.RemoveCallBack();

        if (GameManager.Instance.punk is not null)
        {
            Destroy(GameManager.Instance.punk.gameObject);
        }
        
        
        UIManager.Instance.waitingComponentUI.gameObject.SetActive(false);
        GameManager.Instance.punk.gameObject.SetActive(false);
       // GameManager.Instance.punk.SetEnableThisClientRpc(false, Vector2.zero);
        this.configureObject.SetActive(true);
        this.stopButton.gameObject.SetActive(false);
    }

    [SerializeField] private GameObject networkedPrefab;

    private void SpawnNetworkPrefab(GameObject prefab)
    {
        Debug.Log("SpawnNetworkPrefab");
        var randomPosition = new Vector3(0, 1.0f, 0);
        var gmo = GameObject.Instantiate(prefab, randomPosition, Quaternion.identity);
        GameManager.Instance.punk = gmo.GetComponent<Punk>();
        gmo.GetComponent<NetworkObject>().Spawn(true);
        GameManager.Instance.pauseGame = false;
       // netObject.SpawnWithOwnership(clientId);
    }
}