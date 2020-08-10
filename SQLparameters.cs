using System;
using Mono.Data.Sqlite;

namespace FruitSales
{
    public class SQLparameters
    {
        public string Name { get; set; }
        public object ValueA { get; set; }

        public SQLparameters(string name, object valueA)
        {
            Name = name;
            ValueA = valueA;
        }
    }
}