using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _text;

    public override void Show()
    {
        base.Show();
        _text.text = GameManager.Instance.GetGreenPoint() == 10 ? "Red Win" : "Green Win";
        GameManager.Instance.pauseGame = true;
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.startPanel.Show();
    }
}