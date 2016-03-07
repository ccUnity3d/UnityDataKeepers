using System;
using System.Reflection;
using UnityEngine;

public class CharpSingleton<T> where T : class
{
    public static T _instance = null;

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
                    (_instance as CharpSingleton<T>).OnInit();
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