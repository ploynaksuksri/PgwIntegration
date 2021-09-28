using PwgIntegration.Shared.Models.PaymentToken;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PgwIntegration._2c2p
{
    public class PaymentRepository
    {
        protected string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddPayemntTokenResponse(string invoiceNo, PaymentTokenResponse response)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var query = @"INSERT INTO [payment].[GatewayResponse] (InvoiceNo, PaymentToken, WebPaymentUrl) VALUES  (@InvoiceNo, @PaymentToken, @WebPaymentUrl)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                command.Parameters.AddWithValue("@PaymentToken", response.PaymentToken);
                command.Parameters.AddWithValue("@WebPaymentUrl", response.WebPaymentUrl);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception($"SqlException: {ex.Message}");
            }
        }

        public async Task UpdateResponse(string invoiceNo, string jsonResponse)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var query = @"UPDATE [payment].[GatewayResponse] SET JsonResponse = @jsonResponse WHERE InvoiceNo = @invoiceNo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@jsonResponse", jsonResponse);
                command.Parameters.AddWithValue("@invoiceNo", invoiceNo);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception($"SqlException: {ex.Message}");
            }
        }

        public async Task<string> GetPaymentToken(string invoiceNo)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var query = @"SELECT PaymentToken FROM [Payment].[GatewayResponse] WHERE InvoiceNo = @InvoiceNo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return (string)reader["PaymentToken"];
                }
                throw new Exception($"Payment token is not found for invoice no - {invoiceNo}");
            }
            catch (SqlException ex)
            {
                throw new Exception($"SqlException: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    }
}