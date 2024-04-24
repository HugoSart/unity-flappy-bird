using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GamePlayUIController : UserInterfaceController {

    [SerializeField]
    private GameObject scorePanel;

    [SerializeField]
    private List<Sprite> spriteDigits;
    
    // References
    private List<Image> spawnedImages;
    
    // State
    private int score;


    // =================================================================================================================
    // Unity Lifecycle
    // =================================================================================================================
    private void Start() {
        foreach (Transform child in scorePanel.transform) {
            Destroy(child.gameObject);
        }
        spawnedImages = new List<Image> { InstantiateDigitImage(0) };
    }


    // =================================================================================================================
    // Exposed
    // =================================================================================================================
    public void ChangeScore(int newScore) {
        if (score != newScore) {
            var newScoreStr = newScore.ToString();
            for (var i = 0; i < newScoreStr.Length; i++) {
                int c = newScoreStr[i];
                UpdateDigit(i, c - 48);
            }
            score = newScore;
        }
    }


    // =================================================================================================================
    // Utilities
    // =================================================================================================================
    private void UpdateDigit(int index, int value) {
        var count = spawnedImages.Count;
        
        // Validate index
        if (index > count || (index < 0 && Math.Abs(index) > count)) {
            throw new ArgumentException("Index " + index + " is invalid for digits count of " + count);
        }
        if (index < 0) index += count;
        
        // Validate value
        if (value is < 0 or >= 10) {
            throw new ArgumentException("Value " + value + " should be positive and contain only one digit.");
        }
        
        // Update or add new element if needed for number of digits
        if (index == count) {
            spawnedImages.Add(InstantiateDigitImage(value));
        } else {
            var img = spawnedImages[index];
            img.sprite = spriteDigits[value];
        }

    }

    private Image InstantiateDigitImage(int value) {
        
        // Validate value
        if (value is < 0 or >= 10) {
            throw new ArgumentException("Value " + value + " should be positive and contain only one digit.");
        }
        
        // Create image
        var obj = new GameObject("ScoreImage");
        var img = obj.AddComponent<Image>();
        img.sprite = spriteDigits[value];
        img.rectTransform.sizeDelta = new Vector2(14, 20);
        img.rectTransform.localScale = new Vector2(3, 3);
        img.transform.SetParent(scorePanel.transform);
        return img;

    }
    
}
