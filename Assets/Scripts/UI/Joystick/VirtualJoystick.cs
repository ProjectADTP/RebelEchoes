using UnityEngine;
using UnityEngine.UI;
using System;

public class VirtualJoystick : MonoBehaviour
{
    [SerializeField] private Image joystickBackground;
    [SerializeField] private Image joystickHandle;

    private Vector2 inputVector;
    private RectTransform backgroundRect;
    private bool isDragging;
    private Canvas canvas;
    private RectTransform rectTransform;
    private Camera _camera;

    public event Action OnJoystickReleased;

    private void Awake()
    {
        _camera = Camera.main;

        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        
        if (joystickBackground != null)
            backgroundRect = joystickBackground.rectTransform;

        if (joystickHandle != null)
            joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
        
        isDragging = false;
    }
    
    public void Initialize(Vector2 position)
    {
        if (!backgroundRect || !joystickHandle) 
            return;
        
        if (rectTransform)
            rectTransform.anchoredPosition = position;
            
        isDragging = true;
    }

    public void UpdateJoystick(Vector2 touchPosition)
    {
        if (!isDragging || !backgroundRect || !joystickHandle) 
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                backgroundRect,
                touchPosition,
                (canvas) ? canvas.worldCamera : _camera,
                out var localPoint)) 
            return;
        
        localPoint.x = (localPoint.x / backgroundRect.sizeDelta.x) * 2;
        localPoint.y = (localPoint.y / backgroundRect.sizeDelta.y) * 2;

        inputVector = localPoint;
        if (inputVector.magnitude > 1.0f)
            inputVector = inputVector.normalized;

        joystickHandle.rectTransform.anchoredPosition = new Vector2(
            inputVector.x * (backgroundRect.sizeDelta.x / 2),
            inputVector.y * (backgroundRect.sizeDelta.y / 2)
        );
    }

    public void ReleaseJoystick()
    {
        isDragging = false;
        inputVector = Vector2.zero;
        
        if (joystickHandle)
            joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
            
        OnJoystickReleased?.Invoke();
    }

    public float GetHorizontal()
    {
        return inputVector.x;
    }

    public float GetVertical()
    {
        return inputVector.y;
    }
}