using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [System.Serializable]
    public class BuffUIData
    {
        public Button button;
        public TMP_Text countText;
        public Image activeOverlay;
        public Image cooldownOverlay;
    }

    [Header("Buff UI Elements")]
    public BuffUIData powerBuffUI;
    public BuffUIData multiShotBuffUI;
    public BuffUIData ghostBuffUI;

    private void Start()
    {
        if (powerBuffUI.button != null)
            powerBuffUI.button.onClick.AddListener(() => PlayerBuffs.Instance?.ActivateBuff(BuffType.Power));

        if (multiShotBuffUI.button != null)
            multiShotBuffUI.button.onClick.AddListener(() => PlayerBuffs.Instance?.ActivateBuff(BuffType.MultiShot));

        if (ghostBuffUI.button != null)
            ghostBuffUI.button.onClick.AddListener(() => PlayerBuffs.Instance?.ActivateBuff(BuffType.Ghost));
        
        HideAllOverlays(powerBuffUI);
        HideAllOverlays(multiShotBuffUI);
        HideAllOverlays(ghostBuffUI);
    }

    public void UpdateBuffUI(BuffType type, PlayerBuffs.BuffData buff)
    {
        BuffUIData uiData = GetUIData(type);

        if (uiData == null)
            return;
        
        if (uiData.countText != null)
        {
            uiData.countText.text = buff.charges.ToString();
        }
        
        HideAllOverlays(uiData);
        
        if (buff.isActive)
        {
            ShowActiveOverlay(uiData);
        }
        else if (buff.cooldownTimer > 0)
        {
            ShowCooldownOverlay(uiData);
            UpdateCooldownAlpha(uiData, buff.cooldownTimer / buff.cooldown);
        }
        
        if (uiData.button != null)
        {
            uiData.button.interactable = buff.charges > 0 && !buff.isActive && buff.cooldownTimer <= 0;
        }
    }

    private void HideAllOverlays(BuffUIData uiData)
    {
        if (uiData.activeOverlay != null)
            uiData.activeOverlay.gameObject.SetActive(false);

        if (uiData.cooldownOverlay != null)
            uiData.cooldownOverlay.gameObject.SetActive(false);
    }

    private void ShowActiveOverlay(BuffUIData uiData)
    {
        if (uiData.activeOverlay != null)
        {
            uiData.activeOverlay.gameObject.SetActive(true);
        }
    }

    private void ShowCooldownOverlay(BuffUIData uiData)
    {
        if (uiData.cooldownOverlay != null)
        {
            uiData.cooldownOverlay.gameObject.SetActive(true);
        }
    }

    private void UpdateCooldownAlpha(BuffUIData uiData, float cooldownPercent)
    {
        if (uiData.cooldownOverlay != null)
        {
            Color color = uiData.cooldownOverlay.color;
            color.a = Mathf.Clamp01(cooldownPercent) * 0.7f; 
            uiData.cooldownOverlay.color = color;
        }
    }

    private BuffUIData GetUIData(BuffType type)
    {
        switch (type)
        {
            case BuffType.Power: 
                return powerBuffUI;
            case BuffType.MultiShot: 
                return multiShotBuffUI;
            case BuffType.Ghost: 
                return ghostBuffUI;
            default: return null;
        }
    }
}