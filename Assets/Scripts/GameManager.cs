using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    public Punk punk;
    public bool pauseGame;
    public Score score;
    public bool isHost; 
    
    public Paddle paddleHost; 
    public Paddle paddleClient;
    
    public UnityTransport transport;

    public GameObject score1;
    public GameObject score2;
    
    void Start()
    {
        Application.targetFrameRate = 60;
        score.redPoint.Value = 0;
        score.greenPoint.Value = 0;
        pauseGame = true; 
      //  punk.gameObject.SetActive(false);
      //dfasfasf
    }

    public void SetPoint(bool GhiBan) //True: Red ghi ban; False: Green ghi ban
    {
        if (GhiBan)
        {
            score.redPoint.Value++; //Cong diem cho red
        }
        else
        {
            score.greenPoint.Value++; //Cong diem cho green
        }
    }

    public void RedScored()
    {
        score.redPoint.Value++; 
        score.UpdateScore();
    }
    
    public void GreenScored()
    {
        score.greenPoint.Value++;
        score.UpdateScore();
    }

    public int GetRedPoint()
    {
        return score.redPoint.Value; //Tra ve diem ben do
    }

    public int GetGreenPoint()
    {
        return score.greenPoint.Value; //Tra ve diem ben xanh
    }

    public void ResetPunk()
    {
      //  punk.transform.position = Vector3.zero;
    }
    
    

    
}

