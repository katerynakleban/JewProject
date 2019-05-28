/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    private int _currentRingIndex;
    private int _previousRingIndex;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        _currentRingIndex = 0;
        _previousRingIndex = 0;
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    private void ProceedTrackableStateChange() {
        if (m_NewStatus == TrackableBehaviour.Status.DETECTED ||
            m_NewStatus == TrackableBehaviour.Status.TRACKED ||
            m_NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        } else if (m_PreviousStatus == TrackableBehaviour.Status.TRACKED &&
                   m_NewStatus == TrackableBehaviour.Status.NO_POSE) {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        } else {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;
        ProceedTrackableStateChange();
    }

    public void OnNextButtonPressed() {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);

        _previousRingIndex = _currentRingIndex;
        if (_currentRingIndex == rendererComponents.Length - 1) {
            _currentRingIndex = 0;
        } else if (_currentRingIndex == 0) {
            _currentRingIndex = 2;
        } else {
            _currentRingIndex++;
        }

        ProceedTrackableStateChange();
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

        ProceedTrackableStateChange();
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        if (_currentRingIndex == 0) {
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

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}
