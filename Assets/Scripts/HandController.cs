using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivate = false;

    [SerializeField] private Hand currentHand;

    private bool isAttacking = false;
    private bool isSwinging = false;

    private RaycastHit hitInfo;

    void Update()
    {
        if (!isActivate) return;
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        currentHand.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackActivateDelay);
        isSwinging = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackRecoveryDelay);
        isSwinging = false;

        yield return new WaitForSeconds(currentHand.attackDelay);
        isAttacking = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwinging)
        {
            if (CheckObject())
            {
                isSwinging = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentHand.animator;
        currentHand.transform.localPosition = Vector3.zero;

        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
