using System;

namespace AgOpenGPS.Core.Base
{
    // Base class that makes implementing the Dispose pattern easy.
    public abstract class DisposableObject : IDisposable
    {
        protected bool IsDisposed { get; private set; }

        protected DisposableObject()
        {
        }

        ~DisposableObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {
            // Subclasses only need to override this method to implement the Dispose pattern
            // DisposableObject takes care of the rest.
        }

        private void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                OnDispose();
            }

            IsDisposed = true;
        }
    }
}
