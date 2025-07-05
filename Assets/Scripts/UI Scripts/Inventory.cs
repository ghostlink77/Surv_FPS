using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;
    [SerializeField] private GameObject go_SlotsParent;

    private Slot[] slots;

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }
    private void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
    }

    // �κ��丮�� ������ ȹ��
    public void AcquireItem(Item item, int count = 1)
    {
        // �������� Equipment Ÿ���� �ƴ� ���, ���� �������� �ִ��� Ȯ���� ���� ����(SetSlotCount)
        if (item.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item.itemName == item.itemName)
                {
                    slots[i].SetSlotCount(count);
                    return;
                }
            }
        }

        // �������� Equipment Ÿ���̰ų�, ���� �������� ���� ��� ���ο� ���Կ� ������ �߰�(AddItem)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
    }
}
