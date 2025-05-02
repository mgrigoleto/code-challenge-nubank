using System.Text.Json.Serialization;

namespace project;
public class Operation
{
    [JsonPropertyName("operation")]
    public string OperationType { get; set; }

    [JsonPropertyName("unit-cost")]
    public double UnitCost { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    public double? Tax { get; set; } = 0;

    public Operation(string operationType, double unitCost, int quantity, double? tax) 
    {
        OperationType = operationType;
        UnitCost = unitCost;
        Quantity = quantity;
        Tax = tax;
    }

    public Operation() { }
}


