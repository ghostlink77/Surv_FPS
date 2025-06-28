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
    [SerializeField] private AxeController axeController;
    [SerializeField] private PickaxeController pickaxeController;
    [SerializeField] private Gun[] guns;
    [SerializeField] private MeleeWeapon[] hands;
    [SerializeField] private MeleeWeapon[] axes;
    [SerializeField] private MeleeWeapon[] pickaxes;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> handDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> axeDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> pickaxeDictionary = new Dictionary<string, MeleeWeapon>();

    void Start()
    {
        foreach (Gun gun in guns)
        {
            gunDictionary.Add(gun.gunName, gun);
        }
        foreach (MeleeWeapon hand in hands)
        {
            handDictionary.Add(hand.MeleeWeaponName, hand);
        }
        foreach (MeleeWeapon axe in axes)
        {
            axeDictionary.Add(axe.MeleeWeaponName, axe);
        }
        foreach (MeleeWeapon pickaxe in pickaxes)
        {
            axeDictionary.Add(pickaxe.MeleeWeaponName, pickaxe);
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
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine("Axe", "Axe"));
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(ChangeWeaponCoroutine("Pickaxe", "Pickaxe"));
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
            case "Axe":
                AxeController.isActivate = false;
                break;
            case "Pickaxe":
                PickaxeController.isActivate = false;
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
            handController.MeleeWeaponChange(handDictionary[name]);
        }
        else if (type == "Axe")
        {
            axeController.MeleeWeaponChange(axeDictionary[name]);
        }
        else if (type == "Pickaxe")
        {
            pickaxeController.MeleeWeaponChange(pickaxeDictionary[name]);
        }
    }
}
