namespace MyCloud.NoSqlDatabaseAdminService.Models;

/// <summary>
/// An invoice stating the amounts owed by a customer.
/// </summary>
public class Invoice
{
    /// <summary>
    /// The unique id of the invoice.
    /// </summary>
    /// <example>b4c5634d-2576-4996-9244-69a9aa429ffe</example>
    public Guid Id { get; set; }
    /// <summary>
    /// The project id this invoice is associated with.
    /// </summary>
    /// <example>d514ec00-4a34-4035-86d0-fea096608ac1</example>
    public Guid ProjectId { get; set; }
    /// <summary>
    /// Timestamp of when the invoice was created.
    /// </summary>
    /// <example>2021-11-30T07:58:43.2697702+01:00</example>
    public DateTime Created { get; set; }
    /// <summary>
    /// Timestamp of when the invoice was last updated.
    /// </summary>
    /// <example>2021-11-30T08:01:26.7237038+01:00</example>
    public DateTime Updated { get; set; }
    /// <summary>
    /// The start of the billing period.
    /// </summary>
    /// <example>2021-01-01T00:00:00</example>
    public DateTime BillingPeriodStart { get; set; }
    /// <summary>
    /// The end of the billing period.
    /// </summary>
    /// <example>2021-01-31T00:00:00</example>
    public DateTime BillingPeriodEnd { get; set; }
    /// <summary>
    /// The amount billed in this invoice.
    /// </summary>
    /// <example>1627</example>
    public long AmountBilledCents { get; set; }
    /// <summary>
    /// The amount paid for this invoice.
    /// </summary>
    /// <example>1200</example>
    public long AmountPaidCents { get; set; }
    /// <summary>
    /// The status of the invoice.
    /// </summary>
    /// <example>0</example>
    public InvoiceStatus Status { get; set; }
}

/// <summary>
/// The status an invoice can have.
/// </summary>
public enum InvoiceStatus
{
    /// <summary>
    /// An invoice which is still open but not overdue.
    /// </summary>
    Invoiced,
    /// <summary>
    /// An invoice which was already paid.
    /// </summary>
    Paid,
    /// <summary>
    /// An invoice which hasn't been paid yet and is overdue.
    /// </summary>
    Overdue
}