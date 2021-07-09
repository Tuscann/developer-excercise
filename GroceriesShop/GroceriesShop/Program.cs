using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceriesShop
{
    class Program
    {
        static void Main()
        {
            List<Product> existingProductsInShop = new();

            while (true)
            {
                Console.WriteLine("Write f finish products");

                Console.WriteLine("Write new product name : ");
                string name = Console.ReadLine();

                if (name == "f")
                {
                    break;
                }

                Console.WriteLine("Write product price : ");

                string value = Console.ReadLine();
                if (value == "f")
                {
                    break;
                }

                double price;

                bool result = double.TryParse(value, out price);

                if (result == true)
                {
                    existingProductsInShop.Add(new Product() { Name = name, Price = price });
                }
            }

            Console.WriteLine("Write list of products you want to buy:");
            List<string> input = Console.ReadLine().Split(", ").ToList();

            string currentproduct = input[0].TrimStart('"').TrimEnd('"');

            if (currentproduct == "")
            {
                Console.WriteLine("Try again");
            }
            else
            {
                List<Product> products = FillProducts(input, existingProductsInShop);

                double totalPrice = 0;

                bool isFound2from3 = Deal2from3(products, ref totalPrice);

                bool isFoundHalfPrice = DealHalfPrice(products, ref totalPrice, isFound2from3);

                if (isFound2from3 == true)
                {
                    Console.WriteLine("DEAL Take 2 for 3 : " + isFound2from3);
                }

                if (isFoundHalfPrice == true)
                {
                    Console.WriteLine("DEAL Buy 1 get 1 half price : " + isFoundHalfPrice);
                }              

                if (totalPrice > 99)
                {
                    Console.WriteLine($"Total price : {(int)totalPrice / 100} aws and {totalPrice % 100} clouds");
                }
                else
                {
                    Console.WriteLine($"Total price : {totalPrice % 100} clouds");
                }
            }
        }

        private static bool DealHalfPrice(List<Product> products, ref double totalPrice, bool isFound2from3)
        {
            bool isFoundHalfPrice = false;

            if (products.Count == 1)
            {
                totalPrice = products[0].Price;
            }
            else if (products.Count == 2)
            {
                if (products[0].Name == products[1].Name)
                {
                    isFoundHalfPrice = true;
                    totalPrice = products[0].Price + products[1].Price / 2;
                }
            }
            else
            {
                if (isFound2from3 == true)
                {
                    bool duplicates = products.Skip(3).GroupBy(x => x.Name).Any(g => g.Count() > 1);

                    if (duplicates == false)
                    {

                    }
                    else
                    {
                        Dictionary<string, int> freqMap = products.Skip(3).GroupBy(product => product.Name)
                                            .Where(product => product.Count() > 1)
                                            .ToDictionary(product => product.Key, product => product.Count());

                        isFoundHalfPrice = true;

                        for (int i = 3; i < products.Count; i++)
                        {
                            if (products[i].Name == freqMap.Keys.First())
                            {
                                products[i].Price /= 2;
                                break;
                            }
                        }

                        for (int i = 3; i < products.Count; i++)
                        {
                            totalPrice += products[i].Price;
                        }
                    }
                }
                else
                {
                    totalPrice = products.Select(x => x.Price).Sum();
                }
            }

            return isFoundHalfPrice;
        }

        private static bool Deal2from3(List<Product> products, ref double totalPrice)
        {
            bool isFound2from3 = false;

            if (products.Count > 2)
            {
                if (products[0].Name == products[1].Name)
                {
                    isFound2from3 = true;

                    totalPrice += products[0].Price;
                    totalPrice += products[2].Price;
                }
                else if (products[1].Name == products[2].Name)
                {
                    isFound2from3 = true;

                    totalPrice += products[0].Price;
                    totalPrice += products[2].Price;
                }
                else if (products[0].Name == products[2].Name)
                {
                    isFound2from3 = true;

                    totalPrice += products[1].Price;
                    totalPrice += products[2].Price;
                }
            }
            else
            {
                totalPrice = products.Select(x => x.Price).Sum();
            }

            return isFound2from3;
        }

        private static List<Product> FillProducts(List<string> input, List<Product> existingProducts)
        {
            List<Product> products = new();

            for (int i = 0; i < input.Count; i++)
            {
                string currentProduct = input[i].TrimStart('"').TrimEnd('"');

                bool containsItem = existingProducts.Any(item => item.Name == currentProduct);

                if (containsItem == true)
                {
                    int index = existingProducts.FindIndex(a => a.Name == currentProduct);

                    products.Add(new Product() { Name = currentProduct, Price = existingProducts[index].Price });
                }
                else
                {
                    Console.WriteLine($"We do not sell product: {currentProduct}");
                }
            }

            return products;
        }
    }
}
