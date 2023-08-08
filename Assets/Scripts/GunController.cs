using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Transform firePoint;

    [SerializeField] float bulletSpeed = 50.0f;

    [SerializeField] float lifeTime = 5.0f;

    [SerializeField] Vector3 bulletRotation;

    [SerializeField] int maximunAmmunition = 16;

    [SerializeField] float reloadTime = 2.0f;

    float _currentAmmunition = -1;

    bool _isReloading = false;

    float _nextFireTime = 0.0f;

    [SerializeField] int fireRate = 4;

    Animator _animator;

    [SerializeField] StarterAssetsInputs starterAssetsInputs;

    private void Update()
    {
        if (_isReloading)
        {
            return;
        }

        if (_currentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + 1.0f / fireRate;

            Shoot();
        }
    }

    private void Start()
    {
        if (_currentAmmunition == -1)
        {
            _currentAmmunition = maximunAmmunition;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(bulletRotation));

        Rigidbody rB = bullet.GetComponent<Rigidbody>();

        if (starterAssetsInputs != null && starterAssetsInputs.aim)
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000.0f))
            {
                mouseWorldPosition = raycastHit.point;
            }

            Vector3 aimDirection = (mouseWorldPosition - firePoint.position).normalized;

            rB.velocity = aimDirection * bulletSpeed;
        }
        else
        {
            rB.velocity = firePoint.forward * bulletSpeed;
        }

        Destroy(bullet, lifeTime);

        _currentAmmunition--;
    }

    IEnumerator Reload()
    {
        _isReloading = true;

        _animator.SetBool("reload", true);

        yield return new WaitForSeconds(reloadTime);

        _currentAmmunition = maximunAmmunition;

        _animator.SetBool("reload", false);

        _isReloading = false;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}