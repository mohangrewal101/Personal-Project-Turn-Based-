using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicAttacks : MonoBehaviour
{
    private BattleSystem battleSystem;
    public Button magicAttackButton;


    private void Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
    }
    public void OnHealButton()
    {
        if (battleSystem.state != BattleState.PLAYERTURN)
        {
            return;
        }
       StartCoroutine(battleSystem.PlayerHeal());
    }

    public void OnFireButton()
    {
        if (battleSystem.state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(battleSystem.PlayerFire());
    }
}
