using System;
using System.Threading;

public class RetryPolicy
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _initialDelay;
    private readonly TimeSpan _maxDelay;

    public RetryPolicy(int maxAttempts, TimeSpan initialDelay, TimeSpan maxDelay)
    {
        _maxAttempts = maxAttempts;
        _initialDelay = initialDelay;
        _maxDelay = maxDelay;
    }

    public void Execute(Action action)
    {
        int attempts = 0;
        TimeSpan delay = _initialDelay;

        while (true)
        {
            try
            {
                action();
                break;
            }
            catch (Exception ex)
            {
                attempts++;

                if (attempts >= _maxAttempts)
                {
                    throw new Exception($"Failed after {_maxAttempts} attempts.", ex);
                }

                int randomDelay = new Random().Next((int)(delay.TotalMilliseconds * 0.8), (int)(delay.TotalMilliseconds * 1.2));
                delay = TimeSpan.FromMilliseconds(randomDelay);

                Console.WriteLine($"Attempt {attempts} failed. Retrying in {delay.TotalSeconds} seconds.");

                Thread.Sleep(delay);

                // Increase delay for next attempt
                delay = TimeSpan.FromTicks((long)(delay.Ticks * 1.5));
                if (delay > _maxDelay)
                {
                    delay = _maxDelay;
                }
            }
        }
    }
}
