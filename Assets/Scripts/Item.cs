using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string weaponType;

    public Sprite itemImage;
    public GameObject itemPrefab;
    public ItemType itemType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC 
    }
}
