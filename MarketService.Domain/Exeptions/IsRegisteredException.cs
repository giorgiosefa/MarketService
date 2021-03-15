using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Exeptions
{
    public class IsRegisteredException : Exception
    {
        public IsRegisteredException(string message) : base(message)
        {

        }
    }
}
