using UnityEngine;
using System.Collections;
using TreeEditor;

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
                else if(hitInfo.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<Pig>().Damaged(1, transform.position);
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
