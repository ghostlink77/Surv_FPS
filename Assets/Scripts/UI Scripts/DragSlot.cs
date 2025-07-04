using UnityEngine;
using UnityEngine.UI;

// 인벤토리의 아이템을 드래그할 때의 슬롯
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;               // 현재 드래그 중인 슬롯

    [SerializeField] private Image imageItem;

    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image image)
    {
        imageItem.sprite = image.sprite;
        SetColor(1f);
    }

    public void SetColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
}
