using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string MeleeWeaponName;

    public bool isAxe;
    public bool isPickaxe;
    public bool isHand;

    public float range;
    public int damage;
    public int workSpeed;
    public float attackDelay;
    public float attackActivateDelay;
    public float attackRecoveryDelay;

    public Animator animator;
}
