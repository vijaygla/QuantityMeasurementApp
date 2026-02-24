namespace QuantityMeasurementApp.Models
{
    public class Quantity
    {
        public double Value { get; set; }
        public Quantity(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Quantity)) return false;
            Quantity other = (Quantity)obj;
            return Value == other.Value;
        }
    }
}
