using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;
    public static Transform currentWeapon;
    public static Animator currentWeaponAnimator;

    [SerializeField] private float changeWeaponDelay;
    [SerializeField] private float changeWeaponEndDelay;
    [SerializeField] private string currentWeaponType;

    [SerializeField] private GunController gunController;
    [SerializeField] private HandController handController;
    [SerializeField] private Gun[] guns;
    [SerializeField] private Hand[] hands;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> HandDictionary = new Dictionary<string, Hand>();

    void Start()
    {
        foreach (Gun gun in guns)
        {
            gunDictionary.Add(gun.gunName, gun);
        }
        foreach (Hand hand in hands)
        {
            HandDictionary.Add(hand.handName, hand);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("Hand", "¸Ç¼Õ"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("Gun", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelay);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelay);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "Gun":
                gunController.CancelFineSight();
                gunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "Hand":
                HandController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string type, string name)
    {
        if (type == "Gun")
        {
            gunController.GunChange(gunDictionary[name]);
        }
        else if (type == "Hand")
        {
            handController.HandChange(HandDictionary[name]);
        }
    }
}
