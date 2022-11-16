using System;
using System.Collections.Generic;

public class EventsTranslator
{
    private static Dictionary<string, Action> eventsList = new Dictionary<string, Action>();
    private static Dictionary<string, Action<ISendData>> eventsDataList = new Dictionary<string, Action<ISendData>>();

    public static void AddListener(string key, Action func)
    {
        if (!eventsList.ContainsKey(key))
        {
            eventsList.Add(key, func);
        }
        else
        {
            eventsList[key] += func;
        }
    }

    public static void AddListener(string key, Action<ISendData> func)
    {
        if (!eventsList.ContainsKey(key))
        {
            eventsDataList.Add(key, func);
        }
        else
        {
            eventsDataList[key] += func;
        }
    }

    public static void RemoveAllListeners()
    {
        eventsList.Clear();
        eventsDataList.Clear();
    }

    public static void Call(string key)
    {
        foreach (var item in eventsList)
        {
            if (item.Key == key)
            {
                item.Value.Invoke();
            }
        }
    }

    public static void Call(string key, ISendData data)
    {
        foreach (var item in eventsDataList)
        {
            if (item.Key == key)
            {
                item.Value.Invoke(data);
            }
        }
    }

    public static string GetKey(string type, string tag = "")
    {
        return type + tag;
    }
}

public interface ISendData { }

