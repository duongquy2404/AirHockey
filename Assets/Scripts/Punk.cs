using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class Punk : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float constantSpeed;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("GoalRed"))
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (col.gameObject.CompareTag("GoalGreen"))
        {
            gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsHost)
            {
                RpcChangePosClientRpc();
            }
            
        }
        
    }

    
    [ClientRpc]
    public void RpcChangePosClientRpc()
    {
        var p = rb.transform.position;
        Debug.Log(p);
        this.rb.position = p;

    }
}