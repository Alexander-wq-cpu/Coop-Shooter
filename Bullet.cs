using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLivingTime = 1f;

    [HideInInspector]
    public int bulletDamage;
    private void Start()
    {
        Destroy(gameObject, bulletLivingTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            CreateBulletHoleEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Enemy enemy = collision.transform.root.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    private void CreateBulletHoleEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletHoleEffect, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(collision.transform);
    }
}
