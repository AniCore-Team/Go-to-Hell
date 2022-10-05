using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Effect
{
    private CardID id;
    private int duration;
    private TargetEffect target;

    public void PowerUp(CardProperty newCard)
    {

    }
}

public class CardEffectsController
{
    private Dictionary<CardID, Effect> effects = new Dictionary<CardID, Effect>();
    
    public void AddEffect(CardProperty newCard)
    {
        if (newCard.duration > 0)
            if (effects.ContainsKey(newCard.id))
                effects[newCard.id].PowerUp(newCard);
            else
                effects.Add(newCard.id, new Effect());


    }

    public void Tick()
    {

    }

    private void DropEffect(CardProperty dropCard)
    {

    }
}
