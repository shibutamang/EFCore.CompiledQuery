using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PreCompiledQuery.Data;
using PreCompiledQuery.Entities;
using PreCompiledQuery.Extensions;

namespace PreCompiledQuery.Services
{
    [MemoryDiagnoser] //info on memory alloc. and garbage collections
    public class OrderService
    {
        private AppDbContext _appDbContext;

        [GlobalSetup]
        public void Setup()
        {
            //var config = new ConfigurationBuilder()
            //        .SetBasePath(AppContext.BaseDirectory) // Ensure the correct base path
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //        .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;User ID=postgres;Password=postgres;Database=PrecompiledDB;");
            _appDbContext = new AppDbContext(optionsBuilder.Options);
        }

        [Benchmark]
        public async Task<List<Order>> GetSimpleOrders()
        {
            return await _appDbContext.Orders.AsNoTracking().ToListAsync();
        }

        [Benchmark]
        public IAsyncEnumerable<Order> GetOrdersFromCompiledQuery()
        {
            var r = _appDbContext.CompileAsyncQuery<AppDbContext, Order>(x => x.Orders.AsNoTracking());
            return r(_appDbContext);
        }
    }
}
