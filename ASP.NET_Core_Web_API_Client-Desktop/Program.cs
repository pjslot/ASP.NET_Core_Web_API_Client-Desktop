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
            Console.WriteLine("Вывод всей базы!");
            GetAllProducts();

            Console.WriteLine("Жмём кнопку!");
            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("Добавляем сыр!");
            AddProduct("cheese", 400);
            Console.WriteLine("Жмём кнопку!");
            Console.ReadKey();
            Console.WriteLine();

            //Console.WriteLine("Вывод всей базы!");
            //GetAllProducts();
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();


            //Console.WriteLine("удаляем томат!");
            //DeleteProduct("7faecc5e-245b-49a4-9082-e090edc5765a");
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();
            //Console.WriteLine();

            //Console.WriteLine("Вывод всей базы!");
            //GetAllProducts();
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();

            //Console.WriteLine("меняем один из сыров");
            //Product productToPut = new Product
            //{
            //    ProductID = "f3fd619a-2a92-482a-b4f5-f0e9a2478aec",
            //    Name = "Cheeeese",
            //    Price = 100000
            //};

            //PutProduct(productToPut);
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();
            //Console.WriteLine();

            //Console.WriteLine("Вывод по ценнику 400");
            //GetProductsByPrice(400);
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();

            //Console.WriteLine("Вывод количества сыров");
            //GetNumberOfProductsByName("cheese");
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();

            //Console.WriteLine("Вывод средней цены по товарам");
            //GetPriceAverage();
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();

            //Console.WriteLine("Добавление скидки на всё 50%");
            //PutTotalDiscount(50);
            //Console.WriteLine("Жмём кнопку!");
            //Console.ReadKey();

            Console.WriteLine("Добавление скидки на сыр 50%");
            PutNameDiscount("cheese", 50);
            Console.WriteLine("Жмём кнопку!");
            Console.ReadKey();

            Console.WriteLine("Вывод всей базы!");
            GetAllProducts();
            Console.ReadKey();



        }

        //метод вывода всей базы
        static async void GetAllProducts()
        {
            HttpClient client = new HttpClient();
            string baseUrl = @"http://localhost:5000/api/product";

            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                //  string jsonresp = await response.Content.ReadAsStringAsync();
                //   Console.WriteLine(jsonresp);
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
