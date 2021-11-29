namespace MyCloud.NoSqlDatabaseAdminService.Models;

public class Invoice
{
    /// <summary>
    /// The unique id of the invoice.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The project id this invoice is associated with.
    /// </summary>
    public Guid ProjectId { get; set; }
    /// <summary>
    /// Timestamp of when the invoice was created.
    /// </summary>
    public DateTime Created { get; set; }
    /// <summary>
    /// Timestamp of when the invoice was last updated.
    /// </summary>
    public DateTime Updated { get; set; }
    /// <summary>
    /// The start of the billing period.
    /// </summary>
    public DateTime BillingPeriodStart { get; set; }
    /// <summary>
    /// The end of the billing period.
    /// </summary>
    public DateTime BillingPeriodEnd { get; set; }
    /// <summary>
    /// The amount billed in this invoice.
    /// </summary>
    public long AmountBilledCents { get; set; }
    /// <summary>
    /// The amount paid for this invoice.
    /// </summary>
    public long AmountPaidCents { get; set; }
    /// <summary>
    /// The status of the invoice.
    /// </summary>
    public InvoiceStatus Status { get; set; }

    public Invoice(){}
}

public enum InvoiceStatus
{
    Invoiced,
    Paid,
    Overdue
}