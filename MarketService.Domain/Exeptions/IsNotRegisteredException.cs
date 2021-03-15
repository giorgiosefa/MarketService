using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Exeptions
{
    public class IsNotRegisteredException : Exception
    {
        public IsNotRegisteredException(string message) : base(message)
        {

        }
    }
}
