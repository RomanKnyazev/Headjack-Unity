using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Assets
{
    public static class LogUtil
    {
        public enum Severity
        {
            Info, Warn, Error, Assert
        }

        private static Dictionary<Severity, Action<string, object[]>> _loggersFormat = new Dictionary<Severity, Action<string, object[]>>
        {
            { Severity.Info, Debug.LogFormat },
            { Severity.Warn, Debug.LogWarningFormat },
            { Severity.Error, Debug.LogErrorFormat },
            { Severity.Assert, Debug.LogErrorFormat } // not supported
        };

        private static Color32 GetSeverityColor(Severity s)
        {
            switch (s)
            {
                case Severity.Info:
                    return new Color32(0x22, 0x33, 0x77, 0xff);
                case Severity.Warn:
                    return new Color32(0x99, 0x33, 0x22, 0xff);
                case Severity.Error:
                    return new Color32(0xFF, 0x55, 0x55, 0xff);
                case Severity.Assert:
                    return new Color32(0xAA, 0x22, 0x22, 0xff);
                default:
                    return Color.clear;
            }
        }

        public static void Log(Severity level, object sender, string format, params object[] args)
        {
            var decoratedFormat = string.Format(
                "<color=#{0}>[{1}] [{2}] {3}</color>",
                ColorUtility.ToHtmlStringRGB(GetSeverityColor(level)),
                sender == null ? "Unknown" : sender.GetType().ToString(), 
                DateTime.UtcNow.ToShortTimeString(), 
                format);
            _loggersFormat[level](decoratedFormat, args);
        }
    }
}