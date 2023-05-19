using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool isRedHome;
    [SerializeField] private bool isBlueHome;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Punk"))
        {
            GoalIn();
            col.gameObject.transform.position = Vector3.zero;
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void GoalIn()
    {
        if (isRedHome)
        {
            GameManager.Instance.RedScored();
        }
        else
        {
            GameManager.Instance.GreenScored();
        }
        
        if (GameManager.Instance.GetRedPoint() == 10 || GameManager.Instance.GetGreenPoint() == 10)
        {
            UIManager.Instance.endGameUI.Show();
        }
            
    }
}