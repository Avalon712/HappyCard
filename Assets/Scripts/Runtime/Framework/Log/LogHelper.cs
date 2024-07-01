using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HappyCard
{
    public static class LogHelper
    {
        private const string LOG_FILE = "happycard_debug.log";

        public static void Info(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif

        }

        public static void Warn(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif

        }

        public static void Error(string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif

        }
    }
}
