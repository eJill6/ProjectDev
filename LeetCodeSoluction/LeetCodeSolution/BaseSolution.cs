using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodeSolution
{
    public abstract class BaseSolution
    {
        public abstract object GetResult();


        public void Run()
        {
            Console.WriteLine(JsonConvert.SerializeObject(GetResult()));
            Console.ReadLine();
        }
    }
}
