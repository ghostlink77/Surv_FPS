using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;
    public int damage;

    public int ammoCapacity;    // �� źâ�� �ִ� ź��
    public int currentAmmo;     // ���� źâ�� �ִ� ź��
    public int maxAmmo;         // �ִ� ���� ���� ź��
    public int carryAmmo;       // ���� ���� ���� ź��

    public float retroActionForce;
    public float retroActionFineSightForce;
    public Vector3 fineSightOriginPos;

    public Animator animator;
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;
}
