using QuantityService.Models;

namespace QuantityService.Repository
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
    }
}
