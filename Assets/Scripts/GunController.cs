﻿using UnityEngine;

public class GunController : MonoBehaviour
{
    GameController gc;

    [Header("Universal Settings")]
    public float weaponFireRate = 0.1f;
    public float weaponInaccuracy = 0.0f;
    public bool semiAuto;
    public bool hitscan;

    float fireRateTimer = 0.0f;
    GameObject playerCam;

    [Header("Hitscan Settings")]
    public float weaponRange = 100.0f;
    public LayerMask weaponHitLayer;
    public GameObject bulletDecal;

    [Header("Projectile Settings")]
    public GameObject projectile;
    public float projectileForce;
    public bool randomTourque;

    // Use this for initialization
    void Start ()
    {
        playerCam = Camera.main.gameObject;
        gc = GameController.instance;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fireRateTimer += Time.fixedDeltaTime;

        // Check if the gun is semi automatic
        if (semiAuto)
        {
            // If we're allowed to fire...
            if (Input.GetKeyDown(KeyCode.Mouse0)
                && fireRateTimer >= weaponFireRate
                && !gc.paused)
            {
                FireGun();
            }
        }
        else
        {
            // If we're allowed to fire...
            if (Input.GetKey(KeyCode.Mouse0)
                && fireRateTimer >= weaponFireRate
                && !gc.paused)
            {
                FireGun();
            }
        }

    }

    // Used to create inaccuracy when the weapon is shot
    Vector3 GetWeaponInaccuracy(Vector3 center, float radius)
    {
        float ang = Random.value * 360; // Get angle
        Vector3 pos;

        // Get point in space based on circle
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.x + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z; // We only need the coords in 2D space, so z can be left at 0
        return pos;
    }

    void FireGun()
    {
        fireRateTimer = 0.0f;

        // Check whether we're firing as hitscan or projectile
        if (hitscan)
        {
            // Weapons hit detection
            RaycastHit weaponRay;
            
            // Use raycast to check if we hit something
            if (Physics.Raycast(playerCam.transform.position,
            playerCam.transform.TransformDirection(GetWeaponInaccuracy(Vector3.forward, Random.Range(-weaponInaccuracy / 2, weaponInaccuracy / 2))),
            out weaponRay, weaponRange, weaponHitLayer.value))
            {
            
                // Check if we hit the enemy AI
                if (weaponRay.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    weaponRay.collider.gameObject.GetComponent<AINavigation>().ApplyDamage(10.0f);
                }
                else // If not, then create a bullet decal
                {
                    Instantiate(bulletDecal, weaponRay.point, Quaternion.Euler(weaponRay.normal));
                }
            }
        }
        else
        {
            GameObject firedProj = Instantiate(projectile,
                Camera.main.transform.position + Camera.main.transform.TransformDirection(Vector3.forward),
                Camera.main.transform.rotation) as GameObject;

            firedProj.GetComponent<Rigidbody>().
                AddForce(Camera.main.transform.TransformDirection(
                    GetWeaponInaccuracy(Vector3.forward, Random.Range(-weaponInaccuracy / 2, weaponInaccuracy / 2)))
                    * projectileForce, ForceMode.Impulse);
            if (randomTourque)
                firedProj.GetComponent<ConstantForce>().torque = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        }
    }
}
