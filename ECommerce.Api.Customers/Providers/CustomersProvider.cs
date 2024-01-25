using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        public readonly CustomersDbContext _dbContext;
        public readonly ILogger<CustomersProvider> _logger;
        public readonly IMapper _mapper;
        public CustomersProvider(CustomersDbContext DbContext, ILogger<CustomersProvider> Logger, IMapper mapper)
        {
            _dbContext = DbContext;
            _logger = Logger;  
            _mapper = mapper;
            SeedData();
        }
        private void SeedData()
        {
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "John", Address = "Address1", Phone = "123456789" });
                _dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "James", Address = "Address2", Phone = "123456789" });
                _dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Jameel", Address = "Address3", Phone = "123456789" });
                _dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Johnson", Address = "Address45", Phone = "123456789" });
                _dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int ID)
        {
            try
            {
                var Customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == ID);
                if (Customer != null )
                {
                    var result = _mapper.Map<Db.Customer, Models.Customer>(Customer);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var Customers = await _dbContext.Customers.ToListAsync();
                if (Customers != null && Customers.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(Customers);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
        }
    }
}
