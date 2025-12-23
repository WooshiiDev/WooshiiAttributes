namespace WooshiiAttributes
{
    public abstract class GUIDrawerBase
    {
        public virtual void Initialise() { }
        public abstract void OnGUI();
    }

    public abstract class GUIDrawerBase<T> : GUIDrawerBase
    {
        /// <summary>
        /// The target object to draw.
        /// </summary>
        protected T _target;

        public GUIDrawerBase(T target)
        {
            this._target = target;
        }
    }
}