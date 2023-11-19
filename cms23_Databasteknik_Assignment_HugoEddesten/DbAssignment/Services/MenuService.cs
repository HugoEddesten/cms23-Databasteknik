using DbAssignment.Entities;
using DbAssignment.Models;
using Microsoft.IdentityModel.Tokens;


namespace DbAssignment.Services;

public class MenuService
{
    private readonly CustomerService _customerService;
    private readonly CustomerInformationsService _customerInformationsService;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public MenuService(CustomerService customerService, CustomerInformationsService customerInformationsService, ProductService productService, OrderService orderService)
    {
        _customerService = customerService;
        _customerInformationsService = customerInformationsService;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task MainMenu()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("1. Show Customers");
            Console.WriteLine("2. Add Customer");
            Console.WriteLine("3. Delete Customer");
            Console.WriteLine();
            Console.WriteLine("4. Show Products");
            Console.WriteLine("5. Add Product");
            Console.WriteLine("6. Delete Product");
            Console.WriteLine();

            Console.Write("Choose one: ");
            var MainChoice = Console.ReadLine();

            switch (MainChoice)
            {
                case "1":
                    await ShowCustomersMenu();
                    break;
                case "2":
                    await AddCustomerMenu();
                    break;
                case "3":
                    await DeleteCustomerMenu();
                    break;
                case "4":
                    await ShowProductsMenu();
                    break;
                case "5":
                    await AddProductMenu();
                    break;
                case "6":
                    await DeleteProductMenu();
                    break;
            }
        }
        while (true);
    }

    private async Task ShowProductsMenu()
    {
        Console.Clear();
        var products = await _productService.GetAllProducts();

        int count = 0;
        foreach (var product in products)
        {
            count++;
            Console.WriteLine($"1: {product.Name} {product.Price}kr \n{product.Description}");
            Console.WriteLine();
        }
        Console.ReadKey();
    }

    private async Task AddOrderMenu(CustomerEntity customer)
    {
        OrderEntity order = new OrderEntity();
        order.Customer = customer;
        bool cancel = false;
        
        do
        {
            Console.Clear();
            var products = await _productService.GetAllProducts();
            if (products.Any())
            {
                int count = 0;
                foreach (var product in products)
                {
                    count++;
                    Console.WriteLine($"{count}: {product.Name} {product.Price}kr \n{product.Description}");
                    Console.WriteLine();
                }
                Console.Write("Choose Product to add to Order: ");
                var productChoiceString = Console.ReadLine();

                if (int.TryParse(productChoiceString, out int productChoice))
                {
                    order.Orders.Add(products.ElementAt(productChoice - 1));
                }
                
                Console.Clear();
                Console.WriteLine("Cart:");

                decimal totalPrice = 0;
                foreach (ProductEntity product in order.Orders)
                {
                    Console.WriteLine($"{product.Name} {product.Price}");
                    totalPrice += product.Price;
                }
                Console.WriteLine($"total cost: {totalPrice}");
                Console.WriteLine();

                Console.WriteLine("1: Add Product to Order");
                Console.WriteLine("2: Finish Order");
                Console.WriteLine("0: Cancel Order");

                Console.Write("How do you wish to proceed: ");
                var menuChoice = Console.ReadLine();
                switch (menuChoice)
                {
                    case "1":
                        cancel = false;
                        break;
                    case "2":
                        await _orderService.AddOrderAsync(order);
                        Console.Clear();
                        Console.WriteLine("Order Placed");
                        Console.ReadKey();
                        cancel = true;
                        break;
                    case "0":
                        cancel = true;
                        break;
                }
            }
            else
            {
                Console.WriteLine("No products to order");
                cancel = true;
                Console.ReadKey();
            }
        } while (cancel == false);

        
    }

    private async Task DeleteProductMenu()
    {
        Console.Clear();
        var products = await _productService.GetAllProducts();
        if (products.Any())
        {
            int count = 0;
            foreach (var product in products)
            {
                count++;
                Console.WriteLine($"Product number: {product.Id} \nName: {product.Name}");
                Console.WriteLine();
            }
            Console.Write("Insert Product number to delete: ");
            string? productNumberString = Console.ReadLine();
            if (int.TryParse(productNumberString, out int productNumber))
            {
                bool result = await _productService.DeleteProductAsync(productNumber);
                if (result)
                    Console.WriteLine("Product was removed");
                else
                    Console.WriteLine("Product not found");
            }
        }
        else
            Console.WriteLine("No Products Registered");
        Console.ReadKey();
    }

    private async Task AddProductMenu()
    {
        ProductEntity product = new ProductEntity();

        Console.Clear();
        Console.Write("Name: ");
        product.Name = Console.ReadLine() ?? "";
        Console.Write("Description: ");
        product.Description = Console.ReadLine() ?? "";
        Console.Write("Price: ");
        string priceString = Console.ReadLine() ?? "";
        if (int.TryParse(priceString, out int priceInt))
        {
            product.Price = priceInt;
        }
        await _productService.CreateProductAsync(product);
        
    }

    public async Task ShowCustomersMenu()
    {
        Console.Clear();
        var customers = await _customerService.GetAllCustomersAsync();
        int count = 0;
        if (customers.Any())
        {
            foreach (var customer in customers)
            {
                count++;
                Console.WriteLine($"{count}: {customer.FirstName} {customer.LastName}");
            }
            Console.Write("Choose a Customer to View Details (Leave empty to go back): ");

            string customerOption = Console.ReadLine() ?? null!;
            if (customerOption != null!)
            {
                var parseResult = int.TryParse(customerOption, out var id);
                var index = id - 1;
                if (parseResult && id >= 1 && id <= customers.Count())
                {
                    CustomerEntity customerAtIndex = customers.ElementAt(index);
                    await ShowCustomerDetailsMenu(customerAtIndex);
                }
            }
        }
        
    }
    public async Task ShowCustomerDetailsMenu(CustomerEntity customer)
    {
        Console.Clear();
    
        Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");

        foreach (var information in customer.Information)
            Console.WriteLine($"{information.CustomerInformationTypesEntity.InformationType}: {information.Value}");    

        Console.Write($"Address: {customer.Address.StreetName}");
        
        if (!customer.Address.StreetNumber.IsNullOrEmpty())
            Console.Write($" {customer.Address.StreetNumber}");

        Console.WriteLine($" {customer.Address.PostalCode} {customer.Address.City}");
        Console.WriteLine();
        Console.WriteLine("1: Edit Customer");
        Console.WriteLine("2: Add Order to Customer");
        
        string option = Console.ReadLine() ?? null!;
        switch (option)
        {
            case "1":
                await UpdateCustomerMenu(customer);
                break;
            case "2":
                await AddOrderMenu(customer);
                break;
        }
    }

    public async Task UpdateCustomerMenu(CustomerEntity customer)
    {
        List<string> customerProperties = new List<string>();
        customerProperties.Add(customer.FirstName);
        customerProperties.Add(customer.LastName);
        foreach (var customerInformation in customer.Information)
            customerProperties.Add(customerInformation.Value);
        customerProperties.Add(customer.Address.StreetName);
        customerProperties.Add(customer.Address.StreetNumber ?? "");
        customerProperties.Add(customer.Address.PostalCode);
        customerProperties.Add(customer.Address.City);


        Console.Clear();
        Console.WriteLine($"1: {"First name:",15} \t{customer.FirstName}");
        Console.WriteLine($"2: {"Last name:",15} \t{customer.LastName}");

        // Loops through and displays all customer information
        int count = 2;
        foreach (var information in customer.Information)
        {
            count++;
            Console.WriteLine($"{count}:{information.CustomerInformationTypesEntity.InformationType,15}: \t{information.Value}");
        }

        Console.WriteLine($"{count + 1}: {"Street name:",15} \t{customer.Address.StreetName}");
        Console.WriteLine($"{count + 2}: {"Street number:",15} \t{customer.Address.StreetNumber}");
        Console.WriteLine($"{count + 3}: {"Postal code:",15} \t{customer.Address.PostalCode}");
        Console.WriteLine($"{count + 4}: {"City/Region:",15} \t{customer.Address.City}");

        Console.WriteLine();
        Console.Write("Input the number of the property you wish to edit: ");

        string propertyOption = Console.ReadLine() ?? null!;
        bool propertyParseResult = int.TryParse(propertyOption, out int propertyIndex);

        Console.Clear();
        Console.WriteLine($"Current value: {customerProperties[propertyIndex - 1]}");
        Console.Write("New value: ");
        customerProperties[propertyIndex - 1] = Console.ReadLine() ?? "";

        // updating all the customer properties
        customer.FirstName = customerProperties[0];
        customer.LastName = customerProperties[1];

        for (var x = 0; x < customer.Information.Count(); x++)
        {
            var information = customer.Information.ElementAt(x);
            information.Value = customerProperties[x + 2];
        }
        
        customer.Address.StreetName = customerProperties[customer.Information.Count() + 2];
        customer.Address.StreetNumber = customerProperties[customer.Information.Count() + 3];
        customer.Address.PostalCode = customerProperties[customer.Information.Count() + 4];
        customer.Address.City = customerProperties[customer.Information.Count() + 5];

        await _customerService.UpdateCustomerAsync(customer);
    }

    public async Task AddCustomerMenu()
    {
        Console.Clear();
        CustomerRegistration customer = new CustomerRegistration();

        Console.Write("First name: ");
        customer.FirstName = Console.ReadLine() ?? null!;

        Console.Write("Last name: ");
        customer.LastName = Console.ReadLine() ?? null!;

        Console.Write("Email: ");
        customer.Email = Console.ReadLine() ?? null!;

        Console.Write("Phone number: ");
        customer.Phone = Console.ReadLine() ?? null!;

        Console.Write("Street name: ");
        customer.StreetName = Console.ReadLine() ?? null!;

        Console.Write("Street number: ");
        customer.StreetNumber = Console.ReadLine() ?? null!;

        Console.Write("Postal code: ");
        customer.PostalCode = Console.ReadLine() ?? null!;

        Console.Write("City/Region: ");
        customer.City = Console.ReadLine() ?? null!;

        await _customerService.CreateCustomerAsync(customer);
        
    }
    public async Task DeleteCustomerMenu()
    {
        Console.Clear();
        var customers = await _customerService.GetAllCustomersAsync();
        int count = 0;
        if (customers.Any())
        {
            foreach (var customer in customers)
            {
                var customerInformations = await _customerInformationsService.GetInformationAsync(customer)!;
                string email = "";
                foreach (var informationEntity in customerInformations)
                {
                    if (informationEntity.CustomerInformationTypesEntity.InformationType == "Email")
                        email = informationEntity.Value;
                }
                count++;
                Console.WriteLine($"{count}: {email}");
            }
            Console.Write("\nChoose a customer to remove: ");

            var removeOption = Console.ReadLine();
            var parseResult = int.TryParse(removeOption, out var id);

            if (parseResult && id >= 1 && id <= customers.Count())
            {
                var index = id - 1;
                CustomerEntity customerAtIndex = customers.ElementAt(index);

                await _customerService.DeleteCustomerAsync(customerAtIndex);

                Console.Clear();
                Console.WriteLine($"The customer \"{customerAtIndex.FirstName} {customerAtIndex.LastName}\" has been removed");

            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Invalid input");
            }
        } else
            Console.WriteLine($"No Customers Registered");
        
        Console.ReadKey();
    }

}
