using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 currentPos;

    [SerializeField] private Vector3 limitPos;
    [SerializeField] private Vector3 fineSightLimitPos;
    [SerializeField] private Vector3 smoothSway;
    [SerializeField] private GunController gunController;

    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated)
        {
            TrySway();
        }
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        float rangeX;
        float rangeY;

        if (!gunController.isFineSightMode)
        {
            rangeX = Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x);
            rangeY = Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.y), -limitPos.y, limitPos.y);
        }
        else
        {
            rangeX = Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x);
            rangeY = Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y);
        }

        currentPos.Set(rangeX, rangeY, originPos.z);
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
