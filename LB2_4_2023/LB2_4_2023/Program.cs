using System;
using System.Collections.Generic;

// Клас події з пріоритетом
public class EventArgWithPriority : EventArgs
{
    public int Priority { get; set; }
    public string Message { get; set; }

    public EventArgWithPriority(int priority, string message)
    {
        Priority = priority;
        Message = message;
    }
}

// Клас підписника з пріоритетом
public class SubscriberWithPriority
{
    public string Name { get; set; }
    public int Priority { get; set; }

    public SubscriberWithPriority(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }

    public void OnEvent(object sender, EventArgWithPriority e)
    {
        if (e.Priority == Priority)
        {
            Console.WriteLine($"{Name} received event with priority {e.Priority}: {e.Message}");
        }
    }
}

// Клас видавця
public class PublisherWithPriority
{
    public event EventHandler<EventArgWithPriority> Event;

    public void PublishEvent(int priority, string message)
    {
        Event?.Invoke(this, new EventArgWithPriority(priority, message));
    }
}

// Головний клас програми
public class Program
{
    public static void Main()
    {
        PublisherWithPriority publisher = new PublisherWithPriority();
        List<SubscriberWithPriority> subscribers = new List<SubscriberWithPriority>();

        // Додати підписників з різними пріоритетами
        subscribers.Add(new SubscriberWithPriority("Subscriber 1 with priority 1", 1));
        subscribers.Add(new SubscriberWithPriority("Subscriber 2 with priority 2", 2));
        subscribers.Add(new SubscriberWithPriority("Subscriber 3 with priority 1", 1));
        subscribers.Add(new SubscriberWithPriority("Subscriber 4 with priority 3", 3));
        subscribers.Add(new SubscriberWithPriority("Subscriber 5 with priority 2", 2));

        // Підписати підписників на події
        foreach (var subscriber in subscribers)
        {
            publisher.Event += subscriber.OnEvent;
        }

        // Опублікувати події з різними пріоритетами
        publisher.PublishEvent(1, "Event with priority 1");
        publisher.PublishEvent(2, "Event with priority 2");
        publisher.PublishEvent(3, "Event with priority 3");
        publisher.PublishEvent(1, "Another event with priority 1");

        // Відписати підписників від подій
        publisher.Event -= subscribers[1].OnEvent;
        publisher.Event -= subscribers[3].OnEvent;

        // Опублікувати події з різними пріоритетами після відписки підписників
        publisher.PublishEvent(2, "Event with priority 2 after unsubscribing");
        publisher.PublishEvent(1, "Event with priority 1 after unsubscribing");

        Console.ReadLine();
    }
}
