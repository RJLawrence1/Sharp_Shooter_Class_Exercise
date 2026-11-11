using UnityEngine;

// A ScriptableObject is a data file that lives in your Project window (not in the scene).
// Right-click in Project > Create > Weapons > Weapon Data to make a new weapon.
[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage = 1;
    public float range = 100f;
    public float cooldown = 0.25f;  // seconds between shots
    public int magazineSize = 12;   // ammo you get when you pick it up
    public bool automatic;          // true = hold to fire, false = click per shot
    public float zoomFOV = 30f;     // camera FOV when zoomed (60 = no zoom)
}
