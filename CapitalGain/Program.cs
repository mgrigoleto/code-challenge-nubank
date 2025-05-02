using System.Text.Json;

namespace project;

internal class Program
{
    static void Main(string[] args)
    {
        var lines = new List<string>();
        string? line;

        while ((line = Console.ReadLine()) != null && line != "")
        {
            lines.Add(line.Trim());
        }

        // Crio o json raiz, formando uma lista de listas
        string json = "[" + string.Join(",", lines) + "]";

        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                List<List<Operation>>? gains = JsonSerializer.Deserialize<List<List<Operation>>>(json);

                foreach (var operationBlock in gains)
                {
                    CapitalGain capitalGain = new CapitalGain();
                    if (operationBlock == null)
                    {
                        Console.WriteLine("[]");
                        return;
                    }
                    foreach (var operation in operationBlock)
                    {
                        capitalGain.AddOperation(operation);
                    }

                    Console.WriteLine(JsonSerializer.Serialize(operationBlock.Select(x => new { tax = Math.Round(x.Tax ?? 0.0, 1) })));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao processar JSON: " + ex.Message);
            }
            
        }
    }
}

