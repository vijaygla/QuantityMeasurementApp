using System.Collections.Generic;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly List<QuantityMeasurementEntity> cache = new();

        public void Save(QuantityMeasurementEntity entity)
        {
            cache.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return cache;
        }
    }
}
