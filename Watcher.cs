namespace EFPerformance;

public static class Watcher
{
    static Dictionary<string, IDataSource> sources = new();

    static Dictionary<string, List<double>> results = new();

    public static int CurrentRound = 1;

    public static void AddDataSource(string key, IDataSource source)
    {
        sources.Add(key, source);
        results.Add(key, new());
    }

    public static void RunRound()
    {
        foreach (var source in sources)
        {
            var result = Run(source.Key);

            results[source.Key].Add(result);
        }
    }

    public static void ClearResult()
    {
        foreach (var result in results)
        {
            result.Value.Clear();
        }
    }

    public static void PrintResult()
    {
        var result = new Dictionary<string, double>();

        foreach (var source in sources)
        {
            result.Add(source.Key, results[source.Key].Average());
        }

        Console.WriteLine("=========Benchark Result=========");
        Console.WriteLine($"每次读取数据量: {Config.TestCount} 条");
        Console.WriteLine($"测试轮数: {Config.TestRounds} 次");

        foreach (var keyValue in result.OrderBy(kv => kv.Value))
        {
            Console.WriteLine($"{keyValue.Key} : {keyValue.Value.ToString("N3")}ms");
        }
    }

    private static double Run(string key)
    {
        Console.Write($"{key}....");
        var start = DateTime.Now;

        sources[key].GetCustomers();

        var end = DateTime.Now;
        var result = (end - start).TotalMilliseconds;

        Console.Write($" done in {result}ms\n");

        return result;
    }
}
