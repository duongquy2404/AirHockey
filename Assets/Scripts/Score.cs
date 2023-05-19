using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Score : NetworkBehaviour
{
    [SerializeField] TextMeshPro scoreGreen;
    [SerializeField] TextMeshPro scoreRed;
    public NetworkVariable<int> greenPoint = new NetworkVariable<int>();
    public NetworkVariable<int> redPoint = new NetworkVariable<int>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreGreen.text = GameManager.Instance.GetGreenPoint().ToString();
        scoreRed.text = GameManager.Instance.GetRedPoint().ToString();
    }

    public void Update()
    {
        UpdateScore();
    }
}