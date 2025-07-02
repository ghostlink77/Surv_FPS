using UnityEngine;
using System.Collections;

public class PickaxeController : MeleeWeaponController
{
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentMeleeWeapon.animator;
    }
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
                if (hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
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
