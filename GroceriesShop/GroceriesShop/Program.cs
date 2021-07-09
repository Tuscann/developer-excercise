using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceriesShop
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Write list of products you want to buy:");
            List<string> input = Console.ReadLine().Split(", ").ToList();

            string currentproduct = input[0].TrimStart('"').TrimEnd('"');

            if (currentproduct == "")
            {
                Console.WriteLine("Try again");
            }
            else
            {
                List<Product> products = FillProducts(input, ref currentproduct);

                double totalPrice = 0;

                bool isFound2from3 = Deal2from3(products, ref totalPrice);

                bool isFoundHalfPrice = DealHalfPrice(products, ref totalPrice, isFound2from3);

                Console.WriteLine("DEAL Take 2 for 3 : " + isFound2from3);
                Console.WriteLine("DEAL Buy 1 get 1 half price : " + isFoundHalfPrice);

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

        private static List<Product> FillProducts(List<string> input, ref string currentproduct)
        {
            List<Product> products = new();

            for (int i = 0; i < input.Count; i++)
            {
                currentproduct = input[i].TrimStart('"').TrimEnd('"');

                if (currentproduct == "apple")
                {
                    products.Add(new Product() { Name = "Apple", Price = 50 });
                }
                else if (currentproduct == "banana")
                {
                    products.Add(new Product() { Name = "Banana", Price = 40 });
                }
                else if (currentproduct == "tomato")
                {
                    products.Add(new Product() { Name = "Tomato", Price = 30 });
                }
                else if (currentproduct == "potato")
                {
                    products.Add(new Product() { Name = "potato", Price = 26 });
                }
                else
                {
                    Console.WriteLine($"We do not sell product: {currentproduct}");
                }
            }

            return products;
        }
    }
}
