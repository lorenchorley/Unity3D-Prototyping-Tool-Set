
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Inputter {

    public static Inputter GetPlatformAppropriateInputter() {
#if UNITY_EDITOR
        return new MouseInputter();
#elif UNITY_ANDROID
        return new TouchInputter();
#elif UNITY_STANDALONE_WIN
        return new MouseInputter();
#else
        return new MouseInputter();
#endif
    }

    public class PositionEvent : StrictSignal<Vector2> { }
    public class NormalisedZoomDeltaEvent : StrictSignal<float> { }

    public PositionEvent OnPrimaryAction, OnSecondaryAction; // TODO
    public PositionEvent OnStartDrag, OnSecondaryStartDrag;
    public PositionEvent OnDrag, OnSecondaryDrag;
    public PositionEvent OnEndDrag, OnSecondaryEndDrag;
    public NormalisedZoomDeltaEvent OnZoom;

    protected abstract bool CanStartClickOrTap();
    protected abstract bool CanEndClickOrTap();
    protected abstract bool CanTriggerPrimaryAction();

    protected abstract bool CanStartDrag();
    protected abstract bool CanEndDrag();
    protected abstract bool CanZoomAndDragSimultaneously();

    protected abstract bool CanStartZoom();
    protected abstract bool CanEndZoom();
    protected abstract float GetZoomDelta();
    protected abstract void RegisterStartOfZoom();
    protected abstract bool CanMaintainZoomWithoutZooming();

    protected abstract Vector2 GetScreenPosition();

    public float NormalisedZoomSpeed = 0.01f;
    public bool DisableOverUI = true;

    [Space]

    // Internal values
    protected float DragThresholdInPixels;

    // Drag and click related
    protected bool IsAttemptingDrag = false;
    protected bool IsDragging = false;
    protected Vector2 DragStartPosition, LastDragPosition;
    //public bool IsBlockingClickTapDragActions = false;

    // Zoom related
    protected bool IsZooming = false;
    protected bool IsZoomingPaused = false;

    // TODO Remove
    public string type;

    // UI Related
    private List<RaycastResult> results;

    public Inputter() {
        OnPrimaryAction = new PositionEvent();
        OnStartDrag = new PositionEvent();
        OnDrag = new PositionEvent();
        OnEndDrag = new PositionEvent();
        OnZoom = new NormalisedZoomDeltaEvent();

        results = new List<RaycastResult>();

        RecalculateGlobalDragThreshold();
    }

    public bool IsPositionOverUI(Vector2 position) {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }

    public List<RaycastResult> WhatUIIsPositionOver(Vector2 position) {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }

    public void RecalculateGlobalDragThreshold() {
        EventSystem.current.pixelDragThreshold = Mathf.Max(
                                                     EventSystem.current.pixelDragThreshold,
                                                     (int) (EventSystem.current.pixelDragThreshold * Screen.dpi / 160f)
                                                 );

        // Cache locally
        DragThresholdInPixels = EventSystem.current.pixelDragThreshold;
    }

    public bool Update() {

        if (Input.GetMouseButtonDown(0))
            Assert.IsTrue(Input.GetMouseButtonDown(0));

        if (TryEndZoom())
            return true;

        if (TryZoom())
            return true;

        if (TryStartZoom())
            return true;

        if (TryEndDragOrDetectClickOrTap())
            return true;

        if (TryDrag())
            return true;

        if (TryStartClickTapOrDrag())
            return true;

        return false;
    }

    private bool TryEndZoom() {

        if (IsZooming) {

            if (CanMaintainZoomWithoutZooming()) {

                IsZoomingPaused = true;
                IsZooming = false;

                DebugLog("Paused zooming");

                return true;
            } else if (CanEndZoom()) {

                IsZooming = false;

                DebugLog(type + "End zoom");

                return !CanZoomAndDragSimultaneously(); // Allow drag events while zooming if required
            }

        } else if (IsZoomingPaused) {

            if (CanStartZoom()) {
                DebugLog("Unpaused zooming");
                IsZoomingPaused = false;
                IsZooming = true;

                RegisterStartOfZoom();

                return false;
            } else if (CanEndZoom()) {

                IsZoomingPaused = false;

                DebugLog(type + "End zoom");

                return !CanZoomAndDragSimultaneously(); // Allow drag events while zooming if required
            }

            return true;
        }

        return false;
    }

    private bool TryZoom() {

        if (IsZooming) {

            OnZoom.Dispatch(GetZoomDelta());

            DebugLog(type + "Zooming");

            return !CanZoomAndDragSimultaneously(); // Allow drag events while zooming if required
        }

        return false;
    }

    private bool TryStartZoom() {

        if (CanStartZoom()) {

            IsZooming = true;

            RegisterStartOfZoom();

            OnZoom.Dispatch(GetZoomDelta());

            DebugLog(type + "Start zoom");

            // End any drag currently in progress, if necessary
            if (!CanZoomAndDragSimultaneously()) {

                // Do not register click or tap, just stop
                IsDragging = false;
                IsAttemptingDrag = false;

                return false; // So that drag events may happen as well
            } else {
                return true; // Block any further events
            }

        }

        return false;
    }


    private bool TryEndDragOrDetectClickOrTap() {

        if (CanEndDrag()) {
            if (IsDragging) {

                IsDragging = false;

                OnEndDrag.Dispatch(LastDragPosition);

                DebugLog(type + "End drag");

                return true;
            } else if (IsAttemptingDrag) {

                IsAttemptingDrag = false;

                if (CanTriggerPrimaryAction()) {
                    OnPrimaryAction.Dispatch(LastDragPosition);
                }

                DebugLog(type + "End drag with click or tap");

                return true;
            }
        }

        return false;
    }

    private bool TryDrag() {

        if (IsDragging) {

            Vector2 currentPosition = GetScreenPosition();

            // Check if mouse or touch has moved since last frame
            if (currentPosition != LastDragPosition) {

                OnDrag.Dispatch(GetScreenPosition());
                LastDragPosition = currentPosition;

                DebugLog(type + "Dragging");

            }

            // Want to block if dragging, whether a displacement was made or not
            return true;
        }

        return false;
    }

    private bool TryStartClickTapOrDrag() {
        Assert.IsFalse(IsDragging); // IsDragging here is irrelevant based on Update ordering

        if (IsAttemptingDrag) {

            LastDragPosition = GetScreenPosition();
            float distanceFromStart = (LastDragPosition - DragStartPosition).magnitude;

            if (distanceFromStart >= DragThresholdInPixels) {

                IsDragging = true;
                IsAttemptingDrag = false;

                OnStartDrag.Dispatch(LastDragPosition);

                DebugLog(type + "Started drag");

                return true;
            }

        } else if (CanStartDrag()) {

            Vector2 screenPosition = GetScreenPosition();

            if (DisableOverUI && IsPositionOverUI(screenPosition)) {
                return true;
            }

            IsAttemptingDrag = true;
            DragStartPosition = screenPosition;

            DebugLog(type + "Attempting to start drag");

            return true;
        }

        return false;
    }

    private void DebugLog(string text) {
        //Debug.Log(text);
    }

}

