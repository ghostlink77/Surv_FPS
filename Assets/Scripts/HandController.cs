using System.Collections;
using UnityEngine;

public class HandController : MeleeWeaponController
{
    public static bool isActivate = false;

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
