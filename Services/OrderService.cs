using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Microsoft.EntityFrameworkCore;
using PreCompiledQuery.Data;
using PreCompiledQuery.Entities;
using BenchmarkDotNet.Environments;

namespace PreCompiledQuery.Services
{
    [MemoryDiagnoser] //info on memory alloc. and garbage collections
    //[Config(typeof(OrderServiceConfig))]
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

        [Benchmark(Baseline = true)]
        public async Task<List<Order>> GetSimpleOrders()
        {
            return await _appDbContext.Orders.AsNoTracking().ToListAsync();
        }

        // Compiled query delegate
        private static readonly Func<AppDbContext, IAsyncEnumerable<Order>> GetOrders
            = EF.CompileAsyncQuery((AppDbContext context) => context.Orders.AsNoTracking());

        [Benchmark]
        public IAsyncEnumerable<Order> GetOrdersFromCompiledQuery()
        {
            return GetOrders(_appDbContext);
        }
    }

    /// <summary>
    /// Custom configuration attribute
    /// </summary>
    public class OrderServiceConfig: ManualConfig
    {
        public OrderServiceConfig()
        {
            AddJob(Job
              .Default
              .WithId("x64-Core50")
              .WithLaunchCount(1)
              .WithIterationCount(1)
              .WithPlatform(Platform.X64)
              .WithRuntime(CoreRuntime.Core50));

            AddLogger(ConsoleLogger.Default);
            AddColumn(TargetMethodColumn.Method, StatisticColumn.Max);
            AddExporter(CsvExporter.Default, HtmlExporter.Default);
        }
    }
}
