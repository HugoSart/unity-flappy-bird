using System;
using TMPro;
using UnityEngine;

public class GameOverUIController : UserInterfaceController {

    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    
    // Events
    public event Action OnPlayButtonClick;


    // =================================================================================================================
    // State Changes
    // =================================================================================================================
    public void UpdateState(int score, int bestScore) {
        scoreText.text = score.ToString();
        bestScoreText.text = bestScore.ToString();
    }


    // =================================================================================================================
    // UI Events
    // =================================================================================================================
    public void OnPlayButtonClickHandler() {
        OnPlayButtonClick?.Invoke();
    }
    
}
