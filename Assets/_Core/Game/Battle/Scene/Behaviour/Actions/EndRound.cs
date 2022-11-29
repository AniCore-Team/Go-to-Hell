using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/EndRound", fileName = "EndRound", order = 52)]
public class EndRound : BattleAction
{
    private enum State
    {
        Win,
        Lose
    }

    [SerializeField] private State state;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        switch (state)
        {
            case State.Win:
                break;
            case State.Lose:
                break;
            default:
                break;
        }
    }
}
