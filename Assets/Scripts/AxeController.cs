using System.Collections;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    public static bool isActivate = false;

    //private void Start()
    //{
    //    WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
    //    WeaponManager.currentWeaponAnimator = currentMeleeWeapon.animator;
    //}
    void Update()
    {
        if (!isActivate) return;
        TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwinging)
        {
            if (CheckObject())
            {
                isSwinging = false;
                Debug.Log("Hit: " + hitInfo.collider.name);
            }
            yield return null;
        }
    }

    public override void MeleeWeaponChange(MeleeWeapon meleeWeapon)
    {
        base.MeleeWeaponChange(meleeWeapon);
        isActivate = true;
    }
}
