using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask targetMask;

    private Pig pig;

    private void Start()
    {
        pig = GetComponent<Pig>();
    }

    void Update()
    {
        View();
    }

    private Vector3 BounderyAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angleRad), 0f, Mathf.Cos(angleRad));
    }
    private void View()
    {
        Vector3 leftBoundery = BounderyAngle(-viewAngle * 0.5f);
        Vector3 rightBoundery = BounderyAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundery, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundery, Color.red);

        // 일정 반경 안의 오브젝트 탐지(targetMask 레이어)
        Collider[] targets = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (var target in targets)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(direction, transform.forward);          // npc와 객체 간의 각도

            // 시야각 안에 있을 때
            if (angle < viewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.up, direction, out hit, viewDistance))
                {
                    // 시야각 안에 있는 오브젝트가 Player일 때
                    if (hit.transform.name == "Player")
                    {
                        Debug.Log("Player in sight!");
                        Debug.DrawRay(transform.position + transform.up, direction, Color.white);

                        pig.Run(hit.transform.position);
                    }
                }
            }
        }
    }
}
