using UnityEngine;

public static class LogExtentions
{
    public static void Log(this string text)
    {
#if UNITY_ANDROID
#if !UNITY_EDITOR
        text = "\r\n\r\n" + text;
#endif
        text += "\r\n\r\n";
#endif
        Debug.Log(text);
    }

    public static void Log(this string text, params object[] objects)
    {
#if UNITY_ANDROID
#if !UNITY_EDITOR
        text = "\r\n\r\n" + text;
#endif
        text += "\r\n\r\n";
#endif
        Debug.Log(string.Format(text,objects));
    }
}
