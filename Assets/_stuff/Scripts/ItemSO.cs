using UnityEngine;

public class ItemSO : ScriptableObject
{
    [Header("Base Item Properties")]
    public Sprite sprite;
    public string itemName;
    public string itemDesc;
    public int maxStackAmount = 1;
    /// <summary>
    /// Todennäköisyys saada itemi
    /// </summary>
    public int weight;
}
