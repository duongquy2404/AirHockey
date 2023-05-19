using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using Unity.Netcode.Components;
using UnityEngine;


[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    /// <summary>
    /// Used to determine who can write to this transform. Owner client only.
    /// This imposes state to the server. This is putting trust on your clients. Make sure no security-sensitive features use this transform.
    /// </summary>
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update()
    {
        base.Update();
        if (Unity.Netcode.NetworkManager.Singleton != null &&
            (Unity.Netcode.NetworkManager.Singleton.IsConnectedClient ||
             Unity.Netcode.NetworkManager.Singleton.IsListening))
        {
            if (CanCommitToTransform)
            {
                TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
            }
        }
    }
}