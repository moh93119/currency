namespace Currency.Data.Entities;

public class ConversionRate : BaseModel
{
    public string FromCurrency { get; set; } = null!;
    public string ToCurrency { get; set; } = null!;
    public double Rate { get; set; }
}