//Kabluchkov DS (c) 2023
//добавать через нугет newtonsoft json и microsoft.aspnet.webapi.client
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Reflection;

namespace ASP.NET_Core_Web_API_Client_Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            //МЕНЮ
            bool exit = false;
            do
            {                
                Console.Clear();
                Console.WriteLine("Главное меню: \n 1) Вывод всей базы. \n 2) Удаление продукта по ID. \n 3) Изменение продукта по ID. \n 4) Вывод всех продуктов по определённой цене." +
                    " \n 5) Вывод количества продуктов по определённому имени. \n 6) Вывод средней цены по товарам. \n 7) Добавление скидки на все товары. \n 8) Добавление скидки на определённое наименование. \n 9) Добавление продукта. \n 10) Выход.");
                string menuNum = Console.ReadLine();
                switch (menuNum)
                {

                    //Вывод всей базы
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Вывод всей базы!");
                        GetAllProducts();
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Удаление продукта по ID
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Удаление продукта по ID.");
                        Console.WriteLine("Введите ID продукта для удаления:");
                        string id = Console.ReadLine();
                        DeleteProduct(id);                       
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Изменение продукта по ID
                    case "3":
                        Console.WriteLine("Изменение продукта по ID.");
                        Console.WriteLine("Введите ID продукта для изменения:");
                        id = Console.ReadLine();
                        Console.WriteLine("Введите новое название продукта:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Введите новую цену продукта:");
                        int price = Convert.ToInt32(Console.ReadLine());

                        Product productToPut = new Product
                        {
                            ProductID = id,
                            Name = name,
                            Price = price
                        };

                        PutProduct(productToPut);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Вывод всех продуктов по определённой цене
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Вывод товаров по определённой цене.");
                        Console.WriteLine("Введите цену:");
                        price = Convert.ToInt32(Console.ReadLine());
                        GetProductsByPrice(price);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Вывод количества продуктов по определённому имени
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Вывод количества продуктов по определённому имени.");
                        Console.WriteLine("Введите имя продукта:");
                        name = Console.ReadLine();
                        GetNumberOfProductsByName(name);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Вывод средней цены по товарам
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Вывод средней цены по товарам.");
                        GetPriceAverage();
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Добавление скидки на все товары
                    case "7":
                        Console.Clear();
                        Console.WriteLine("Добавление скидки на все товары.");
                        Console.WriteLine("Введите размер скидки в процентах:");
                        int discount = Convert.ToInt32(Console.ReadLine());
                        PutTotalDiscount(discount);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Добавление скидки на определённое наименование
                    case "8":
                        Console.Clear();
                        Console.WriteLine("Добавление скидки на определённое наименование.");
                        Console.WriteLine("Введите имя продукта:");
                        name = Console.ReadLine();
                        Console.WriteLine("Введите размер скидки в процентах:");
                        discount = Convert.ToInt32(Console.ReadLine());
                        PutNameDiscount(name, discount);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //Добавление продукта
                    case "9":
                        Console.Clear();
                        Console.WriteLine("Добавление продуата в базу.");
                        Console.WriteLine("Введите имя продукта:");
                        name = Console.ReadLine();
                        Console.WriteLine("Введите стоимость продукта:");
                        price = Convert.ToInt32(Console.ReadLine());
                        AddProduct(name, price);
                        Console.WriteLine("Конец команды! Press Any Key.");
                        Console.ReadKey();
                        break;

                    //выход
                    case "10":
                        exit= true;
                        break;

                }

            } while (exit != true);
            
        }

        //метод вывода всей базы
        static async void GetAllProducts()
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();
                foreach (var p in products)
                {
                    Console.WriteLine(p);
                }

            }
        }

        //метод добавления продукта
        static async void AddProduct(string name, int price)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            Product product = new Product
            {
                Name = name,
                Price = price
            };

            HttpResponseMessage response = await client.PostAsJsonAsync<Product>(baseUrl, product);

            if (response.IsSuccessStatusCode)
            {
                Product p = await response.Content.ReadAsAsync<Product>();
            }
        }

        //метод удаления продукта
        static async void DeleteProduct(string productId)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.DeleteAsync(baseUrl + "/" + productId);
        }

        //метод обновления продукта
        static async void PutProduct(Product product)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.PutAsJsonAsync<Product>(baseUrl, product);

        }


        //метод поиска продуктов по определённой цене
        static async void GetProductsByPrice(int price)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();
                foreach (var p in products)
                {
                    if (p.Price == price) Console.WriteLine(p);
                }

            }
        }

        //метод определения количества продуктов с конкретным именем
        static async void GetNumberOfProductsByName(string name)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                int counter = 0;
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();
                foreach (var p in products)
                {
                    if (p.Name == name) counter++;
                }
                Console.WriteLine(counter);
            }
        }

        //метод определения средней цены товара
        static async void GetPriceAverage()
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                int counter = 0;
                double result = 0;
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();
                foreach (var p in products)
                {
                    counter += p.Price;
                }
                result = counter / products.Count;

                Console.WriteLine(result);
            }
        }

        //метод определённой скидки на все товары
        static async void PutTotalDiscount(int discount)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();

                foreach (var p in products)
                {
                    double price = p.Price;
                    price = price / 100 * discount;
                    p.Price = (int)price;
                    await client.PutAsJsonAsync<Product>(baseUrl, p);
                }

                HttpResponseMessage resp = await client.PutAsJsonAsync<List<Product>>(baseUrl, products);

            }

        }

        //метод определённой скидки на товар с определённым именем
        static async void PutNameDiscount(string name, int discount)
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                List<Product> products = await response.Content.ReadAsAsync<List<Product>>();

                foreach (var p in products)
                {
                    if (p.Name == name) 
                    {
                        double price = p.Price;
                        price = price / 100 * discount;
                        p.Price = (int)price;
                    } 
                    await client.PutAsJsonAsync<Product>(baseUrl, p);
                }
                HttpResponseMessage resp = await client.PutAsJsonAsync<List<Product>>(baseUrl, products);
            }
        }
    }
}
