using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    [SerializeField] private TextMeshProUGUI healthUI;
    private void Start()
    {
        healthUI.text = $"Health: {health}";
    }
    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            //DEATH
        }
        else
        {
            healthUI.text = $"Health: {health}";
            //Добавить звук урона
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ZombieHand"))
        {
            TakeDamage(Enemy.enemyDamage);
        }
    }
}
