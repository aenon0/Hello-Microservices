using System;
using Play.Common;
namespace Play.Catalog.Service.Entities
{
    public class Item : IEntity
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public decimal Price { set; get; }
        public DateTimeOffset CreatedDate {get; set;}
    }
}