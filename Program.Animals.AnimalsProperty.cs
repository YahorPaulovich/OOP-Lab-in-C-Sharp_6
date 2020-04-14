using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionHandling
{
    partial class Program
    {
        public abstract partial class Animals //Животные
        {
            public enum AnimalsProperty : byte
            {
                PrintNames,// 0              
                MAXBodyLength,// 1                
                TotalWeight// 2               
            }
        }
    }
}
