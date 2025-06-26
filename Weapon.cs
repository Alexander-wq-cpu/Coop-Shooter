using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletVelocity;
    [SerializeField] private ShootingMode mode;
    [SerializeField] private bool canShoot;

    [Range(0.1f,1f)]
    [SerializeField] private float delay = 0.25f;
    private float nextTimeToShoot = 0;

    [Range(0,2)]
    [SerializeField] private float spreadRangeX = 1;
    [Range(0, 2)]
    [SerializeField] private float spreadRangeY = 1;

    [SerializeField] private int bulletsInBurst = 1;

    public Recoil Camera_Recoil_Script;
    public WeaponRecoil Weapon_Recoil_Script;
    public enum ShootingMode
    {
        Automatic,
        Single,
        Burst
    }


    void Update()
    {
        canShoot = Time.time >= nextTimeToShoot;

        if (canShoot)
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
