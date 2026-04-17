using QuantityService.Models;
using QuantityService.Exceptions;
using System;

namespace QuantityService.Repository
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityDbContext _context;

        public QuantityMeasurementDatabaseRepository(QuantityDbContext context)
        {
            _context = context;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            try
            {
                _context.QuantityMeasurements.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Database error while saving measurement using EF Core", ex);
            }
        }
    }
}
