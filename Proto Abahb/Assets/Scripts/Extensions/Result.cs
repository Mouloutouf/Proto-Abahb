using System;

namespace Extensions
{
    public class Result
    {
        public Action OnPerformed;
        public Action OnCanceled;
        public Result(Action onPerformed, Action onCanceled)
        {
            OnPerformed = onPerformed;
            OnCanceled = onCanceled;
        }

        public void Perform()
        {
            OnPerformed.Invoke();
        }

        public void Cancel()
        {
            OnCanceled.Invoke();
        }
    }
    
    public class Result<T>
    {
        public Action<T> OnPerformed;
        public Action OnCanceled;
        public Result(Action<T> onPerformed, Action onCanceled)
        {
            OnPerformed = onPerformed;
            OnCanceled = onCanceled;
        }

        public void Perform(T arg)
        {
            OnPerformed.Invoke(arg);
        }

        public void Cancel()
        {
            OnCanceled.Invoke();
        }
    }
    
    public class Result<T,C>
    {
        public Action<T> OnPerformed;
        public Action<C> OnCanceled;
        public Result(Action<T> onPerformed, Action<C> onCanceled)
        {
            OnPerformed = onPerformed;
            OnCanceled = onCanceled;
        }

        public void Perform(T arg)
        {
            OnPerformed.Invoke(arg);
        }

        public void Cancel(C arg)
        {
            OnCanceled.Invoke(arg);
        }
    }
}