using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Contracts.Base
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(string message)
        {
            Description = message;
        }

        public override string ToString()
        {
            return Description.ToString();
        }
    }
}
