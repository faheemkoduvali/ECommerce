using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchServices : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchServices(IOrderService orderService, IProductService productService, ICustomerService customerService ) 
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerID)
        {
            var customersResult = await customerService.GetCustomerAsync(customerID);
            var ordersResult = await orderService.GetOrdersAsync(customerID);
            var productsResult = await productService.GetProductsAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ? 
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product Information is not available";
                    }
                }


                var result = new
                {
                    Customers = customersResult.IsSuccess ? 
                        customersResult.Customer : 
                        new Models.Customer{ Name = "Customer not avaialble"},
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
