using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers
{
    [ApiController]
    [Route("api/projects/{id}/invoices")]
    [ApiVersion("1.0")]
    public class InvoicesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public InvoicesController(ApiContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the invoices of a project.
        /// </summary>
        /// <param name="id">The id of the project the users are assigned to.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetInvoices")]
        public ActionResult<List<Invoice>> Get(Guid id)
        {
            return _context.Invoices.Where(invoice => invoice.ProjectId == id).ToList();
        }

        /// <summary>
        /// Get an invoice by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceId">The id of the invoice to get.</param>
        /// <returns></returns>
        [HttpGet("{invoiceId}", Name = "GetInvoiceById")]
        public ActionResult<Invoice> GetById(Guid id, Guid invoiceId)
        {
            var invoice = _context.Invoices.FirstOrDefault(invoice => invoice.ProjectId == id && invoice.Id == invoiceId);
            if (invoice != null)
                return invoice;
            return NotFound();
        }
    }
}
