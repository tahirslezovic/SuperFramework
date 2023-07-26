namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each object which can be added to the specific pool.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Method called when this object is about to return back to the pool.
        /// For example, this is oportunity to disable gameObject.
        /// </summary>
        void OnReturn();

        /// <summary>
        /// Method called when this object is taken from pool.
        /// For example, this is oportunity to enable gameObject and set on specific position.
        /// </summary>
        void OnGet();

    }
}
