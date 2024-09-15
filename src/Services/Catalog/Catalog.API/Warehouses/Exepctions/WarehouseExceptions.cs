namespace Catalog.API.Warehouses.Exepctions
{
    public static partial class WarehouseExceptions
    {
        public class WarehouseNotFoundException : Exception
        {
            public WarehouseNotFoundException() : base("This warehouse is not found") { }
            public WarehouseNotFoundException(string message) : base(message) { }
            public WarehouseNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        }

        public class WarehouseAlreadyExistsException : Exception
        {
            public WarehouseAlreadyExistsException() : base("This warehouse already exists") { }
            public WarehouseAlreadyExistsException(string message) : base(message) { }
            public WarehouseAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
