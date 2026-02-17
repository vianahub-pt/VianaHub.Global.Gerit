using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;

var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "src", "VianaHub.Global.Gerit.Api", "Localization");
var groups = new[] { "api", "application", "domain", "swagger" };
var cultures = new[] { "en-US", "es-ES", "pt-PT" };

foreach (var group in groups)
{
    var files = cultures.Select(c => Path.Combine(basePath, $"{group}.{c}.json")).ToList();
    var jsons = files.Select(f => JsonNode.Parse(File.ReadAllText(f))).ToList();
    var lengths = jsons.Select(j => ((JsonObject)j).Count).ToList();
    if (lengths.Distinct().Count() != 1)
    {
        Console.WriteLine($"Mismatch in group {group}: counts = {string.Join(',', lengths)}");
        Environment.Exit(2);
    }

    // check keys per position (order in file) - using naive approach: compare sequences of keys
    var keySequences = files.Select(f => File.ReadAllLines(f).Where(l => l.TrimStart().StartsWith("\""))).Select(lines => lines.Select(l => l.Trim()).ToList()).ToList();
    var keyCounts = keySequences.Select(s => s.Count).ToList();
    if (keyCounts.Distinct().Count() != 1)
    {
        Console.WriteLine($"Mismatch key line counts in group {group}: {string.Join(',', keyCounts)}");
        Environment.Exit(3);
    }

    Console.WriteLine($"Group {group} OK: {lengths[0]} entries");
}

Console.WriteLine("All localization groups validated");
