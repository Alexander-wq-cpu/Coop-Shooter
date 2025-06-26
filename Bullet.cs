using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLivingTime = 1f;
    private void Start()
    {
        Destroy(gameObject, bulletLivingTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            createBulletHoleEffect(collision);
        }
    }

    private void createBulletHoleEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletHoleEffect, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(collision.transform);
    }
}
