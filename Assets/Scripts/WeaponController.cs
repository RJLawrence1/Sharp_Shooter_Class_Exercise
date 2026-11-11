using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on the Player (same object as PlayerController).
// Left click = shoot, Right click = zoom, 1/2 = switch weapons you've picked up.
public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera playerCamera;
    [SerializeField] TMP_Text ammoText; // optional: shows weapon name + ammo on screen

    [Header("Weapon")]
    [SerializeField] WeaponData startingWeapon;  // the gun you spawn with
    [SerializeField] LayerMask hittableLayers;   // which layers the raycast can hit

    [Header("Zoom")]
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float zoomSpeed = 10f;      // higher = faster zoom

    WeaponData currentWeapon;
    int ammo;
    float nextFireTime;

    // Weapons you've picked up, and how much ammo each one has left
    List<WeaponData> ownedWeapons = new List<WeaponData>();
    Dictionary<WeaponData, int> savedAmmo = new Dictionary<WeaponData, int>();

    void Start()
    {
        playerCamera.fieldOfView = normalFOV;

        if (startingWeapon != null)
        {
            SetWeapon(startingWeapon);
        }
    }

    void Update()
    {
        if (currentWeapon == null) return; // no gun yet

        HandleShooting();
        HandleZoom();
        HandleSwitching();
    }

    // ---------- Shooting ----------

    void HandleShooting()
    {
        // Automatic weapons fire while held; others fire once per click
        bool wantsToFire = currentWeapon.automatic
            ? Mouse.current.leftButton.isPressed
            : Mouse.current.leftButton.wasPressedThisFrame;

        if (wantsToFire && CanFire())
        {
            RecordShot();
            Shoot();
        }
    }

    bool CanFire()
    {
        // Need ammo AND the cooldown has to be over
        return ammo > 0 && Time.time >= nextFireTime;
    }

    void RecordShot()
    {
        ammo--;
        nextFireTime = Time.time + currentWeapon.cooldown;
        UpdateAmmoText();
    }

    void Shoot()
    {
        // Raycast overload: origin, direction, out hit, range, layer filter, ignore triggers
        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            currentWeapon.range,
            hittableLayers,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log(currentWeapon.weaponName + " hit: " + hit.collider.name);

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
            }
        }

        Debug.DrawRay(playerCamera.transform.position,
            playerCamera.transform.forward * currentWeapon.range, Color.red, 1f);
    }

    // ---------- Zoom ----------

    void HandleZoom()
    {
        bool zooming = Mouse.current.rightButton.isPressed;
        float targetFOV = zooming ? currentWeapon.zoomFOV : normalFOV;

        // Mathf.Lerp: move part of the way toward the target each frame = smooth zoom
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    // ---------- Switching (1, 2, 3 keys) ----------

    void HandleSwitching()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SwitchTo(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SwitchTo(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SwitchTo(2);
    }

    void SwitchTo(int index)
    {
        if (index >= ownedWeapons.Count) return;     // don't own that slot yet

        WeaponData weapon = ownedWeapons[index];
        if (weapon == currentWeapon) return;         // already holding it

        savedAmmo[currentWeapon] = ammo;             // remember ammo in the old gun
        currentWeapon = weapon;
        ammo = savedAmmo[weapon];                    // restore ammo in the new gun
        playerCamera.fieldOfView = normalFOV;        // reset zoom
        UpdateAmmoText();
    }

    // ---------- Called by pickups ----------

    // Called by WeaponPickup. newWeapon is the parameter.
    public void SetWeapon(WeaponData newWeapon)
    {
        if (currentWeapon != null)
        {
            savedAmmo[currentWeapon] = ammo;         // remember ammo in the old gun
        }

        if (!ownedWeapons.Contains(newWeapon))
        {
            ownedWeapons.Add(newWeapon);             // new gun goes in the next number slot
        }

        currentWeapon = newWeapon;
        ammo = currentWeapon.magazineSize;           // fresh magazine
        playerCamera.fieldOfView = normalFOV;        // restore normal FOV before new zoom
        UpdateAmmoText();
    }

    // Called by AmmoPickup
    public void AddAmmo(int amount)
    {
        ammo += amount;
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentWeapon.weaponName + "   Ammo: " + ammo;
        }
    }
}
