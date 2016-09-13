using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class TokenModel
    {
        public long Id;
        public string Token;
        public string AccountId { get; set; }
    }
}