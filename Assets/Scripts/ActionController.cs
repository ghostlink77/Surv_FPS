using UnityEngine;
using TMPro;

using System.Security.Cryptography;

public class ActionController : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Inventory inventory;

    private bool pickupActivated = false;

    private RaycastHit hitInfo;

    void Update()
    {
        TryAction();
        CheckItem();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            Pickup();
        }
    }

    // 주울 수 있는 아이템을 확인
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            // 주울 수 있는 아이템이면 pick up 텍스트 표시
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " pick up" + "<color=yellow>(E)</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);

    }

    private void Pickup()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                // 인벤토리에 아이템 추가
                inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
                Debug.Log("Picked up: " + hitInfo.transform.GetComponent<ItemPickup>().item.itemName);
            }
        }
    }
}
