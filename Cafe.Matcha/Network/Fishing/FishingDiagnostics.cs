namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.Utils;

    // Callers pass fixed event names, availability flags and counts, never player data or exception text.
    internal sealed class FishingDiagnostics
    {
        private readonly Action<char, string> sink;
        private readonly object sync = new object();
        private readonly Dictionary<string, string> states = new Dictionary<string, string>();
        public FishingDiagnostics(Action<char, string> sink = null)
        { this.sink = sink ?? ((level, message) => Log.Add(LogType.Fishing, level, message)); }
        public void Event(string message, char level = 'I')
        {
            try { sink(level, message); } catch { /* Logging cannot interrupt sampling or network handling. */ }
        }
        public void State(string topic, string state, char level = 'I')
        {
            lock (sync)
            {
                if (states.TryGetValue(topic, out var previous) && previous == state) return;
                states[topic] = state;
            }
            Event(topic + ": " + state, level);
        }
        public static string Error(Exception error)
        {
            if (error is OperationCanceledException) return "timeout-or-cancelled";
            if (error is FishingApiException api) return "http-" + api.StatusCode;
            if (error is HttpRequestException) return "network";
            if (error is Newtonsoft.Json.JsonException || error is InvalidDataException) return "invalid-data";
            if (error is UnauthorizedAccessException) return "access-denied";
            if (error is IOException) return "read-failed";
            if (error is System.ComponentModel.Win32Exception) return "process-access";
            return "unexpected";
        }
    }
}
