
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public GameObject rangedAttackPrefab;
    public Transform firePoint;

    public int damage;

    public float maxHP;
    public float currentHP;

    public float meleeRange;
    public float magicRange;

    public bool TakeDamage(float dmg)
    {
        currentHP -= dmg;
        return currentHP <= 0;
    }

    public void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > 100)
        {
            currentHP = 100;
        }
    }
}
