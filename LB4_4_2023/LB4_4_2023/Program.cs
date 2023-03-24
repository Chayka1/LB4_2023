using System;

namespace EventDrivenWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting workflow...");

            // Create instances of the classes representing the different actions in the workflow
            var action1 = new Action1();
            var action2 = new Action2();
            var action3 = new Action3();

            // Subscribe to the events triggered by each action
            action1.ActionCompleted += action2.Start;
            action2.ActionCompleted += action3.Start;

            // Start the workflow by calling the Start method of the first action
            action1.Start();

            Console.WriteLine("Workflow completed.");
            Console.ReadLine();
        }
    }

    // Base class representing an action in the workflow
    public abstract class WorkflowAction
    {
        public event EventHandler ActionCompleted;

        protected void OnActionCompleted()
        {
            ActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Start();
    }

    // Example action 1
    public class Action1 : WorkflowAction
    {
        public override void Start()
        {
            Console.WriteLine("Action 1 started...");
            // Simulate some work being done
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Action 1 completed.");
            OnActionCompleted();
        }
    }

    // Example action 2
    public class Action2 : WorkflowAction
    {
        public override void Start()
        {
            Console.WriteLine("Action 2 started...");
            // Simulate some work being done
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Action 2 completed.");
            OnActionCompleted();
        }
    }

    // Example action 3
    public class Action3 : WorkflowAction
    {
        public override void Start()
        {
            Console.WriteLine("Action 3 started...");
            // Simulate some work being done
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Action 3 completed.");
            OnActionCompleted();
        }
    }
}
