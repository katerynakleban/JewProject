using UnityEngine;

public class RingsManagerScript : MonoBehaviour
{
    private int _currentRingIndex;
    private int _previousRingIndex;

    private void Start() {
        _currentRingIndex = 0;
        _previousRingIndex = 0;

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        foreach(var comp in rendererComponents) {
            comp.enabled = false;
        }

        rendererComponents[0].enabled = true;
        rendererComponents[1].enabled = true;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnNextButtonPressed() {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        _previousRingIndex = _currentRingIndex;
        if (_currentRingIndex == rendererComponents.Length - 1) {
            _currentRingIndex = 0;
        } else if(_currentRingIndex == 0) {
            _currentRingIndex = 2;
        } else {
            _currentRingIndex++;
        }

        ProceedRingIndexChange();
    }

    private void ProceedRingIndexChange() {

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        if(_currentRingIndex == 0) {
            rendererComponents[0].enabled = true;
            rendererComponents[1].enabled = true;
        } else {
            rendererComponents[_currentRingIndex].enabled = true;
        }

        if (_currentRingIndex != _previousRingIndex) {
            if (_previousRingIndex == 0) {
                rendererComponents[0].enabled = false;
                rendererComponents[1].enabled = false;
            } else {
                rendererComponents[_previousRingIndex].enabled = false;
            }
        }
    }

    public void OnPreviousButtonPressed() {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);

        _previousRingIndex = _currentRingIndex;
        if (_currentRingIndex == 0) {
            _currentRingIndex = rendererComponents.Length - 1;
        } else if (_currentRingIndex == 2) {
            _currentRingIndex = 0;
        } else { 
            _currentRingIndex--;
        }

        ProceedRingIndexChange();
    }
}
