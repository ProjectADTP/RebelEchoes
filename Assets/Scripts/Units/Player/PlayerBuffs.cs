using System;
using UnityEngine;

public class PlayerBuffs : MonoBehaviour
{
    [Serializable]
    public class BuffData
    {
        public int charges = 2;
        public float duration = 5f;
        public float cooldown = 10f;
        public bool isActive = false;
        public float activeTimer = 0f;
        public float cooldownTimer = 0f;
    }

    [Header("Buff Settings")]
    public BuffData powerBuff;
    public BuffData multiShotBuff;
    public BuffData ghostBuff;

    [Header("References")]
    [SerializeField] private BuffUI buffUI;

    public static PlayerBuffs Instance { get; private set; }

    public event Action OnBuffActivated;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateBuff(powerBuff, BuffType.Power);
        UpdateBuff(multiShotBuff, BuffType.MultiShot);
        UpdateBuff(ghostBuff, BuffType.Ghost);
    }

    private void UpdateBuff(BuffData buff, BuffType type)
    {
        if (buff.isActive)
        {
            buff.activeTimer -= Time.deltaTime;
            if (buff.activeTimer <= 0)
            {
                buff.isActive = false;
                OnBuffEnd(type);
            }
        }
        else if (buff.cooldownTimer > 0)
        {
            buff.cooldownTimer -= Time.deltaTime;
        }

        buffUI?.UpdateBuffUI(type, buff);
    }

    public void ActivateBuff(BuffType type)
    {
        BuffData buff = GetBuffData(type);

        if (buff.charges > 0 && !buff.isActive && buff.cooldownTimer <= 0)
        {
            buff.charges--;
            buff.isActive = true;
            buff.activeTimer = buff.duration;
            
            OnBuffStart(type);
            
            OnBuffActivated?.Invoke();
            
            buffUI?.UpdateBuffUI(type, buff);
        }
    }

    public bool IsBuffActive(BuffType type)
    {
        return GetBuffData(type).isActive;
    }

    public float GetPowerMultiplier()
    {
        return powerBuff.isActive ? 2f : 1f; 
    }

    public bool IsGhostMode()
    {
        return ghostBuff.isActive;
    }

    public int GetShotCount()
    {
        return multiShotBuff.isActive ? 3 : 1;
    }

    public float GetSpreadAngle()
    {
        return multiShotBuff.isActive ? 1f : 0f;
    }

    private BuffData GetBuffData(BuffType type)
    {
        switch (type)
        {
            case BuffType.Power: 
                return powerBuff;
            
            case BuffType.MultiShot:
                return multiShotBuff;
            
            case BuffType.Ghost:
                return ghostBuff;
            
            default: return null;
        }
    }

    private void OnBuffStart(BuffType type)
    {
        switch (type)
        {
            case BuffType.Power:
                Debug.Log("Power Buff Activated");
                break;
            case BuffType.MultiShot:
                Debug.Log("MultiShot Buff Activated");
                break;
            case BuffType.Ghost:
                Debug.Log("Ghost Buff Activated");
                break;
        }
    }

    private void OnBuffEnd(BuffType type)
    {
        switch (type)
        {
            case BuffType.Power:
                Debug.Log("Power Buff Deactivated");
                break;
            case BuffType.MultiShot:
                Debug.Log("MultiShot Buff Deactivated");
                break;
            case BuffType.Ghost:
                Debug.Log("Ghost Buff Deactivated");
                break;
        }

        BuffData buff = GetBuffData(type);
        buff.cooldownTimer = buff.cooldown;
        buffUI?.UpdateBuffUI(type, buff);
    }
    
    public void AddBuffCharge(BuffType type)
    {
        BuffData buff = GetBuffData(type);
        buff.charges++;
        buffUI?.UpdateBuffUI(type, buff);
    }
}

public enum BuffType
{
    Power,
    MultiShot,
    Ghost
}