namespace WooshiiAttributes
{
    /// <summary>
    /// The base GUI class for custom drawer types.
    /// </summary>
    public abstract class GUIDrawerBase
    {
        public virtual void Initialise() { }

        public abstract void OnGUI();
    }

    /// <summary>
    /// The base GUI class for custom drawer types.
    /// </summary>
    public abstract class GUIDrawerBase<T> : GUIDrawerBase
    {
        /// <summary>
        /// The data tied to this drawer.
        /// </summary>
        protected T _data;

        public GUIDrawerBase(T data)
        {
            _data = data;
        }
    }
}