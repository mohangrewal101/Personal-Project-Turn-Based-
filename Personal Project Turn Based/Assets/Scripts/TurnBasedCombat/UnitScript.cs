
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    [HideInInspector]
    public int buffTurnsLeft;
    [HideInInspector]
    public float unitMagic;
    [HideInInspector]
    public float unitDefenseBuff;
    [HideInInspector]
    public float unitAttackBuff;

    public string unitName;
    public int unitLevel;
    public GameObject rangedAttackPrefab;
    public Transform firePoint;

    public int damage;
    public int numOfTilesToMoveOn;

    public float speed;

    public float maxHP;
    public float currentHP;

    public float meleeRange;
    public float magicRange;


    private void Update()
    {
    }

    public bool TakeDamage(float dmg)
    {
        currentHP = currentHP - dmg;
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

    public void DefenseBuff(float amount, int numofBuffTurns)
    {
        unitDefenseBuff = amount;
        buffTurnsLeft = numofBuffTurns;
    }

}
