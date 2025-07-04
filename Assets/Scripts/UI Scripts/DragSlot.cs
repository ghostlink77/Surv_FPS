using UnityEngine;
using UnityEngine.UI;

// �κ��丮�� �������� �巡���� ���� ����
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;               // ���� �巡�� ���� ����

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
