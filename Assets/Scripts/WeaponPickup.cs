using UnityEngine;

// Put this on a pickup object with a Collider set to Is Trigger.
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponData weapon;     // drag RifleData or SniperData here
    [SerializeField] float spinSpeed = 90f; // just so it's easy to spot

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponController controller = other.GetComponent<WeaponController>();

        if (controller != null)
        {
            controller.SetWeapon(weapon);
            gameObject.SetActive(false);
        }
    }
}
