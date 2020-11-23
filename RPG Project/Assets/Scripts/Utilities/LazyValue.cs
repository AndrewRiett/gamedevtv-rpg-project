using UnityEngine;

namespace RPG.Utilities
{

    public class LazyValue<T>
    {
        public delegate T InitializerDelegate();
        private T _value;
        private bool isInitialized = false;
        private InitializerDelegate initializer;

        public LazyValue(InitializerDelegate initializer) // class constructor
        {
            this.initializer = initializer;
        }

        public T value // T value property
        {
            get 
            {
                ForceInit();
                return _value;
            }
            set 
            {
                isInitialized = true;
                _value = value; // note: the value here means the given value, bot a property itself
            }
        }

        public void ForceInit()
        {
            if (!isInitialized)
            {
                value = initializer(); // executes a method from a delegate
                isInitialized = true;
            }
        }

        
    }
}