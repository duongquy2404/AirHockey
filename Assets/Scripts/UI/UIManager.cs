using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GoRoomIP roomIP;
    public WaitingComponentUI waitingComponentUI;
    public StartPanel startPanel;
    public EndGameUI endGameUI;
}