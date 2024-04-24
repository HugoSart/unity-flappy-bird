using UnityEngine;

public class ScoreTrigger : MonoBehaviour {
    
    // References
    private GameEvents _gameEvents;


    // =================================================================================================================
    // Unity Events
    // =================================================================================================================
    private void Start() {
        _gameEvents = GameEvents.main;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Bird>() is not null) {
            _gameEvents.TriggerOnScore();
        }
    }
    
}
