using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 중 하나")]
    public string[] part;
    public int[] num;
}

public class ItemEffectDataBase : MonoBehaviour
{
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    [SerializeField] private ItemEffect[] itemEffects;

    [SerializeField] private StatusController playerStatus;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private SlotToolTip slotToolTip;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        slotToolTip.ShowToolTip(item, pos);
    }
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }

    public void UseItem(Item item)
    {
        // 아이템이 장비일 경우 장착(WeaponManager)
        if (item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        // 아이템이 소모품일 경우 효과 적용(StatusController)
        else if (item.itemType == Item.ItemType.Used)
        {
            foreach (ItemEffect effect in itemEffects)
            {
                if (effect.itemName == item.itemName)
                {
                    for (int i = 0; i< effect.part.Length; i++)
                    {
                        switch (effect.part[i])
                        {
                            case HP:
                                playerStatus.IncreaseHp(effect.num[i]);
                                break;
                            case SP:
                                playerStatus.IncreaseSp(effect.num[i]);
                                break;
                            case DP:
                                playerStatus.IncreaseDp(effect.num[i]);
                                break;
                            case HUNGRY:
                                playerStatus.IncreaseHungry(effect.num[i]);
                                break;
                            case THIRSTY:
                                playerStatus.IncreaseThirsty(effect.num[i]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 부위를 지정했습니다. : " + effect.part[i]);
                                break;
                        }
                        Debug.Log("Used item: " + item.itemName);
                    }
                    return; 
                }
            }
            Debug.Log("일치하는 itemName을 찾을 수 없습니다: " + item.itemName);
        }
    }
}
