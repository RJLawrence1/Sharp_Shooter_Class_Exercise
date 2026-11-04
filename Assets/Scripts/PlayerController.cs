using UnityEngine;
using UnityEngine.InputSystem;

// Put this on the Player (the thing the enemy chases).
// W/S = move forward/back, A/D = turn, Left Click or Space = shoot.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 120f; // degrees per second

    [Header("Shooting")]
    [SerializeField] Camera playerCamera;  // the camera that's a child of the Player
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 1;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();

        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Move()
    {
        float forward = 0f;
        float turn = 0f;

        if (Keyboard.current.wKey.isPressed) forward += 1f;
        if (Keyboard.current.sKey.isPressed) forward -= 1f;
        if (Keyboard.current.dKey.isPressed) turn += 1f;
        if (Keyboard.current.aKey.isPressed) turn -= 1f;

        // Turn left/right
        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        // Move forward/back. SimpleMove handles gravity and won't walk through walls.
        controller.SimpleMove(transform.forward * forward * moveSpeed);
    }

    void Shoot()
    {
        RaycastHit hit;

        // Origin = camera position, direction = camera forward,
        // out hit = stores what we hit, range = how far the ray goes
        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Does the thing we hit have EnemyHealth on it?
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Draws the ray in the Scene view for 1 second (handy for testing)
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red, 1f);
    }
}
