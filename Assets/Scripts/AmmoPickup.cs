using UnityEngine;

// Put this on an ammo pickup object with a Collider set to Is Trigger.
public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int amount = 10;
    [SerializeField] float spinSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponController controller = other.GetComponent<WeaponController>();

        if (controller != null)
        {
            controller.AddAmmo(amount);
            gameObject.SetActive(false);
        }
    }
}
