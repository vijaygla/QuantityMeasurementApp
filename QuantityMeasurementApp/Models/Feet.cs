namespace QuantityMeasurementApp.Models
{
    public class Feet
    {
        private readonly double _value;

        public Feet(double value)
        {
            _value = value;
        }

        public double Value => _value;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null)
                return false;

            if (obj.GetType() != typeof(Feet))
                return false;

            Feet other = (Feet)obj;

            return _value.CompareTo(other._value) == 0;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
