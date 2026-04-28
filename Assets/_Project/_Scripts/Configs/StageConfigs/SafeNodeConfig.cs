using UnityEngine;

[CreateAssetMenu(fileName = "Safe Node", menuName = "Stage/Safe")]
public class SafeNodeConfig : NodeConfigs
{
    public bool canHeal;
    public bool canShop;
}