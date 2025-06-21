using System.Text;

namespace Invoicer.Web.Pages.Invoices;

public static class InvoiceCodeGenerator
{
    private static readonly Random _random = new Random();
    private static readonly object _lock = new object();
    private static int _sequenceCounter = 0;

    public static string CreateInvoiceCode(string clientCode, DateTime dateTime)
    {
        // Get a sequential number (00-99, then wraps around)
        int sequenceNumber;
        lock (_lock)
        {
            sequenceNumber = _sequenceCounter;
            _sequenceCounter = (_sequenceCounter + 1) % 100; // Wrap around at 100
        }

        // Generate 2 random uppercase letters
        string randomLetters = new string(Enumerable.Range(0, 2)
            .Select(_ => (char)('A' + _random.Next(0, 26))).ToArray());

        // Format: CLIENT:YYYYMMDD-XXYY where XX is sequential (00-99) and YY is random letters
        return $"{clientCode}:{dateTime:yyyyMMdd}-{sequenceNumber:D2}{randomLetters}";
    }
} 