using System;

namespace BtrGudang.Winform.Domain
{
    public class FakturType : IFakturKey
    {
        public FakturType(string fakturId, string fakturCode, DateTime fakturDate)
        {
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
        }

        public static FakturType Default => new FakturType(
            "-", "-", new DateTime(3000, 1, 1));

        public static IFakturKey Key(string id)
        {
            var result = Default;
            result.FakturId = id;
            return result;
        }

        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
    }

    public interface IFakturKey
    {
        string FakturId { get; }
    }
}

