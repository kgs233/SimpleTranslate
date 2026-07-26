// Source - https://stackoverflow.com/a/79985957
// Posted by DebuggingByCoffee
// Retrieved 2026-07-27, License - CC BY-SA 4.0

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Microsoft.UI.Dispatching;

namespace SimpleTranslate;

public sealed class DispatcherQueueScheduler(DispatcherQueue dispatcherQueue) : IScheduler
{
    private readonly DispatcherQueue _dispatcherQueue = dispatcherQueue;

    public DateTimeOffset Now => DateTimeOffset.Now;

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        => Schedule(state, TimeSpan.Zero, (s, st) => action(s, st));

    public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        => Schedule(state, DateTimeOffset.Now + dueTime, action);

    public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        var scheduleId = new SingleAssignmentDisposable();
        var delay = dueTime - DateTimeOffset.Now;
        if (delay <= TimeSpan.Zero)
        {
            var posted = _dispatcherQueue.TryEnqueue(() =>
            {
                if (!scheduleId.IsDisposed)
                    scheduleId.Disposable = action(this, state);
            });
            if (!posted)
                scheduleId.Disposable = Disposable.Empty;
        }
        else
        {
            var timer = new Timer(_ =>
            {
                _dispatcherQueue.TryEnqueue(() =>
                {
                    if (!scheduleId.IsDisposed)
                        scheduleId.Disposable = action(this, state);
                });
            }, null, (long)delay.TotalMilliseconds, Timeout.Infinite);
            scheduleId.Disposable = Disposable.Create(() => timer.Dispose());
        }
        return scheduleId;
    }
}
