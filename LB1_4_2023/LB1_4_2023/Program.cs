using System;
using System.Collections.Generic;
using System.Threading;

class EventBus
{
    private Dictionary<string, List<Delegate>> eventHandlers = new Dictionary<string, List<Delegate>>();
    private Dictionary<string, DateTime> lastEventTimes = new Dictionary<string, DateTime>();
    private int throttleInterval = 1000;

    public void Register(string eventName, Delegate handler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers.Add(eventName, new List<Delegate>());
        }
        eventHandlers[eventName].Add(handler);
    }

    public void Unregister(string eventName, Delegate handler)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName].Remove(handler);
        }
    }

    public void SendEvent(string eventName, object eventData)
    {
        if (lastEventTimes.ContainsKey(eventName))
        {
            DateTime lastTime = lastEventTimes[eventName];
            if ((DateTime.Now - lastTime).TotalMilliseconds < throttleInterval)
            {
                Console.WriteLine("Throttled event: {0}", eventName);
                return;
            }
        }
        if (eventHandlers.ContainsKey(eventName))
        {
            foreach (Delegate handler in eventHandlers[eventName])
            {
                handler.DynamicInvoke(eventData);
            }
        }
        lastEventTimes[eventName] = DateTime.Now;
    }
}

class Program
{
    static void Main(string[] args)
    {
        EventBus eventBus = new EventBus();
        eventBus.Register("buttonClick", new Action<object>(OnButtonClick));
        eventBus.Register("buttonClick", new Action<object>(OnButtonClickThrottled));
        eventBus.Register("mouseMove", new Action<object>(OnMouseMove));
        eventBus.SendEvent("buttonClick", "Button 1 clicked");
        Thread.Sleep(500);
        eventBus.SendEvent("buttonClick", "Button 2 clicked");
        Thread.Sleep(1500);
        eventBus.SendEvent("mouseMove", new { x = 10, y = 20 });
        Console.ReadLine();
    }

    static void OnButtonClick(object eventData)
    {
        Console.WriteLine("Button click event: {0}", eventData);
    }

    static void OnButtonClickThrottled(object eventData)
    {
        Console.WriteLine("Throttled button click event: {0}", eventData);
    }

    static void OnMouseMove(object eventData)
    {
        Console.WriteLine("Mouse move event: ({0}, {1})", ((dynamic)eventData).x, ((dynamic)eventData).y);
    }
}
