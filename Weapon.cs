using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletVelocity;

    [Header("Shooting Settings")]
    [SerializeField] private ShootingMode mode;
    public WeaponModel weaponModel;
    [SerializeField] private bool canShoot;
    [Range(0.1f,1f)]
    [SerializeField] private float delay = 0.25f;
    private float nextTimeToShoot = 0;
    [SerializeField] private int bulletsInBurst = 1;

    [Header("Spread settings")]
    [Range(0,2)]
    [SerializeField] private float spreadRangeX = 1;
    [Range(0, 2)]
    [SerializeField] private float spreadRangeY = 1;

    [Header("Ammo settings")]
    public int magazineSize = 30;
    public int bulletsLeft;
    [SerializeField] private bool magazineIsNotEmpty = true;

    public Recoil Camera_Recoil_Script;
    public WeaponRecoil Weapon_Recoil_Script;
    public enum ShootingMode
    {
        Automatic,
        Single,
        Burst
    }

    public enum WeaponModel
    {
        AKM,
        Shotgun
    }

    private void Start()
    {
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        canShoot = Time.time >= nextTimeToShoot;

        if (canShoot && magazineIsNotEmpty)
        {
            if (mode == ShootingMode.Automatic)
            {
                //���� ������� � ����������� �������� ������ ������ delay �� �� ��������
                if (Input.GetMouseButton(0) /*&& Time.time >= nextTimeToShoot*/)
                {
                    Shoot();
                }
            }

            if (mode == ShootingMode.Single)
            {
                if (Input.GetMouseButtonDown(0))
                    Shoot();
            }

            if (mode == ShootingMode.Burst)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < bulletsInBurst; i++)
                        Shoot();
                }
            }
        }

        CheckAmmo();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void CheckAmmo()
    {
        if (bulletsLeft <= 0)
            magazineIsNotEmpty = false;
        else
            magazineIsNotEmpty = true;
    }

    private void Reload()
    {
        if (weaponModel == WeaponModel.AKM && AmmoManager.Instance.totalAKMAmmo > 0)
        {
            int neededAmmo = magazineSize - bulletsLeft;
            int actualAmmo = AmmoManager.Instance.totalAKMAmmo >= neededAmmo ? neededAmmo : AmmoManager.Instance.totalAKMAmmo;
            bulletsLeft += actualAmmo;
            AmmoManager.Instance.totalAKMAmmo -= actualAmmo;
        }

        if (weaponModel == WeaponModel.Shotgun && AmmoManager.Instance.totalShotgunAmmo > 0)
        {
            int neededAmmo = magazineSize - bulletsLeft;
            int actualAmmo = AmmoManager.Instance.totalShotgunAmmo >= neededAmmo ? neededAmmo : AmmoManager.Instance.totalShotgunAmmo;
            bulletsLeft += actualAmmo;
            AmmoManager.Instance.totalShotgunAmmo -= actualAmmo;
        }
    }

    private void Shoot()
    {
        //���������� ����������� ������ ���� 
        Vector3 shootingDirection = CalculateShootingDirection();

        //������ ���� � ���� ������ (bulletSpawn)
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(shootingDirection));

        //������������ ����
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        Camera_Recoil_Script.doCameraRecoil();
        Weapon_Recoil_Script.doWeaponRecoil();

        //����� ������ ��������� ��-�� WeaponSway � �������� ������������� 
        /*muzzleFlashEffect.transform.rotation = Quaternion.identity;
        muzzleFlashEffect.Play();*/

        //������ ������ �������� �� ����� ������ ����. ������ ������ �������� �������� , ����� � ���� �� ����������� �������� WeaponSway.
        //�� ����������� ������ �������� �������� ����� ��� ������� ������ ����� � ������
        ParticleSystem _muzzleFlashEffect = Instantiate(GlobalReferences.Instance.muzzleFlashEffect, bulletSpawn.position, Quaternion.identity);
        _muzzleFlashEffect.transform.parent = bulletSpawn;
        _muzzleFlashEffect.Play();
        Destroy(_muzzleFlashEffect.gameObject, _muzzleFlashEffect.duration); // ���������� ������, � �� ������ ���������

        nextTimeToShoot = Time.time + delay;

        //�������� ������������ ���� ���� � ������(�������������)
        foreach (var otherBullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Physics.IgnoreCollision(
                bullet.GetComponent<Collider>(),
                otherBullet.GetComponent<Collider>()
            );
        }

        bulletsLeft--;
    }


    private Vector3 CalculateShootingDirection()
    {
        Vector3 shootingDirection;
        Vector3 collisionPoint;

        //������ ��� �� ������ ������
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f,0));
        RaycastHit hit;
        //���� ��� �������� � ������ �������� ���� ���������,
        //�� ���� ����� � ����� ������������ � ���� ��������
        if(Physics.Raycast(ray,out hit))
        {
            collisionPoint = hit.point;
        }
        else
        {
            //���� ��� �� �� ��� �� ��������,
            //�� ���� ����� � ����� �� ���������� 100 �� �� ������ ������ (bulletSpawn)
            collisionPoint = ray.GetPoint(100);
        }

        //������ �������
        float spreadX = Random.Range(-spreadRangeX, spreadRangeX);
        float spreadY = Random.Range(-spreadRangeY, spreadRangeY);

        Vector3 right = Camera.main.transform.right;
        Vector3 up = Camera.main.transform.up;

        collisionPoint += right * spreadX + up * spreadY;

        shootingDirection = collisionPoint - bulletSpawn.position;

        return shootingDirection;
    }
}
