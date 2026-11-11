using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public TowerData arrowData;
    public TowerData iceData;
    public TowerData fireData;
    public TowerData cannonData;

    public void OnClickArrow() { MouseTrackBuilder.Instance.SelectTowerToBuild(arrowData); }
    public void OnClickIce() { MouseTrackBuilder.Instance.SelectTowerToBuild(iceData); }
    public void OnClickFire() { MouseTrackBuilder.Instance.SelectTowerToBuild(fireData); }
    public void OnClickCannon() { MouseTrackBuilder.Instance.SelectTowerToBuild(cannonData); }
}
