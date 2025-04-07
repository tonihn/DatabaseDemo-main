
using Microsoft.EntityFrameworkCore;
// Ensure the namespace exists or remove this line if unnecessary

using WebStore.Entities;
using WebStoreSolution.Assignments; // Verify that the 'Assignments' namespace is defined in your project



using var context = new AssignmentContext();


var Assigments = new LinqQueriesAssignment(context);

await Assigments.Task01_ListAllCustomers();

await Assigments.Task02_ListOrdersWithItemCount();

await Assigments.Task03_ListProductsByDescendingPrice();

//            await Assigments.Task04_ListPendingOrdersWithTotalPrice();

await Assigments.Task05_OrderCountPerCustomer();

await Assigments.Task06_Top3CustomersByOrderValue();

await Assigments.Task07_RecentOrders();

await Assigments.Task08_TotalSoldPerProduct();

await Assigments.Task09_DiscountedOrders();

await Assigments.Task10_AdvancedQueryExample();


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
