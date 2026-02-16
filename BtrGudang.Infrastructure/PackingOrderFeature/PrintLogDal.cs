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
    public interface IPrintLogDal :
        IInsert<PrintLogType>,
        IUpdate<PrintLogType>,
        IDelete<IPrintLogKey>,
        IGetData<PrintLogType, IPrintLogKey>,
        IListData<PrintLogType>
    {
    }

    public class PrintLogDal : IPrintLogDal
    {
        private readonly DatabaseOptions _opt;

        public PrintLogDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PrintLogType dto)
        {
            const string sql = @"
               INSERT INTO BTRG_PrintLog(
                   PrintLogId, PrintLogTimestamp, DocType)
               VALUES( 
                   @PrintLogId, @PrintLogTimestamp, @DocType)
               ";

            var dp = new DynamicParameters();
            dp.AddParam("@PrintLogId", dto.PrintLogId, SqlDbType.VarChar);
            dp.AddParam("@PrintLogTimestamp", dto.PrintLogTimestamp, SqlDbType.DateTime);
            dp.AddParam("@DocType", dto.DocType, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PrintLogType dto)
        {
            const string sql = @"
                   UPDATE 
                       BTRG_PrintLog
                   SET
                       PrintLogTimestamp = @PrintLogTimestamp,
                       DocType = @DocType
                   WHERE
                       PrintLogId = @PrintLogId
                   ";

            var dp = new DynamicParameters();
            dp.AddParam("@PrintLogId", dto.PrintLogId, SqlDbType.VarChar);
            dp.AddParam("@PrintLogTimestamp", dto.PrintLogTimestamp, SqlDbType.DateTime);
            dp.AddParam("@DocType", dto.DocType, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPrintLogKey key)
        {
            const string sql = @"
               DELETE FROM 
                    BTRG_PrintLog
               WHERE
                   PrintLogId = @PrintLogId
               ";

            var dp = new DynamicParameters();
            dp.AddParam("@PrintLogId", key.PrintLogId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PrintLogType GetData(IPrintLogKey key)
        {
           const string sql = @"
               SELECT
                   PrintLogId,
                   PrintLogTimestamp,
                   DocType
               FROM 
                   BTRG_PrintLog
               WHERE
                   PrintLogId = @PrintLogId
               ";

            var dp = new DynamicParameters();
            dp.AddParam("@PrintLogId", key.PrintLogId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var result = conn.ReadSingle<PrintLogType>(sql, dp);
                return result;
            }
        }

        public IEnumerable<PrintLogType> ListData()
        {
            const string sql = @"
                SELECT
                    PrintLogId,
                    PrintLogTimestamp,
                    DocType
                FROM 
                    BTRG_PrintLog
                ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PrintLogType>(sql);
            }
        }
    }
}
