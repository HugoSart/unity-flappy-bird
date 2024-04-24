using System;
using UnityEngine;

public class GameEvents : MonoBehaviour {
    
    public static GameEvents main => GameObject.FindWithTag("MainGameEvents").GetComponent<GameEvents>();

    // Events
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action OnScore;

    // =================================================================================================================
    // Triggers
    // =================================================================================================================
    public void TriggerOnGameStart() {
        Debug.Log("[GameEvents] Triggered OnGameStart");
        OnGameStart?.Invoke();
    }
    
    public void TriggerOnGameEnd() {
        Debug.Log("[GameEvents] Triggered OnGameEnd");
        OnGameEnd?.Invoke();
    }
    
    public void TriggerOnScore() {
        Debug.Log("[GameEvents] Triggered OnScore");
        OnScore?.Invoke();
    }
    
}
