using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Matcha.Network.Fishing;
internal static class DiagnosticsTests
{
    internal static void Run()
    {
        var messages = new List<string>();
        var log = new FishingDiagnostics((level, text) => messages.Add(text));
        for (int i = 0; i < 100; i++) log.State("memory", "unavailable", 'W');
        if (messages.Count != 1) throw new Exception("deduplicate sampling state");
        log.State("memory", "ready");
        if (messages.Count != 2) throw new Exception("report recovery");
        var broken = new FishingDiagnostics((_, __) => throw new Exception("synthetic-private-text"));
        broken.Event("test"); broken.State("memory", "ready");
        using (var reader = new FishingMemoryReader(() => throw new Exception("synthetic-private-text"), () => null, () => 1000, log))
        {
            if (reader.Read().Gathering != null) throw new Exception("read failure must return unknown");
        }
        if (messages.Any(m => m.Contains("synthetic-private-text"))) throw new Exception("exception text leaked");
        if (FishingDiagnostics.Error(new FishingApiException(TimeSpan.FromSeconds(60), 429)) != "http-429") throw new Exception("HTTP category");
    }
}
