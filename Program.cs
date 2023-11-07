using Anropa_databasen__SQL___ORM_.Data;
using Anropa_databasen__SQL___ORM_.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Anropa_databasen__SQL___ORM_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NorthWindContext())
            {
                MainMenu(context);
            }
        }

        // Simple menu for navigating
        static void MainMenu(NorthWindContext context)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("[1] Get all customers");
                Console.WriteLine("[2] Add a new customer");
                Console.WriteLine("[3] Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        GettingCustomers(context);
                        break;
                    case "2":
                        AddCustomerDetails(context);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Gives the user the choice of ascending or descending for CustomerDetails
        static void GettingCustomers(NorthWindContext context)
        {
            {
                Console.Write("Enter 'a' for ascending or 'd' for descending sort by CompanyName: ");
                string sortOrder = Console.ReadLine().ToLower();

                try
                {
                    // Attempt to retrieve customer data from the database
                    var customers = context.Customers
                        .Include(c => c.Orders)
                        .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .OrderBy(c => c.CompanyName);


                    if (sortOrder == "d")
                    {
                        Console.WriteLine("Descending");
                        customers = customers.OrderByDescending(c => c.CompanyName);
                    }
                    else if (sortOrder == "a")
                    {
                        Console.WriteLine("Ascending");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Data will be displayed in ascending order by default.");
                        Console.WriteLine("Press Enter to continue:");
                        Console.ReadLine();
                    }

                    var sortedCustomers = customers.ToList();

                    Console.Clear();
                    DisplayCustomerDetails(sortedCustomers);

                    DisplayAllCustomerDetails(sortedCustomers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while retrieving customer data.");
                    Console.WriteLine($"Details: {ex.Message}");
                    Console.WriteLine("Press Enter to go back to the main menu:");
                    Console.ReadLine();
                }
            }
        }

        // Create a new user
        public static void AddCustomerDetails(NorthWindContext context)
        {
            // All inputs can be converted to null if empty.
            Console.Clear();
            Console.Write("Company Name: ");
            string companyName = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Contact Name: ");
            string contactName = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Contact Title: ");
            string contactTitle = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Address: ");
            string address = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("City: ");
            string city = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Region: ");
            string region = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Postal Code: ");
            string postalCode = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Country: ");
            string country = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Phone: ");
            string phone = ConvertEmptyToNull(Console.ReadLine());

            Console.Write("Fax: ");
            string fax = ConvertEmptyToNull(Console.ReadLine());

            // Random string five letters long. Used for "CustomerId"
            string randomString = GenerateRandomString(5);

            try
            {
                if (companyName != null)
                {
                    // New customer is created from input details
                    var newCustomer = new Customer
                    {
                        CompanyName = companyName,
                        CustomerId = randomString,
                        ContactName = contactName,
                        ContactTitle = contactTitle,
                        Address = address,
                        City = city,
                        Region = region,
                        PostalCode = postalCode,
                        Country = country,
                        Phone = phone,
                        Fax = fax
                    };

                    // Add the new customer to context
                    context.Customers.Add(newCustomer);

                    // Save changes to the database
                    context.SaveChanges();

                    Console.WriteLine("New customer added to the database.");
                    Console.WriteLine("Press enter to go back to the main menu:");
                    Console.ReadLine();
                }
                else
                {
                    // "CompanyName" is of "no null" in the NorthWind databas
                    Console.WriteLine("New customer could not be added to the database. Company name can not be null.");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Error: Failed to add a new customer. Please check the data and try again.");
                Console.WriteLine($"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine($"Details: {ex.Message}");
            }

            Console.WriteLine("Press enter to go back to the main menu:");
            Console.ReadLine();
        }

        // Displays a small amount of info on all customers
        public static void DisplayCustomerDetails(List<Customer> sortedCustomers)
        {
            for (int i = 0; i < sortedCustomers.Count; i++)
            {
                var c = sortedCustomers[i];
                Console.WriteLine($"   [{i + 1}]:CompanyName: {c.CompanyName}\n\tCountry: {c.Country}\n\tPhone: {c.Phone}\n\tRegion: {c.Region}\n\tAmount of Orders: {c.Orders.Count}\n");
                Console.WriteLine("--------------------------------------------------------------------\n");
            }
        }

        // Displays all info on a specific customer
        public static void DisplayAllCustomerDetails(List<Customer> sortedCustomers)
        {
            while (true)
            {
                Console.WriteLine($"Please choose a customer number (1-{sortedCustomers.Count}) or enter '0' to go back to the main menu: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        break;
                    }
                    else if (choice >= 1 && choice <= sortedCustomers.Count)
                    {
                        var c = sortedCustomers[choice - 1];
                        Console.Clear();
                        Console.WriteLine("--------------------------------------------------------------------\n");
                        Console.WriteLine($"CompanyName: {c.CompanyName}\n" +
                            $"ContactName: {c.ContactName}\n" +
                            $"ContactTitle: {c.ContactTitle}\n" +
                            $"Address: {c.Address}\n" +
                            $"City: {c.City}\n" +
                            $"PostalCode: {c.PostalCode}\n" +
                            $"Country: {c.Country}\n" +
                            $"Phone: {c.Phone}\n" +
                            $"Fax: {c.Fax}\n" +
                            $"Region: {c.Region}\n" +
                            $"Amount of Orders: {c.Orders.Count}\n");
                        Console.WriteLine("--------------------------------------------------------------------\n");

                        // Displays all the customers orders
                        foreach (var order in c.Orders)
                        {
                            // Print out order id and date ordered
                            Console.WriteLine($" Order: {order.OrderId}");
                            Console.WriteLine($" Date ordered: {order.OrderDate}");

                            // Displays all the products and their price
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                Console.WriteLine($" Product: {orderDetail.Product.ProductName} | Unit price: {orderDetail.Product.UnitPrice}");
                            }
                            Console.WriteLine();
                            Console.WriteLine("--------------------------------------------------------------------\n");
                        }
                        Console.WriteLine("Press enter to go back to the main menu:");
                        Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }

        // Converts any empty strings to null
        public static string ConvertEmptyToNull(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            return input;
        }

        // Generates a random string
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }
    }
}