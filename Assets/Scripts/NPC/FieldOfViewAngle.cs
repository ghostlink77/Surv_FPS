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

        // ���� �ݰ� ���� ������Ʈ Ž��(targetMask ���̾�)
        Collider[] targets = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (var target in targets)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(direction, transform.forward);          // npc�� ��ü ���� ����

            // �þ߰� �ȿ� ���� ��
            if (angle < viewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.up, direction, out hit, viewDistance))
                {
                    // �þ߰� �ȿ� �ִ� ������Ʈ�� Player�� ��
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
