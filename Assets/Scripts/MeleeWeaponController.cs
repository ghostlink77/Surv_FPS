using UnityEngine;
using System.Collections;

// 근접무기 컨트롤러 추상 클래스
public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] protected MeleeWeapon currentMeleeWeapon;

    protected bool isAttacking = false;
    protected bool isSwinging = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Inventory.inventoryActivated)
        {
            return;
        }
        if (Input.GetButton("Fire1"))
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        currentMeleeWeapon.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(currentMeleeWeapon.attackActivateDelay);
        isSwinging = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentMeleeWeapon.attackRecoveryDelay);
        isSwinging = false;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay);
        isAttacking = false;
    }
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true;
        }
        return false;
    }

    public virtual void MeleeWeaponChange(MeleeWeapon meleeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentMeleeWeapon = meleeWeapon;
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentMeleeWeapon.animator;
        currentMeleeWeapon.transform.localPosition = Vector3.zero;

        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
