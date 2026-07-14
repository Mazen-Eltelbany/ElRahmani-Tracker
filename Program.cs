using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Threading.Tasks;

class CFTracker
{
    static readonly HttpClient client = new HttpClient();
    private static List<SolvedProblem> _solved = new List<SolvedProblem>();

    public static void Details(List<SolvedProblem>sol,List<SolvedProblem>sol2,List<SolvedProblem>sol3)
    {
       
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n(1)Show Eltelbany Problems:");
            Console.WriteLine("(2)Show ElAbiad Problems:");
            Console.WriteLine("(3)Show ElAmin Problems:");
            Console.WriteLine("(4)Exit");
            Console.Write("Enter Your Choice:");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }
            switch (choice)
            {
                case 1:
                    foreach (var p in sol)
                        Console.WriteLine($"{p.SolvedAt:yyyy-MM-dd}  {p.Key,-8} {p.Name} ({p.Rating})");
                    break;
                case 2:
                    foreach (var p in sol2)
                        Console.WriteLine($"{p.SolvedAt:yyyy-MM-dd}  {p.Key,-8} {p.Name} ({p.Rating})");
                    break;
                case 3:
                    foreach (var p in sol3)
                        Console.WriteLine($"{p.SolvedAt:yyyy-MM-dd}  {p.Key,-8} {p.Name} ({p.Rating})");
                    break;
                case 4:
                    running = false; break;
                default:
                    Console.WriteLine("Choose Between 1 -> 4 ");
                    break;
            }
        }
    }
    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.Title = "ElRahmani Codeforces Tracker";
        client.BaseAddress = new Uri("https://codeforces.com/");
        await GetAllSolvedProblems("Mazen_Eltelbany");
        var oneWeekAgo = DateTimeOffset.UtcNow.AddDays(-7);
        var sol =_solved.Where(X=>X.SolvedAt>=oneWeekAgo).ToList();
        var rate800=sol.Where(x=>x.Rating==800).ToList();
        var rateabove800=sol.Where(x=>x.Rating>800).ToList();

        await GetAllSolvedProblems("El-Abiad");
        var sol2 =_solved.Where(X=>X.SolvedAt>=oneWeekAgo).ToList();
        var rate8002 = sol2.Where(x => x.Rating == 800).ToList();
        var rateabove8002 = sol2.Where(x => x.Rating > 800).ToList();


        await GetAllSolvedProblems("ElAmino");
        var sol3 =_solved.Where(X=>X.SolvedAt>=oneWeekAgo).ToList();
        var rate8003 = sol3.Where(x => x.Rating == 800).ToList();
        var rateabove8003 = sol3.Where(x => x.Rating > 800).ToList();

        Console.WriteLine("================================================================");
        Console.WriteLine($"    ElRahmani Team Member Stats From:{oneWeekAgo.ToString("yyyy:MM:dd")} to {DateTime.Now.ToString("yyyy:MM:dd")}");
        Console.WriteLine("================================================================");
        Console.WriteLine($"ElTelbany solved: {sol.Count} Problems |Rate 800: {rate800.Count}  Problems   |Rate>800:{rateabove800.Count} Problems");
        Console.WriteLine($"ElAbiad   solved: {sol2.Count} Problems |Rate 800: {rate8002.Count} Problems     |Rate>800:{rateabove8002.Count} Problems");
        Console.WriteLine($"ElAmino   solved: {sol3.Count} Problems |Rate 800: {rate8003.Count} Problems    |Rate>800:{rateabove8003.Count} Problems\n");


        Details(sol,sol2,sol3);
    }

    public class SolvedProblem
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public long SolvedAtUnix { get; set; }
        public DateTimeOffset SolvedAt => DateTimeOffset.FromUnixTimeSeconds(SolvedAtUnix).ToLocalTime();
    }

    public static async Task GetAllSolvedProblems(string handle)
    {
        try
        {
            var response = await client.GetAsync($"api/user.status?handle={handle}");
            response.EnsureSuccessStatusCode();
            string json=await response.Content.ReadAsStringAsync();
            using var doc=JsonDocument.Parse(json);
            var docroot = doc.RootElement;
            if (docroot.GetProperty("status").ToString() != "OK")
            {
                Console.WriteLine("API returned an error.");
                return;
            }
            var submissions = docroot.GetProperty("result");
            var seen = new Dictionary<string, SolvedProblem>();
            foreach(var sub in submissions.EnumerateArray())
            {
                string verdict = sub.TryGetProperty("verdict", out var v) ? v.ToString() : null;
                if (verdict != "OK") continue;
                var problem = sub.GetProperty("problem");
                var name=problem.GetProperty("name").GetString();
                var index = problem.GetProperty("index").GetString();
                string contestid = problem.TryGetProperty("contestId", out var cid) ? cid.ToString() : "Gym";
                string key = contestid + index;
                long time = sub.GetProperty("creationTimeSeconds").GetInt64();
                int rating = problem.TryGetProperty("rating", out var r) ? r.GetInt32():0;
                if (!seen.ContainsKey(key) || seen[key].SolvedAtUnix>time)
                {
                    seen[key] = new SolvedProblem
                    {
                        Key = key,
                        Rating = rating,
                        Name=name,
                        SolvedAtUnix = time
                    };
                }
                _solved=seen.Values.OrderBy(p=>p.SolvedAtUnix).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
