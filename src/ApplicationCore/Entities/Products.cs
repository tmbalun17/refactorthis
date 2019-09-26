using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ApplicationCore.Models
{
    public class Products
    {
        public List<Product> Items { get; set; }
    }
}