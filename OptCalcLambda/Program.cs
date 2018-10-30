using System;
using System.Collections.Generic;
using System.Linq;

namespace OptCalcLambda
{
    class Program
    {
        private delegate TResult PrmsFunc<TSourse, TResult>(params TSourse[] prms);

        static int SomeExpensiveOperation(params int[] prms)
        {
            Console.WriteLine($"Вызов функции F({prms[0]})");
            return prms.Sum();
        }

        static void Main(string[] args)
        {
            PrmsFunc<int, int> F = Memoize<int, int>(SomeExpensiveOperation);
            PrmsFunc<int, int> Lambda = prms => F(prms[0]) > F(prms[1]) ? F(prms[0]) : (F(prms[0]) < F(2 * prms[1]) ? F(2 * prms[1]) : F(prms[1]));
            int[] lambdaParams = { 1, 5 };

            double i = OptimizedCalculation(Lambda, lambdaParams);

            Console.WriteLine($"Результат: {i}");
            Console.ReadKey();
        }

        static TResult OptimizedCalculation<TSourse, TResult>(PrmsFunc<TSourse, TResult> lambda,
            IEnumerable<TSourse> lambdaParams)
                => lambda(lambdaParams.ToArray());

        static PrmsFunc<TSourse, TResult> Memoize<TSourse, TResult>(PrmsFunc<TSourse, TResult> func)
        {
            Dictionary<string, TResult> cache = new Dictionary<string, TResult>();
            return args =>
            {
                var key = $"{ args.ToString() }:{ string.Join("|", args) }";

                TResult result = default(TResult);

                if (cache.ContainsKey(key))
                {
                    result = cache[key];
                }
                else
                {
                    result = cache[key] = func(args);
                }
                return result;
            };
        }
    }
}
