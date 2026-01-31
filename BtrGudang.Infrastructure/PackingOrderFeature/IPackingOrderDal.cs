using Bilreg.Infrastructure.Shared.Helpers;
using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;
using BtrGudang.Winform.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BtrGudang.Infrastructure.PackingOrderFeature
{
    public interface IPackingOrderDal :
        IInsert<PackingOrderDto>,
        IUpdate<PackingOrderDto>,
        IDelete<IPackingOrderKey>,
        IGetData<PackingOrderDto, IPackingOrderKey>,
        IListData<PackingOrderDto, Periode>
    {
    }

    public class PackingOrderDal : IPackingOrderDal
    {
        private readonly DatabaseOptions _opt;

        public PackingOrderDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PackingOrderDto dto)
        {
            const string sql = @"
                INSERT INTO BTRG_PackingOrder(
                    PackingOrderId, PackingOrderDate, PackingOrderCode,
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    FakturId, FakturCode, FakturDate,
                    Latitude, Longitude, Accuracy)
                VALUES(
                    @PackingOrderId, @PackingOrderDate, @PackingOrderCode,
                    @CustomerId, @CustomerCode, @CustomerName, @Alamat, @NoTelp,
                    @FakturId, @FakturCode, @FakturDate,
                    @Latitude, @Longitude, @Accuracy)
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", dto.PackingOrderId, SqlDbType.VarChar);
            dp.AddParam("@PackingOrderDate", dto.PackingOrderDate, SqlDbType.DateTime);
            dp.AddParam("@PackingOrderCode", dto.PackingOrderCode, SqlDbType.VarChar);

            dp.AddParam("@CustomerId", dto.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", dto.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", dto.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@Alamat", dto.Alamat, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", dto.NoTelp, SqlDbType.VarChar);

            dp.AddParam("@FakturId", dto.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", dto.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", dto.FakturDate, SqlDbType.DateTime);

            dp.AddParam("@Latitude", dto.Latitude, SqlDbType.Decimal);
            dp.AddParam("@Longitude", dto.Longitude, SqlDbType.Decimal);
            dp.AddParam("@Accuracy", dto.Accuracy, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PackingOrderDto dto)
        {
            const string sql = @"
                UPDATE BTRG_PackingOrder
                SET
                    PackingOrderDate = @PackingOrderDate,
                    PackingOrderCode = @PackingOrderCode,
                    CustomerId = @CustomerId,
                    CustomerCode = @CustomerCode,
                    CustomerName = @CustomerName,
                    Alamat = @Alamat,
                    NoTelp = @NoTelp,
                    FakturId = @FakturId,
                    FakturCode = @FakturCode,
                    FakturDate = @FakturDate,
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    Accuracy = @Accuracy
                WHERE
                    PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", dto.PackingOrderId, SqlDbType.VarChar);
            dp.AddParam("@PackingOrderDate", dto.PackingOrderDate, SqlDbType.DateTime);
            dp.AddParam("@PackingOrderCode", dto.PackingOrderCode, SqlDbType.VarChar);

            dp.AddParam("@CustomerId", dto.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", dto.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", dto.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@Alamat", dto.Alamat, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", dto.NoTelp, SqlDbType.VarChar);

            dp.AddParam("@FakturId", dto.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", dto.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", dto.FakturDate, SqlDbType.DateTime);

            dp.AddParam("@Latitude", dto.Latitude, SqlDbType.Decimal);
            dp.AddParam("@Longitude", dto.Longitude, SqlDbType.Decimal);
            dp.AddParam("@Accuracy", dto.Accuracy, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPackingOrderKey key)
        {
            const string sql = @"
                DELETE FROM BTRG_PackingOrder
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PackingOrderDto GetData(IPackingOrderKey key)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, PackingOrderDate, PackingOrderCode,
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    FakturId, FakturCode, FakturDate,
                    Latitude, Longitude, Accuracy
                FROM BTRG_PackingOrder
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PackingOrderDto>(sql, dp);
            }
        }

        public IEnumerable<PackingOrderDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, PackingOrderDate, PackingOrderCode,
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    FakturId, FakturCode, FakturDate,
                    Latitude, Longitude, Accuracy
                FROM BTRG_PackingOrder
                WHERE PackingOrderDate BETWEEN @Tgl1 AND @Tg2
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingOrderDto>(sql, dp);
            }
        }
    }
}