using System;
using System.Reflection;
using UnityEngine;

namespace DataKeepers
{
    public class CharpSingleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T) typeof(T).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null,
                        new Type[0],
                        new ParameterModifier[0]).Invoke(null);

                    try
                    {
                        var charpSingleton = _instance as CharpSingleton<T>;
                        if (charpSingleton != null) charpSingleton.OnInit();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }

                return _instance;
            }
        }

        protected virtual void OnInit() { }
    }
}