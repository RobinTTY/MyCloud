﻿using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers
{
    /// <summary>
    /// Provides access to <see cref="Invoice"/> resources.
    /// </summary>
    [ApiController]
    [Route("api/projects/{id}/invoices")]
    [ApiVersion("1.0")]
    [SwaggerTag("Invoices state the amount owed due to the usage of resources.")]
    public class InvoicesController : ControllerBase
    {
        private readonly Database _context;
        private readonly ILogger<ProjectsController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="InvoicesController"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public InvoicesController(Database context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all invoices of a project.
        /// </summary>
        /// <param name="id">The id of the project the users are assigned to.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetInvoices")]
        public ActionResult<List<Invoice>> Get(Guid id)
        {
            return _context.Invoices.Where(invoice => invoice.ProjectId == id).ToList();
        }

        /// <summary>
        /// Get an invoice of a project.
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
