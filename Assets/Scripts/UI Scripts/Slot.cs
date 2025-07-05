using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private GameObject go_countImage;

    private ItemEffectDataBase itemEffectDataBase;

    void Start()
    {
        itemEffectDataBase = FindFirstObjectByType<ItemEffectDataBase>();
    }

    // �κ��丮 ���Կ� ������ �߰�
    public void AddItem(Item item, int count = 1)
    {
        this.item = item;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_countImage.SetActive(true);
            itemCountText.text = itemCount.ToString();
        }
        else
        {
            itemCountText.text = "0";
            go_countImage.SetActive(false);
        }

        SetColor(1f);
    }

    // ������ ���� ����
    public void SetSlotCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0f);
        itemCountText.text = "0";
        go_countImage.SetActive(false);
    }

    // �巡�� ���� ���԰� ���� ������ �������� ��ȯ(OnDrop���� ȣ��)
    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(tempItem, tempCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == null)
            {
                return;
            }
            itemEffectDataBase.UseItem(item);
            if (item.itemType == Item.ItemType.Used)
            {
                SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0f);
        DragSlot.instance.dragSlot = null;

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
        
    }
}
