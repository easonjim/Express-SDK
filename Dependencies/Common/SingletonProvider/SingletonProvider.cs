namespace ComLib
{
    using System;

    public class SingletonProvider<T> where T: new()
    {
        private SingletonProvider()
        {
        }

        public static T Instance
        {
            get
            {
                return SingletonCreator.instance;
            }
        }

        private class SingletonCreator
        {
            internal static readonly T instance;

            static SingletonCreator()
            {
                SingletonProvider<T>.SingletonCreator.instance = new T();
            }
        }
    }
}

