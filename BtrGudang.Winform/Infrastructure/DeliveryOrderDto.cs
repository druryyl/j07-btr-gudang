using BtrGudang.Winform.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Infrastructure
{
    public class DeliveryOrderDto 
    {
        public string DeliveryOrderId { get; set; }
        public DateTime DeliveryOrderDate { get; set; }
        public string DeliveryOrderCode { get; set; }

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

        // Flattened LocationType properties
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Accuracy { get; set; }

        public static DeliveryOrderDto FromModel(Domain.DeliveryOrderModel model)
        {
            return new DeliveryOrderDto
            {
                DeliveryOrderId = model.DeliveryOrderId,
                DeliveryOrderDate = model.DeliveryOrderDate,
                DeliveryOrderCode = model.DeliveryOrderCode,
                CustomerId = model.Customer.CustomerId,
                CustomerCode = model.Customer.CustomerCode,
                CustomerName = model.Customer.CustomerName,
                Alamat = model.Customer.Alamat,
                NoTelp = model.Customer.NoTelp,
                FakturId = model.Faktur.FakturId,
                FakturCode = model.Faktur.FakturCode,
                FakturDate = model.Faktur.FakturDate,
                Latitude = model.Location.Latitude,
                Longitude = model.Location.Longitude,
                Accuracy = model.Location.Accuracy
            };
        }

        public DeliveryOrderModel ToModel(IEnumerable<DeliveryOrderItemModel> listItem)
        {
            var customer = new CustomerType(
                CustomerId, CustomerCode, CustomerName, Alamat, NoTelp);
            var faktur = new FakturType(
                FakturId, FakturCode, FakturDate);
            var location = new LocationType(
                Latitude, Longitude, Accuracy);
            return new DeliveryOrderModel(
                DeliveryOrderId,
                DeliveryOrderDate,
                DeliveryOrderCode,
                customer,
                faktur,
                location,
                listItem);
        }
    }
}
