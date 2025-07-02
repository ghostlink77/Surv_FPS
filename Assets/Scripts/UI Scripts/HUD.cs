using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private GunController gunController;
    [SerializeField] private GameObject goAmmoHUD;
    [SerializeField] private TextMeshProUGUI[] textAmmo;
    private Gun currentGun;

    void Update()
    {
        ChectAmmo();
    }

    private void ChectAmmo()
    {
        currentGun = gunController.GetGun();
        textAmmo[0].text = currentGun.carryAmmo.ToString();
        textAmmo[1].text = currentGun.ammoCapacity.ToString();
        textAmmo[2].text = currentGun.currentAmmo.ToString();
    }
}
