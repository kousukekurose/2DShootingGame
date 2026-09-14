using System.Diagnostics;


namespace Framework.Core
{
    public static class CustomLogger
    {
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
