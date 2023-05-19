using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Paddle : NetworkBehaviour
{
    private bool wasJustClicked = true;
    private bool canMove;

    private Vector2 boundRed = new Vector2(-4.85f, -0.42f);
    private Vector2 boundBlue = new Vector2(0.42f, 4.85f);


    [SerializeField] private Vector2 boundX;
    [SerializeField] private Vector2 boundY;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [SerializeField] private bool isOwnerRF;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (NetworkManager.Singleton.IsHost)
        {
            spriteRenderer.color = IsOwner ? Color.red : Color.green;
            if (IsOwner)
            {
                GameManager.Instance.paddleHost = this; 
            }
            else
            {
                GameManager.Instance.paddleClient = this;
            }
        }
        else if (IsClient)
        {
            spriteRenderer.color = IsOwner ? Color.green : Color.red;
            Camera.main.transform.rotation = Quaternion.Euler(180, 180, 0);
            GameManager.Instance.score1.transform.rotation = Quaternion.Euler(180, 180, 0);
            GameManager.Instance.score2.transform.rotation = Quaternion.Euler(180, 180, 0);
        }


        isOwnerRF = IsOwner;

        boundY = NetworkManager.Singleton.IsHost ? boundRed : boundBlue;
        // rb.position = GameManager.Instance.isHost ? new Vector2(0, -1) : new Vector2(0, 1);

        rb.position = new Vector2(0, (NetworkManager.Singleton.IsHost && IsOwner) ? -3 : 3);
        UpdateClientPositionServerRpc(rb.position);
    }


    private void Update()
    {
        if (GameManager.Instance.pauseGame)
            return;

        if (IsOwner)
        {
            OnOwnerControl();
        }
    }


    private void OnOwnerControl()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!(mousePos.x < boundX.x || mousePos.x > boundX.y || mousePos.y < boundY.x || mousePos.y > boundY.y))
        {
            if (Input.GetMouseButton(0))
            {
                rb.MovePosition(mousePos);
            }
        }
        else
        {
            if (!Input.GetMouseButton(0))
                return;
            if ((mousePos.x < boundX.x && mousePos.y < boundY.x) || (mousePos.x < boundX.x && mousePos.y > boundY.y)
                                                                 || (mousePos.x > boundX.y &&
                                                                     mousePos.y < boundY.x) ||
                                                                 (mousePos.x > boundX.y && mousePos.y > boundY.y))
            {
                return;
            }

            if (mousePos.x < boundX.x || mousePos.x > boundX.y)
            {
                var vector2 = rb.position;
                vector2.y = mousePos.y;
                rb.MovePosition(vector2);
            }

            if (mousePos.y < boundY.x || mousePos.y > boundY.y)
            {
                var vector2 = rb.position;
                vector2.x = mousePos.x;
                rb.MovePosition(vector2);
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void UpdateClientPositionServerRpc(Vector2 pk)
    {
        if (GameManager.Instance.paddleClient is null)
            return;
        Debug.Log("Update Client Pos");
        GameManager.Instance.paddleClient.rb.MovePosition(pk);
    }
}