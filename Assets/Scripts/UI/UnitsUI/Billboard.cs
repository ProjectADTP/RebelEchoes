using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}