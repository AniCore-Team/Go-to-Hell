using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Decisions/CheckEnemyDie", fileName = "CheckEnemyDie", order = 52)]
public class CheckEnemyDie : BattleDecisions
{
    private enum Character
    {
        Player,
        Enemy
    }

    [SerializeField] private Character character;

    public override bool GetDecision(BattleManager entity)
    {
        return character switch
        {
            Character.Enemy => entity.GetEnemyStateData().enemy.IsDie,
            Character.Player => entity.GetEnemyStateData().player.IsDie,
            _ => true
        };
    }
}
