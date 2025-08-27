using UnityEngine;

public class InputReader : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private GameObject joystickPrefab;
    [SerializeField] private PlayerMover inputReceiver;

    private VirtualJoystick currentJoystick;
    private bool isTracking;
    private Canvas cachedCanvas;

    private void Awake()
    {
        cachedCanvas = FindObjectOfType<Canvas>();
        
        isTracking = false;
    }

    private void Update()
    {
        if (!joystickPrefab) 
            return;
        
        HandleInput();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateJoystick(Input.mousePosition);
            isTracking = true;
        }
        
        if (isTracking && currentJoystick != null)
        {
            currentJoystick.UpdateJoystick(Input.mousePosition);
            SendInputToReceiver();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseJoystick();
        }
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                CreateJoystick(touch.position);
                isTracking = true;
            }
            
            if (isTracking && currentJoystick != null)
            {
                currentJoystick.UpdateJoystick(touch.position);
                SendInputToReceiver();
            }
            
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                ReleaseJoystick();
            }
        }
        else if (isTracking)
        {
            SendInputToReceiver(Vector2.zero);
        }
    }
#endif

    private void CreateJoystick(Vector2 position)
    {
        CleanupJoystick();

        if (!joystickPrefab || !cachedCanvas)
            return;

        Vector2 anchoredPosition = ScreenToAnchoredPosition(position, cachedCanvas);
        
        GameObject joyGO = Instantiate(joystickPrefab, cachedCanvas.transform, false);
        
        if (!joyGO) 
            return;
        
        if (joyGO.TryGetComponent(out RectTransform joyRect)) 
            joyRect.anchoredPosition = anchoredPosition;

        if (!joyGO.TryGetComponent(out VirtualJoystick virtualJoystick))
            return;
        
        currentJoystick = virtualJoystick;
            
        currentJoystick.Initialize(anchoredPosition);
        currentJoystick.OnJoystickReleased += OnJoystickReleased;
    }

    private void SendInputToReceiver(Vector2? input = null)
    {
        Vector2 inputValue = input ?? (currentJoystick ? 
            new Vector2(currentJoystick.GetHorizontal(), currentJoystick.GetVertical()) : 
            Vector2.zero);
            
        if (inputReceiver)
        {
            inputReceiver.SetMovementInput(inputValue);
        }
    }

    private void ReleaseJoystick()
    {
        if (currentJoystick)
        {
            currentJoystick.ReleaseJoystick();
        }
        
        isTracking = false;
        
        SendInputToReceiver(Vector2.zero);
    }

    private Vector2 ScreenToAnchoredPosition(Vector2 screenPosition, Canvas canvas)
    {
        if (!canvas) 
            return Vector2.zero;
        
        Vector2 anchoredPosition;
        
        if (canvas.TryGetComponent(out RectTransform canvasRect))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, 
                screenPosition, 
                canvas.worldCamera, 
                out anchoredPosition);
        }
        else
        {
            anchoredPosition = screenPosition;
        }
            
        return anchoredPosition;
    }

    private void OnJoystickReleased()
    {
        CleanupJoystick();
    }

    private void CleanupJoystick()
    {
        if (currentJoystick)
        {
            currentJoystick.OnJoystickReleased -= OnJoystickReleased;
            
            if (currentJoystick.gameObject)
                Destroy(currentJoystick.gameObject);
            
            currentJoystick = null;
        }
        
        isTracking = false;
        
        SendInputToReceiver(Vector2.zero);
    }

    private void OnDisable()
    {
        CleanupJoystick();
    }

    private void OnDestroy()
    {
        CleanupJoystick();
    }
}