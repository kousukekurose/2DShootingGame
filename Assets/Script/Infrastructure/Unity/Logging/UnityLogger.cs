using System.Diagnostics;


namespace Project.Infrastructure.Unity.Logging
{
    public static class UnityLogger
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
