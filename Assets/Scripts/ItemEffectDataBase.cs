using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY �� �ϳ�")]
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
        // �������� ����� ��� ����(WeaponManager)
        if (item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        // �������� �Ҹ�ǰ�� ��� ȿ�� ����(StatusController)
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
                                Debug.Log("�߸��� ������ �����߽��ϴ�. : " + effect.part[i]);
                                break;
                        }
                        Debug.Log("Used item: " + item.itemName);
                    }
                    return; 
                }
            }
            Debug.Log("��ġ�ϴ� itemName�� ã�� �� �����ϴ�: " + item.itemName);
        }
    }
}
