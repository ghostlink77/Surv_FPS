using UnityEngine;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField] private GameObject go_Base;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemHowtoUseText;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        // ���� ��ġ ����
        var rect = go_Base.GetComponent<RectTransform>().rect;
        go_Base.SetActive(true);
        pos += new Vector3(rect.width * 0.5f, -rect.height, 0f);
        go_Base.transform.position = pos;

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;

        // ������ ��� ��� �ؽ�Ʈ ����
        if (item.itemType == Item.ItemType.Equipment)
        {
            itemHowtoUseText.text = "Right click to Equip";
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            itemHowtoUseText.text = "Right click to Use";
        }
        else
        {
            itemHowtoUseText.text = "";
        }
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
