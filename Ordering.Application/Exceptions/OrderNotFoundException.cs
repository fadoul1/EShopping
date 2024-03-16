namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(string name, object key)
    : ApplicationException($"Entity {name} - {key} is not found.") { }
