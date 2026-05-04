using System;
using System.Collections.Generic;

public static class ObserverManager
{
    private static Dictionary<GameEventID, Action<object>> eventTable =
        new Dictionary<GameEventID, Action<object>>();

    // Subscribe
    public static void Subscribe(GameEventID eventID, Action<object> callback)
    {
        if (!eventTable.ContainsKey(eventID))
            eventTable[eventID] = delegate { };

        eventTable[eventID] += callback;
    }

    // Unsubscribe
    public static void Unsubscribe(GameEventID eventID, Action<object> callback)
    {
        if (eventTable.ContainsKey(eventID))
            eventTable[eventID] -= callback;
    }

    // Publish
    public static void Invoke(GameEventID eventID, object param = null)
    {
        if (eventTable.ContainsKey(eventID))
            eventTable[eventID]?.Invoke(param);
    }
}

