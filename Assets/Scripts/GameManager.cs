using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    [SerializeField]
    private Bird bird;

    [SerializeField]
    private GameObject pipeWallPrefab;

    [SerializeField]
    private GameObject foregroundLayer;
    
    [SerializeField]
    private float floorPlacingOffset;

    [SerializeField]
    private List<GameObject> floors;
    
    [SerializeField]
    private GameOverUIController gameOverUIController;

    [SerializeField]
    private GamePlayUIController gamePlayUIController;

    [SerializeField]
    private int targetFrameRate = 30;

    [SerializeField]
    private float timeToSpawnFirstObstacle = 0f;

    [SerializeField]
    private float timeToSpawnNewObstacles = 2f;
    
    // References
    private GameEvents _gameEvents;
    private GameObject _firstFloor;
    private GameObject _lastFloor;
    private SpriteRenderer _lastFloorSpriteRenderer;
    private CameraEffects _cameraEffects;
    private int _score;
    private bool isGameEnded;
    private float elapsedTimeSinceGameStart;
    private float elapsedTimeSinceLastObstableSpawn;
    private bool hasForegroundLayer;


    // =================================================================================================================
    // Unity Events
    // =================================================================================================================
    private void Start() {
        _gameEvents = GameEvents.main;
        _gameEvents.OnGameStart += OnGameStartHandler;
        _gameEvents.OnGameEnd += OnGameEndHandler;
        _gameEvents.OnScore += OnScoreHandler;
        if (Camera.main is not null) _cameraEffects = Camera.main.GetComponent<CameraEffects>();
        gameOverUIController.OnPlayButtonClick += RestartGame;
        isGameEnded = true;
        hasForegroundLayer = foregroundLayer is not null;
        
        _InitFloors();

        if (targetFrameRate > 0) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }

    }

    private void Update() {
        _UpdateFloors();
        TickPipeSpawn();
    }
    
    
    // =================================================================================================================
    // Handlers
    // =================================================================================================================
    private void OnGameStartHandler() {
        isGameEnded = false;
        elapsedTimeSinceGameStart = 0;
        elapsedTimeSinceLastObstableSpawn = 0;
        
        int bestScore = PlayerPrefs.GetInt("bestScore", -1);
        if (bestScore == -1) {
            PlayerPrefs.SetInt("bestScore", 0);
        }
        _ShowGameEndUI(false);
        _ShowGamePlayUI(true);
    }
    
    private void OnGameEndHandler() {
        if (isGameEnded) return;
        isGameEnded = true;
        
        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        if (_score > bestScore) {
            PlayerPrefs.SetInt("bestScore", _score);
        }

        if (_cameraEffects is not null) {
            var seconds = 0.2f;
            _cameraEffects.Blink(seconds);
            StartCoroutine(_ShowGameEndUIDelayed(true, seconds));
        } else {
            _ShowGameEndUI(true);
        }
        _ShowGamePlayUI(false);
    }

    private void OnScoreHandler() {
        _score++;
        gamePlayUIController.ChangeScore(_score);
    }


    // =================================================================================================================
    // Utilities
    // =================================================================================================================
    private void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TickPipeSpawn() {
        if (isGameEnded) return;
        elapsedTimeSinceGameStart += Time.deltaTime;
        elapsedTimeSinceLastObstableSpawn += Time.deltaTime;
        if (elapsedTimeSinceLastObstableSpawn >= timeToSpawnNewObstacles && elapsedTimeSinceGameStart >= 5) {
            _SpawnNewObstacle();
        }
    }
    
    private void _UpdateFloors() {
        if (floors.Count == 0) return;
        var firstPos = _firstFloor.transform.position;
        var distToBird = Mathf.Abs(firstPos.x - bird.transform.position.x);
        
        // Move last floor to first floor
        if (distToBird >= 3f) {
            Debug.Log("[GameManager] Updating floors");
            var lastPos = _lastFloor.transform.position;
            var lastSize = _lastFloorSpriteRenderer.bounds.size.x;
            _firstFloor.transform.position = new Vector2(lastPos.x + lastSize + floorPlacingOffset, lastPos.y);
            _InitFloors();
        }
        
    }

    private void _InitFloors() {
        if (floors.Count == 0) return;
        if (_firstFloor is null) {
            _firstFloor = floors[0];
            _lastFloor = floors[^1];
        } else {
            floors.Add(_firstFloor);
            floors.RemoveAt(0);
            _lastFloor = _firstFloor;
            _firstFloor = floors[0];
        }
        _lastFloorSpriteRenderer = _lastFloor.GetComponent<SpriteRenderer>();
    }
    
    private void _ShowGamePlayUI(bool visible) {
        if (gamePlayUIController is null) return;
        if (visible) {
            Debug.Log("[GameManager] Showing game play UI");
            gamePlayUIController.ChangeScore(_score);
            gamePlayUIController.Show();
        } else {
            gamePlayUIController.Hide();
        }
    }

    private void _ShowGameEndUI(bool visible) {
        if (gameOverUIController is null) return;
        if (visible) {
            Debug.Log("[GameManager] Showing game end UI");
            gameOverUIController.UpdateState(_score, PlayerPrefs.GetInt("bestScore", 0));
            gameOverUIController.Show();
        } else {
            gameOverUIController.Hide();
        }
    }

    private IEnumerator _ShowGameEndUIDelayed(bool visible, float seconds) {
        yield return new WaitForSeconds(seconds);
        _ShowGameEndUI(visible);
    }

    private void _SpawnNewObstacle() {
        var birdX = bird.transform.position.x;
        var pipeY = Random.Range(-0.2f, 0.95f);
        elapsedTimeSinceLastObstableSpawn = 0;
        var obj = Instantiate(pipeWallPrefab, new Vector3(birdX + 2, pipeY, 1), Quaternion.identity);
        var distanceDestroyer = obj.AddComponent<DistanceDestroyer>();
        distanceDestroyer.reference = bird.transform;
        distanceDestroyer.limit = 2f;
        if (hasForegroundLayer) {
            obj.transform.parent = foregroundLayer.transform;
        }
    }
    
}