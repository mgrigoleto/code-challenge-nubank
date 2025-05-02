namespace project;
public class CapitalGain
{
    public List<Operation> Operations { get; set; } = new List<Operation>();
    public int CurrentTotalQuantity { get; set; } = 0;
    public double WeightedAveragePrice { get; set; } = 0;
    public double AccumulatedLoss { get; set; } = 0;
    public CapitalGain() { }

    public void AddOperation(Operation operation)
    {
        if (operation != null)
        {
            Operations.Add(operation);
            if(operation.OperationType == "buy")
            {
                UpdateWeightedAveragePrice(operation.Quantity, operation.UnitCost);
                UpdateCurrentTotalQuantity(operation.Quantity);

            } else if (operation.OperationType == "sell")
            {
                CalculateTaxes(operation);
                UpdateCurrentTotalQuantity(-operation.Quantity);
            }
        }
    }

    public void UpdateCurrentTotalQuantity(int quantity)
    {
        CurrentTotalQuantity += quantity;
    }

    public void UpdateWeightedAveragePrice(int incomingQuantity, double unitCost)
    {
        WeightedAveragePrice = Math.Round(((CurrentTotalQuantity * WeightedAveragePrice) + (incomingQuantity * unitCost)) / (CurrentTotalQuantity + incomingQuantity),2);
    }

    public void CalculateTaxes(Operation operation)
    {
        // CONSIDERAÇÕES:
        // Uso a média ponderada para saber se estou tendo lucro ou prejuízo ao vender as ações.
        // O percentual de imposto pago é de 20% sobre o lucro obtido na operação, mas não paga imposto quando o total da operação for menor ou igual a R$ 20000,00.
        // Se eu estiver no prejuízo, devo usar o prejuízo passado para deduzir múltiplos lucros futuros, até que todo prejuízo seja deduzido.

        double operationTotal = operation.UnitCost * operation.Quantity;
        double costBasis = WeightedAveragePrice * operation.Quantity;
        double profit = operationTotal - costBasis;

        if (profit > 0)
        {
            if (operationTotal <= 20000)
            {
                operation.Tax = 0;
            }
            else
            {
                // Uso o prejuízo acumulado para deduzir o lucro
                double taxableProfit = profit - AccumulatedLoss;

                // Se eu ainda estiver no prejuízo, não pago imposto e atualizo o prejuízo acumulado
                if (taxableProfit <= 0)
                {
                    operation.Tax = 0;
                    AccumulatedLoss -= profit;
                }

                // Se eu saí do prejuízo, pago imposto sobre o lucro taxável
                else
                {
                    operation.Tax = Math.Round(taxableProfit * 0.20, 2);
                    AccumulatedLoss = 0;
                }
            }
        }
        else
        {
            // Fiquei no prejuízo, então atualizo o prejuízo acumulado. Nesse caso não tem imposto
            AccumulatedLoss += -profit;
            operation.Tax = 0;
        }
    }
}


