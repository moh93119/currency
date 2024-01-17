namespace Currency.BusinessLogic.Contract;

public interface ICurrencyConverter
{
    void ClearConfiguration();
    void UpdateConfiguration(IEnumerable<(string, string, double)> conversionRates);
    double Convert(string fromCurrency, string toCurrency, double amount);
}