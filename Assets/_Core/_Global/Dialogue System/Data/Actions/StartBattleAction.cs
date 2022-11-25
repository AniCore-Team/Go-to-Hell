﻿using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Actions/StartBattle", fileName = "StartBattle", order = 52)]
public class StartBattleAction : DialogueAction
{
    public override void BeginAction(LevelDialogueWindow entity)
    {
        base.BeginAction(entity);
        LocationEvents.SendEvent(LocationEventType.BATTLE);
    }
}
