using UnityEngine;

public class CameraEffects : MonoBehaviour {
    
    [SerializeField]
    private Camera camera;
    
    [SerializeField]
    private SpriteRenderer fadeSprite;
    
    // References
    private bool startFadeInNextFrame;
    private bool startFadeOutNextFrame;
    private bool isFadingIn;
    private bool isFadingOut;
    private Color fadeInitialColor;
    private float fadeFraction;
    private float fadeDuration = 1f;
    private bool isBlinking;
    

    // =================================================================================================================
    // Unity Events
    // =================================================================================================================
    private void Start() {
        if (camera is null) camera = Camera.main;
        if (fadeSprite is not null) {
            fadeInitialColor = fadeSprite.color;
            fadeSprite.gameObject.transform.localScale = new Vector3(999, 999, 999);
        }
    }
    
    private void Update() {
        UpdateFadeInEffect();
    }


    // =================================================================================================================
    // Fade Effect
    // =================================================================================================================
    public void FadeIn(float seconds = 0.2f) {
        if (isFadingOut) {
            isFadingOut = false;
            startFadeOutNextFrame = false;
        }

        fadeDuration = seconds;
        startFadeInNextFrame = true;
    }
    
    public void FadeOut(float seconds = 0.2f) {
        if (isFadingIn) {
            isFadingIn = false;
            startFadeInNextFrame = false;
        }

        fadeDuration = seconds;
        startFadeOutNextFrame = true;
    }
    
    public void Blink(float seconds = 0.2f) {
        fadeDuration = seconds / 2f;
        isBlinking = true;
        FadeIn(seconds);
    }
    
    private void UpdateFadeInEffect() {
        if (fadeSprite is null) return;
        
        // Init fade in or out
        if (startFadeInNextFrame || startFadeOutNextFrame) {
            isFadingIn = startFadeInNextFrame;
            isFadingOut = startFadeOutNextFrame;
            startFadeInNextFrame = false;
            startFadeOutNextFrame = false;
            fadeFraction = 0f;
        }
        
        // Update fade
        if (isFadingIn || isFadingOut) {
            fadeFraction += Time.deltaTime / fadeDuration;
            var a = isFadingIn
                ? Mathf.Lerp(fadeInitialColor.a, 1, fadeFraction)
                : Mathf.Lerp(1, fadeInitialColor.a, fadeFraction); 
            var color = fadeSprite.color;
            fadeSprite.color = new Color(color.r, color.g, color.b, a);
            if (fadeFraction >= 1) {
                if (isFadingIn && isBlinking) FadeOut(fadeDuration);
                else if (isFadingOut && isBlinking) isBlinking = false;
                isFadingIn = isFadingOut = false;
            }
        }
        
    }
    
}