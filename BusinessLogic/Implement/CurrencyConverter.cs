using Currency.BusinessLogic.Contract;

namespace Currency.BusinessLogic.Implement;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly Dictionary<string, Dictionary<string, double>> _conversionGraph;

    public CurrencyConverter()
    {
        _conversionGraph = new Dictionary<string, Dictionary<string, double>>();
    }

    public void ClearConfiguration()
    {
        lock (_conversionGraph)
        {
            _conversionGraph.Clear();
        }
    }

    public void UpdateConfiguration(IEnumerable<(string, string, double)> conversionRates)
    {
        lock (_conversionGraph)
        {
            foreach (var rate in conversionRates)
            {
                AddConversionRate(rate.Item1, rate.Item2, rate.Item3);
            }
        }
    }

    public double Convert(string fromCurrency, string toCurrency, double amount)
    {
        lock (_conversionGraph)
        {
            if (!_conversionGraph.ContainsKey(fromCurrency) || !_conversionGraph.ContainsKey(toCurrency))
                throw new ArgumentException("Invalid currency codes");

            var visitedCurrencies = new HashSet<string>();
            var queue = new Queue<Tuple<string, double>>();
            queue.Enqueue(new Tuple<string, double>(fromCurrency, amount));

            while (queue.Count > 0)
            {
                var (currentCurrency, currentAmount) = queue.Dequeue();

                if (currentCurrency == toCurrency)
                    return currentAmount;

                visitedCurrencies.Add(currentCurrency);

                foreach (var (neighborCurrency, conversionRate) in _conversionGraph[currentCurrency])
                {
                    if (visitedCurrencies.Contains(neighborCurrency)) continue;

                    var newAmount = currentAmount * conversionRate;
                    queue.Enqueue(new Tuple<string, double>(neighborCurrency, newAmount));
                }
            }

            throw new ArgumentException("No conversion path found");
        }
    }

    private void AddConversionRate(string fromCurrency, string toCurrency, double rate)
    {
        if (!_conversionGraph.ContainsKey(fromCurrency))
            _conversionGraph[fromCurrency] = new Dictionary<string, double>();

        if (!_conversionGraph.ContainsKey(toCurrency))
            _conversionGraph[toCurrency] = new Dictionary<string, double>();

        _conversionGraph[fromCurrency][toCurrency] = rate;
        _conversionGraph[toCurrency][fromCurrency] = 1 / rate;
    }
}