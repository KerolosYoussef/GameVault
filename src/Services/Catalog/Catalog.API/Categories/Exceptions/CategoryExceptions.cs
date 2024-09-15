namespace Catalog.API.Categories.Exceptions
{
    public partial class CategoryExceptions 
    {
        public class CategoryNotFoundException : Exception
        {
            public CategoryNotFoundException() { }
            public CategoryNotFoundException(string message) : base(message) { }
            public CategoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        }

        public class CategoryAlreadyExistsException : Exception
        {
            public CategoryAlreadyExistsException() { }
            public CategoryAlreadyExistsException(string message) : base(message) { }
            public CategoryAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
        }

    }
}
