using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SemanticTest
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }
        public async Task<Guid> RequestPaymentFromUserAsync(Guid cartId)
        {

            _logger.LogInformation("Llamando al pago");
            // Simula el proceso de pago
            return await Task.FromResult(Guid.NewGuid());
        }
    }


}