using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class AccountingDAL
    {
        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://localhost:44361");
            return client;
        }

        public async Task<List<AccountsPayable>> GetPayables()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/payables");
            List<AccountsPayable> payablesList = await response.Content.ReadAsAsync<List<AccountsPayable>>();
            return payablesList;
        }

        public async Task<AccountsPayable> GetPayable(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/payables/{id}");
            if(response.IsSuccessStatusCode)
            {
                var payable = await response.Content.ReadAsAsync<AccountsPayable>();
                return payable;
            }
            else
            {
                return null;
            }
        }

        public async Task<AccountsPayable> AddPayable(AccountsPayable payable)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/payables", payable);
            var payableResult = await response.Content.ReadAsAsync<AccountsPayable>();
            return payableResult;
             
        }

        public async Task<bool> DeletePayable (int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/payables/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdatePayable (AccountsPayable payable)
        {
            HttpClient client = GetHttpClient();
             await client.PutAsJsonAsync("api/payables", payable);

        }
        public async Task<List<AccountsReceivable>> GetReceivables()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/receivables");
            List<AccountsReceivable> receivablesList = await response.Content.ReadAsAsync<List<AccountsReceivable>>();
            return receivablesList;
        }

        public async Task<AccountsReceivable> GetReceivable(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/receivables/{id}");
            if (response.IsSuccessStatusCode)
            {
                var receivable = await response.Content.ReadAsAsync<AccountsReceivable>();
                return receivable;
            }
            else
            {
                return null;
            }
        }

        public async Task<AccountsReceivable> AddReceivable(AccountsReceivable receivable)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/receivables", receivable);
            var receivableResult = await response.Content.ReadAsAsync<AccountsReceivable>();
            return receivableResult;

        }

        public async Task<bool> DeleteReceivable(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/receivables/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdateReceivables(AccountsReceivable receivable)
        {
            HttpClient client = GetHttpClient();
            await client.PutAsJsonAsync("api/receivables", receivable);

        }
        public async Task<List<Cash>> GetCashTransactions()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/bank");
            List<Cash> cashList = await response.Content.ReadAsAsync<List<Cash>>();
            return cashList;
        }

        public async Task<Cash> GetCashTransaction(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/bank/{id}");
            if (response.IsSuccessStatusCode)
            {
                var cashTransaction = await response.Content.ReadAsAsync<Cash>();
                return cashTransaction;
            }
            else
            {
                return null;
            }
        }

        public async Task<Cash> AddCashTransaction(Cash cash)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/bank", cash);
            var cashResult = await response.Content.ReadAsAsync<Cash>();
            return cashResult;

        }

        public async Task<bool> DeleteCashTransaction(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/bank/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdateCashTransaction(Cash cash)
        {
            HttpClient client = GetHttpClient();
            await client.PutAsJsonAsync("api/bank", cash);

        }

        public async Task<List<Expenses>> GetExpenses()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/companyexpenses");
            List<Expenses> expenseList = await response.Content.ReadAsAsync<List<Expenses>>();
            return expenseList;
        }

        public async Task<Expenses> GetExpense(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/companyexpenses/{id}");
            if (response.IsSuccessStatusCode)
            {
                var expense = await response.Content.ReadAsAsync<Expenses>();
                return expense;
            }
            else
            {
                return null;
            }
        }

        public async Task<Expenses> AddExpense(Expenses expense)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/companyexpenses", expense);
            var expenseResult = await response.Content.ReadAsAsync<Expenses>();
            return expenseResult;

        }

        public async Task<bool> DeleteExpense(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/companyexpenses/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdateExpenses(Expenses expense)
        {
            HttpClient client = GetHttpClient();
            await client.PutAsJsonAsync("api/companyexpenses", expense);

        }
        public async Task<List<Sales>> GetSales()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/companysales");
            List<Sales> salesList = await response.Content.ReadAsAsync<List<Sales>>();
            return salesList;
        }

        public async Task<Sales> GetSale(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/companysales/{id}");
            if (response.IsSuccessStatusCode)
            {
                var sale = await response.Content.ReadAsAsync<Sales>();
                return sale;
            }
            else
            {
                return null;
            }
        }

        public async Task<Sales> AddSale(Sales sale)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/companysales", sale);
            var saleResult = await response.Content.ReadAsAsync<Sales>();
            return saleResult;

        }

        public async Task<bool> DeleteSale(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/companysales/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdateSales(Sales sale)
        {
            HttpClient client = GetHttpClient();
            await client.PutAsJsonAsync("api/companysales", sale);

        }

        public async Task<List<Vendor>> GetVendors()
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync("api/vendors");
            List<Vendor> vendorList = await response.Content.ReadAsAsync<List<Vendor>>();
            return vendorList;
        }

        public async Task<Vendor> GetVendor(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/vendors/{id}");
            if (response.IsSuccessStatusCode)
            {
                var vendor = await response.Content.ReadAsAsync<Vendor>();
                return vendor;
            }
            else
            {
                return null;
            }
        }

        public async Task<Vendor> AddVendor(Vendor vendor)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync("api/vendors", vendor);
            var vendorResult = await response.Content.ReadAsAsync<Vendor>();
            return vendorResult;

        }

        public async Task<bool> DeleteVendor(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/vendors/{id}");
            return response.IsSuccessStatusCode;
        }
        public async void UpdateVendor(Vendor vendor)
        {
            HttpClient client = GetHttpClient();
            await client.PutAsJsonAsync("api/vendors", vendor);

        }
    }
}
