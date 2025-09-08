using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float activationInterval = 3f;
    [SerializeField] private float activeDuration = 2f;

    [Header("Components")]
    [SerializeField] private TrapSpikes trapSpikes;
    
    private Coroutine activationCoroutine = null;
    private bool isInitialized = false;

    private void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        if (isInitialized) return;
        
        if (trapSpikes != null)
            trapSpikes.DeactivateSpikes();
        
        activationCoroutine = StartCoroutine(ActivationLoop());
        isInitialized = true;
    }
    
    private IEnumerator ActivationLoop()
    {
        yield return new WaitForSeconds(Random.Range(0f, activationInterval));
        
        while (true)
        {
            ToggleTrap();
            
            yield return new WaitForSeconds(activeDuration);
            
            DeactivateNow();
            
            yield return new WaitForSeconds(activationInterval);
        }
    }
    
    private void ToggleTrap()
    {
        if (trapSpikes != null)
        {
            trapSpikes.ActivateSpikes();
        }
    }
    
    private void DeactivateNow()
    {
        if (trapSpikes != null)
        {
            trapSpikes.DeactivateSpikes();
        }
    }
    
    private void OnDestroy()
    {
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
        }
    }
}