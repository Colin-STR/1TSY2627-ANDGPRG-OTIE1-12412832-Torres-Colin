using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeUI : MonoBehaviour
{
    public static TowerUpgradeUI Instance;

    [Header("UI Panels & Elements")]
    public GameObject upgradePanel; 
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    [Header("Confirmation Dialog")]
    public GameObject confirmationPopup;
    public Button confirmYesButton;
    public Button confirmNoButton;

    private TowerUpgrade activeTower;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HidePanel();
        if (confirmationPopup != null) confirmationPopup.SetActive(false);

        if (upgradeButton != null) upgradeButton.onClick.AddListener(OnClickUpgradeRequest);
        if (confirmYesButton != null) confirmYesButton.onClick.AddListener(OnClickConfirmUpgrade);
        if (confirmNoButton != null) confirmNoButton.onClick.AddListener(OnClickCancelConfirmation);
    }

    public void ShowPanel(TowerUpgrade tower)
    {
        activeTower = tower;
        upgradePanel.SetActive(true);
        if (confirmationPopup != null) confirmationPopup.SetActive(false);
        UpdateUI();
    }

    public void HidePanel()
    {
        upgradePanel.SetActive(false);
        activeTower = null;
        if (confirmationPopup != null) confirmationPopup.SetActive(false);
    }

    public void UpdateUI()
    {
        if (activeTower == null) return;

        towerNameText.text = activeTower.gameObject.name.Replace("(Clone)", "");
        levelText.text = $"Level: {activeTower.level} / {activeTower.maxLevel}";

        if (activeTower.CanUpgrade())
        {
            int cost = activeTower.GetUpgradeCost();
            costText.text = $"Upgrade Cost: {cost} Gold";
            upgradeButton.interactable = true;
        }
        else
        {
            costText.text = "MAX LEVEL REACHED";
            upgradeButton.interactable = false;
        }
    }

    void OnClickUpgradeRequest()
    {
        if (activeTower == null || !activeTower.CanUpgrade()) return;

        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(true);
        }
        else
        {
            ExecuteUpgrade();
        }
    }

    void OnClickConfirmUpgrade()
    {
        ExecuteUpgrade();
        if (confirmationPopup != null) confirmationPopup.SetActive(false);
    }

    void OnClickCancelConfirmation()
    {
        if (confirmationPopup != null) confirmationPopup.SetActive(false);
    }

    void ExecuteUpgrade()
    {
        if (activeTower != null)
        {
            activeTower.UpgradeTower();
            UpdateUI();
        }
    }
}
