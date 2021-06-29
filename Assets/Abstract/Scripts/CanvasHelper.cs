using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
[RequireComponent(typeof(Canvas))]
public class CanvasHelper : MonoBehaviour
{
    private static List<CanvasHelper> _helpers = new List<CanvasHelper>();

    private static UnityEvent _onResolutionOrOrientationChanged;
 
    private static bool _screenChangeVarsInitialized = false;
    private static ScreenOrientation _lastOrientation = ScreenOrientation.Landscape;
    private static Vector2 _lastResolution = Vector2.zero;
    private static Rect _lastSafeArea = Rect.zero;
 
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private RectTransform _safeAreaTransform;

    void Start()
    {
        _onResolutionOrOrientationChanged = new UnityEvent();
    }

    void Awake()
    {
        if(!_helpers.Contains(this))
            _helpers.Add(this);
   
        _canvas = GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
   
        _safeAreaTransform = transform.Find("SafeArea") as RectTransform;
   
        if(!_screenChangeVarsInitialized)
        {
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;
   
            _screenChangeVarsInitialized = true;
        }
       
        ApplySafeArea();
    }
 
    void Update()
    {
        if(_helpers[0] != this)
            return;
   
        if(Application.isMobilePlatform && Screen.orientation != _lastOrientation)
            OrientationChanged();
   
        if(Screen.safeArea != _lastSafeArea)
            SafeAreaChanged();
   
        if(Math.Abs(Screen.width - _lastResolution.x) > 0.1 || Math.Abs(Screen.height - _lastResolution.y) > 0.1)
            ResolutionChanged();
    }
 
    void ApplySafeArea()
    {
        if(_safeAreaTransform == null)
            return;
   
        var safeArea = Screen.safeArea;
   
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        var pixelRect = _canvas.pixelRect;
        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;
        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;
   
        _safeAreaTransform.anchorMin = anchorMin;
        _safeAreaTransform.anchorMax = anchorMax;
    }
 
    void OnDestroy()
    {
        if(_helpers != null && _helpers.Contains(this))
            _helpers.Remove(this);
    }
 
    private static void OrientationChanged()
    {
        Debug.Log("Orientation changed from " + _lastOrientation + " to " + Screen.orientation + " at " + Time.time);
   
        _lastOrientation = Screen.orientation;
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
 
        _onResolutionOrOrientationChanged.Invoke();
    }
 
    private static void ResolutionChanged()
    {
        Debug.Log("Resolution changed from " + _lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);
   
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
 
        _onResolutionOrOrientationChanged.Invoke();
    }
 
    private static void SafeAreaChanged()
    {
        Debug.Log("Safe Area changed from " + _lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);
   
        _lastSafeArea = Screen.safeArea;

        foreach (var t in _helpers)
        {
            t.ApplySafeArea();
        }
    }
}