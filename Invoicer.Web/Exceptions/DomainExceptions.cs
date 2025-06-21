namespace Invoicer.Web.Exceptions;

public class InvoiceDomainException : Exception
{
    public InvoiceDomainException(string message) : base(message) { }
    public InvoiceDomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class ClientMismatchException : InvoiceDomainException
{
    public ClientMismatchException() : base("Work client differs from invoice client. Multiple clients cannot be on the same invoice. Create a separate invoice for each client.") { }
}

public class InvoiceNotDeletableException : InvoiceDomainException
{
    public InvoiceNotDeletableException(Guid invoiceId, string status) 
        : base($"Invoice with ID {invoiceId} cannot be deleted in its current status: {status}") { }
}

public class InvoiceNotFoundException : InvoiceDomainException
{
    public InvoiceNotFoundException(Guid invoiceId) : base($"Invoice with ID {invoiceId} was not found") { }
}

public class InvalidHoursException : InvoiceDomainException
{
    public InvalidHoursException(string message) : base(message) { }
}

public class InvalidRateException : InvoiceDomainException
{
    public InvalidRateException(string message) : base(message) { }
}

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, object id) 
        : base($"{entityName} with ID {id} was not found") { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
} 