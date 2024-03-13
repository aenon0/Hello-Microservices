using Amazon.SecurityToken.Model;
using Play.Common;

namespace Play.Inventory.Service.Entities;

public class InventoryItem : IEntity
{
    public Guid Id {set; get;}
    public Guid UserId {set; get;}
    public Guid CatalogItemId {get; set;}
    public int Quantity  {set; get;}
    public DateTimeOffset AcquiredDate {set; get;}
}
 