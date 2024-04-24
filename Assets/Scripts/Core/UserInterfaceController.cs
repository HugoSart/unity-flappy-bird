using System;
using UnityEngine;

public abstract class UserInterfaceController : MonoBehaviour {


    // =================================================================================================================
    // State Changes
    // =================================================================================================================
    public void Toggle(bool? visible = null) {
        gameObject.SetActive(visible ?? !gameObject.activeSelf);
    }

    public void Show() {
        Toggle(true);
    }

    public void Hide() {
        Toggle(false);
    }

}