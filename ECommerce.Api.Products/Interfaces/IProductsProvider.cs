using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool Isuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool Isuccess, Product Product, string ErrorMessage)> GetProductAsync(int ID);
    }
}
