using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDeck
{
    private List<DeckSlot> slots = new List<DeckSlot>();

    public List<DeckSlot> Slots => slots;

    public void AddCardToDeck(CardProperty card)
    {
        foreach (var item in slots)
        {
            if(item.card == card)
            {
                item.LevelUp();
                return;
            }
        }

        slots.Add(new DeckSlot(card, 1));
    }
}

public struct DeckSlot
{
    public CardProperty card;
    public int level;

    public DeckSlot(CardProperty card, int level)
    {
        this.card = card;
        this.level = level;
    }

    public void LevelUp()
    {
        if (level >= card.max_level) return;

        level++;
    }
}