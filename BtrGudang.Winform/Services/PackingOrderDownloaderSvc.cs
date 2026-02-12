using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Configuration;

namespace BtrGudang.Winform.Services
{
    public class PackingOrderDownloaderSvc
    {
        private readonly RegistryHelper _registryHelper;

        public async Task<(bool, string, List<PackingOrderModel>)> Execute(Periode periode)
        {
            //var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            //var baseUrl = System.Configuration.ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            //var endpoint = $"{baseUrl}/api/Order/incremental/{{tgl1}}/{{tgl2}}/{{serverId}}";
            //var client = new RestClient(endpoint);

            //var request = new RestRequest()
            //    .AddUrlSegment("tgl1", periode.Tgl1.ToString("yyyy-MM-dd"))
            //    .AddUrlSegment("tgl2", periode.Tgl2.ToString("yyyy-MM-dd"))
            //    .AddUrlSegment("serverId", serverTargetId);
            //var response = await client.ExecuteGetAsync(request);

            //if (!response.IsSuccessful)
            //{
            //    return (false, response.ErrorMessage ?? response.StatusDescription, null);
            //}

            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //};
            //var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PackingOrderModel>>>(response.Content, options);

            //if (apiResponse == null)
            //{
            //    return (false, "Failed to deserialize API response", null);
            //}

            //if (apiResponse.Status?.ToLower() != "success")
            //{
            //    return (false, $"API returned non-success status: {apiResponse.Status}", null);
            //}

            //return (true, "", apiResponse.Data ?? new List<PackingOrderModel>());
            await Task.Delay(1000);

            // For demo purposes - replace with actual implementation
            return (true,
                   "Download completed successfully",
                   new List<PackingOrderModel>
                   {
                       Faker1(),
                       Faker2()
                   });
        }
        public PackingOrderModel Faker1()
        {
            var listItem = new List<PackingOrderModel>();
            return new PackingOrderModel("A000139", new DateTime(2026, 2, 11), 
                CustomerReff.Default, LocationReff.Default, FakturReff.Default, 
                new DateTime(2026,2,2), "JOG",
                new List<PackingOrderItemModel>());
        }

        public PackingOrderModel Faker2()
        {
            var listItem = new List<PackingOrderModel>();
            return new PackingOrderModel("A000144", new DateTime(2026, 2, 11), 
                CustomerReff.Default, LocationReff.Default, FakturReff.Default,
                new DateTime(2026,2,3), "JOG",
                new List<PackingOrderItemModel>());
        }
    }

    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public T Data { get; set; }
    }
}