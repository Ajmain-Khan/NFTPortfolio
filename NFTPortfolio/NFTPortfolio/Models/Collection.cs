using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFTPortfolio.Models
{
    class Collection
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Policy { get; set; }
    }
}
