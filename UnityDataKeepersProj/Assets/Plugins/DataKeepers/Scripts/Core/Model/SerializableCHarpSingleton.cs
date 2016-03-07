using System;
using System.Reflection;
using JsonDotNet.Extras.CustomConverters;
using Newtonsoft.Json;
using UnityEngine;

namespace DataKeepers
{
    [Serializable]
    public class SerializableCharpSingleton<T> : SerializableObject where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)typeof(T).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null,
                        new Type[0],
                        new ParameterModifier[0]).Invoke(null);

                    try
                    {
                        var serializableCharpSingleton = _instance as SerializableCharpSingleton<T>;
                        if (serializableCharpSingleton != null)
                            serializableCharpSingleton.OnInit();
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