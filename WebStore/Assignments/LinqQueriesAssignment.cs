using Microsoft.EntityFrameworkCore;
using WebStore.Entities;
namespace WebStoreSolution.Assignments;

    /// <summary>
    /// This class demonstrates various LINQ query tasks 
    /// to practice querying an EF Core database.
    /// 
    /// ASSIGNMENT INSTRUCTIONS:
    ///   1. For each method labeled "TODO", write the necessary
    ///      LINQ query to return or display the required data.
    ///      
    ///   2. Print meaningful output to the console (or return
    ///      collections, as needed).
    ///      
    ///   3. Test each method by calling it from your Program.cs
    ///      or test harness.
    /// </summary>
    public class LinqQueriesAssignment(AssignmentContext context)
{
        private readonly AssignmentContext _dbContext = context;

    /// <summary>
    /// 1. List all customers in the database:
    ///    - Print each customer's full name (First + Last) and Email.
    /// </summary>
    public async Task Task01_ListAllCustomers()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            foreach (var Customer in customers)
            {
                Console.WriteLine($"Customer ID: {Customer.CustomerId}, Name: {Customer.FirstName}, Name: {Customer.LastName}, Email: {Customer.Email}");
        }
        }

        /// <summary>
        /// 2. Fetch all orders along with:
        ///    - Customer Name
        ///    - Order ID
        ///    - Order Status
        ///    - Number of items in each order (the sum of OrderItems.Quantity)
        /// </summary>
        public async Task Task02_ListOrdersWithItemCount()
        {
            var orders = await _dbContext.Orders
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
                    Status = order.OrderStatus,
                    ItemCount = order.OrderItems.Sum(item => item.Quantity)
                })
                .ToListAsync();

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}, Status: {order.Status}, Item Count: {order.ItemCount}");
            }
        }

        /// <summary>
        /// 3. List all products (ProductName, Price),
        ///    sorted by price descending (highest first).
        /// </summary>
        public async Task Task03_ListProductsByDescendingPrice()
        {
            var products = await _dbContext.Products
                .OrderByDescending(product => product.Price)
                .ToListAsync();

            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.ProductName}, Price: {product.Price}");
            }
        }

        /// <summary>
        /// 4. Find all "Pending" orders (order status = "Pending")
        ///    and display:
        ///      - Customer Name
        ///      - Order ID
        ///      - Order Date
        ///      - Total price (sum of unit_price * quantity - discount) for each order
        /// </summary>
        public async Task Task04_ListPendingOrdersWithTotalPrice()
        {
            var pendingOrders = await _dbContext.Orders
                .Where(order => order.OrderStatus == "Pending")
                .Select(order => new
                {
                    CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.OrderItems.Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount)
                })
                .ToListAsync();

            foreach (var order in pendingOrders)
            {
                Console.WriteLine($"Customer: {order.CustomerName}, Order ID: {order.OrderId}, Date: {order.OrderDate}, Total Price: {order.TotalPrice:C}");
            }
        }

        /// <summary>
        /// 5. List the total number of orders each customer has placed.
        ///    Output should show:
        ///      - Customer Full Name
        ///      - Number of Orders
        /// </summary>
        public async Task Task05_OrderCountPerCustomer()
        {
            var orderCounts = await _dbContext.Customers
                .Select(customer => new
                {
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    OrderCount = customer.Orders.Count()
                })
                .ToListAsync();

            foreach (var customer in orderCounts)
            {
                Console.WriteLine($"Customer: {customer.CustomerName}, Number of Orders: {customer.OrderCount}");
            }
        }

        /// <summary>
        /// 6. Show the top 3 customers who have placed the highest total order value overall.
        ///    - For each customer, calculate SUM of (OrderItems * Price).
        ///      Then pick the top 3.
        /// </summary>
        public async Task Task06_Top3CustomersByOrderValue()
        {
            var topCustomers = await _dbContext.Customers
                .Select(customer => new
                {
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    TotalOrderValue = customer.Orders.Sum(order => order.OrderItems.Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount))
                })
                .OrderByDescending(c => c.TotalOrderValue)
                .Take(3)
                .ToListAsync();

            foreach (var customer in topCustomers)
            {
                Console.WriteLine($"Customer: {customer.CustomerName}, Total Order Value: {customer.TotalOrderValue:C}");
            }
        }

        /// <summary>
        /// 7. Show all orders placed in the last 30 days (relative to now).
        ///    - Display order ID, date, and customer name.
        /// </summary>
        public async Task Task07_RecentOrders()
        {
            var recentOrders = await _dbContext.Orders
                .Where(order => order.OrderDate >= DateTime.Now.AddDays(-30))
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    CustomerName = order.Customer.FirstName + " " + order.Customer.LastName
                })
                .ToListAsync();

            foreach (var order in recentOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Date: {order.OrderDate}, Customer: {order.CustomerName}");
            }
        }

        /// <summary>
        /// 8. For each product, display how many total items have been sold
        ///    across all orders.
        ///    - Product name, total sold quantity.
        ///    - Sort by total sold descending.
        /// </summary>
        public async Task Task08_TotalSoldPerProduct()
        {
            var productSales = await _dbContext.Products
                .Select(product => new
                {
                    ProductName = product.ProductName,
                    TotalSold = product.OrderItems.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .ToListAsync();

            foreach (var product in productSales)
            {
                Console.WriteLine($"Product: {product.ProductName}, Total Sold: {product.TotalSold}");
            }
        }

        /// <summary>
        /// 9. List any orders that have at least one OrderItem with a Discount > 0.
        ///    - Show Order ID, Customer name, and which products were discounted.
        /// </summary>
        public async Task Task09_DiscountedOrders()
        {
            var discountedOrders = await _dbContext.Orders
                .Where(order => order.OrderItems.Any(oi => oi.Discount > 0))
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
                    DiscountedProducts = order.OrderItems
                        .Where(oi => oi.Discount > 0)
                        .Select(oi => oi.Product.ProductName)
                        .ToList()
                })
                .ToListAsync();

            foreach (var order in discountedOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}, Discounted Products: {string.Join(", ", order.DiscountedProducts)}");
            }
        }

        /// <summary>
        /// 10. (Open-ended) Combine multiple joins or navigation properties
        ///     to retrieve a more complex set of data.
        /// </summary>
        public async Task Task10_AdvancedQueryExample()
        {
            var electronicsProducts = _dbContext.Products
                .Where(p => p.Categories.Any(c => c.CategoryName == "Electronics"))
                .Select(p => p.ProductId)
                .ToList();

            var electronicsOrders = await _dbContext.Orders
                .Where(order => order.OrderItems.Any(oi => electronicsProducts.Contains(oi.Product.ProductId)))
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
                    Products = order.OrderItems
                        .Where(oi => electronicsProducts.Contains(oi.Product.ProductId))
                        .Select(oi => new
                        {
                            ProductName = oi.Product.ProductName,
                            StoreWithMaxStock = oi.Product.Stocks
                                .OrderByDescending(stock => stock.QuantityInStock)
                                .Select(stock => stock.Store.StoreName)
                                .FirstOrDefault()
                        })
                        .ToList()
                })
                .ToListAsync();

            foreach (var order in electronicsOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}");
                foreach (var product in order.Products)
                {
                    Console.WriteLine($"  Product: {product.ProductName}, Store with Max Stock: {product.StoreWithMaxStock}");
                }
            }
        }
    }
