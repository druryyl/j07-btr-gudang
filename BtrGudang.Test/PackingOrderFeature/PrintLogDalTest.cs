using Bilreg.Infrastructure.Shared.Helpers;
using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;
using BtrGudang.Infrastructure.PackingOrderFeature;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BtrGudang.Test.PackingOrderFeature
{
    public class PrintLogDalTest
    {
        private readonly PrintLogDal _sut = new PrintLogDal(ConnStringHelper.GetTestEnv());

        private static PrintLogType Faker()
            => new PrintLogType("A", new DateTime(2023, 1, 1), "B");

        private static IPrintLogKey FakerKey()
            => new PrintLogType("A", new DateTime(2023, 1, 1), "B");

        [Fact]
        public void InsertTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
            }
        }

        [Fact]
        public void UpdateTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Update(Faker());
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Delete(FakerKey());
            }
        }

        [Fact]
        public void GetDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
                var actual = _sut.GetData(FakerKey());
                actual.Should().BeEquivalentTo(Faker());
            }
        }

        [Fact]
        public void ListDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
                var actual = _sut.ListData();
                actual.Should().ContainEquivalentOf(Faker());
            }
        }
    }
}
