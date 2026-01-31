using BtrGudang.Domain.PackingOrderFeature;
using System;
using System.Collections.Generic;

namespace BtrGudang.Infrastructure.PackingOrderFeature
{
    public class PackingOrderDto 
    {
        public string PackingOrderId { get; set; }
        public DateTime PackingOrderDate { get; set; }
        public string PackingOrderCode { get; set; }

        // Flattened CustomerType properties
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public string NoTelp { get; set; }

        // Flattened FakturType properties
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string AdminName { get; set; }

        // Flattened LocationType properties
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Accuracy { get; set; }

        public static PackingOrderDto FromModel(PackingOrderModel model)
        {
            return new PackingOrderDto
            {
                PackingOrderId = model.PackingOrderId,
                PackingOrderDate = model.PackingOrderDate,
                PackingOrderCode = model.PackingOrderCode,
                CustomerId = model.Customer.CustomerId,
                CustomerCode = model.Customer.CustomerCode,
                CustomerName = model.Customer.CustomerName,
                Alamat = model.Customer.Alamat,
                NoTelp = model.Customer.NoTelp,
                FakturId = model.Faktur.FakturId,
                FakturCode = model.Faktur.FakturCode,
                FakturDate = model.Faktur.FakturDate,
                AdminName = model.Faktur.AdminName,
                Latitude = model.Location.Latitude,
                Longitude = model.Location.Longitude,
                Accuracy = model.Location.Accuracy
            };
        }

        public PackingOrderModel ToModel(IEnumerable<PackingOrderItemModel> listItem)
        {
            var customer = new CustomerType(
                CustomerId, CustomerCode, CustomerName, Alamat, NoTelp);
            var faktur = new FakturType(
                FakturId, FakturCode, FakturDate, AdminName);
            var location = new LocationType(
                Latitude, Longitude, Accuracy);
            return new PackingOrderModel(
                PackingOrderId,
                PackingOrderDate,
                PackingOrderCode,
                customer,
                faktur,
                location,
                listItem);
        }
    }
}
