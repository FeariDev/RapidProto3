using UnityEngine;

public class ItemSO : ScriptableObject
{
    [Header("Base Item Properties")]
    public string itemName;
    public int maxStackAmount = 1;
    /// <summary>
    /// Todennäköisyys saada itemi
    /// </summary>
    public int weight;
}
