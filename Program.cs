namespace EFPerformance;

public static class Config
{
    public const string ConnectionString = "server=localhost;user=root;password=example;database=ef_perf";
    public const int TestCount = 1; // 每次读取的数据量
    public const int TestRounds = 100; // 测试次数，结构去取均值
    public const int WarmupRounds = 10; // 预热次数

    /*
    * 单条 Customer 数据量：37 = 1 (Customer) + 1 (Group) + 5 (Comment) + 5 (Order) + 5 * 5 (OrderItem)
    * 每次查询数据量为: 37 * TestCount
    * 总数据量为: 37 * CustomerCount
    */

    public const int CustomerCount = 1000; // Customer 数据量
    public const int CommentPerGroup = 5; // Comment 一对多数据量
    public const int OrderPerCustom = 5; // Order 一对多数据量
    public const int OrderItemPerOrder = 5; // OrderItem 一对多数据量

}

public static class Program
{
    static void Main()
    {
        // 请先将本地数据库配置到 ConnectionString
        // 然后执行以下代码，构建数据库表结构，并插入测试数据
        // 初始化数据库后，可注释以下代码，直接执行性能测试

        // SetupDatabase();
        // return;

        Watcher.AddDataSource("Dapper", new DapperDataSource());
        Watcher.AddDataSource("Ado", new AdoDataSource());
        Watcher.AddDataSource("EF Tracking", new EFDataSourceTracking());
        Watcher.AddDataSource("EF NoTracking", new EFDataSourceNoTracking());
        Watcher.AddDataSource("EF FromRawSQL", new EFRawDataSource());

        Console.WriteLine($"每次查询数据量: {37 * Config.TestCount}");
        Console.WriteLine($"开始 {Config.WarmupRounds} 轮预热测试");

        for (var i = 0; i < Config.WarmupRounds; i++)
        {
            Watcher.RunRound();
        }

        Watcher.ClearResult();

        Console.WriteLine($"预热结束，开始 {Config.TestRounds} 轮正式测试");

        for (var i = 0; i < Config.TestRounds; i++)
        {
            Watcher.RunRound();
        }

        Watcher.PrintResult();
    }

    public static string SQL = $@"
SELECT
    `t`.`CustomerId` as CustomerId1,
    `t`.`Address` as Address2,
    `t`.`Email` as Email3,
    `t`.`Mobile` as Mobile4,
    `t`.`Name` as Name5,
    `g`.`GroupId` as GroupId6,
    `g`.`CustomerId` as CustomerId7,
    `g`.`GroupName` as GroupName8,
    `c0`.`CommentId` as CommentId9,
    `c0`.`Content` as Content10,
    `c0`.`CreatedAt` as CreatedAt11,
    `c0`.`CreatedBy` as CreatedBy12,
    `c0`.`GroupId` as GroupId13,
    `c0`.`OrderId` as OrderId14,
    `c0`.`UpdatedAt` as UpdatedAt15,
    `t0`.`OrderId` as OrderId16,
    `t0`.`CreatedAt` as CreatedAt17,
    `t0`.`CustomerId` as CustomerId18,
    `t0`.`Field1` as Field119,
    `t0`.`Field2` as Field220,
    `t0`.`Field3` as Field321,
    `t0`.`Field4` as Field422,
    `t0`.`Field5` as Field523,
    `t0`.`Field6` as Field624,
    `t0`.`Field7` as Field725,
    `t0`.`Field8` as Field826,
    `t0`.`OrderName` as OrderName27,
    `t0`.`Status` as Status28,
    `t0`.`UpdatedAt` as UpdatedAt29,
    `t0`.`Id` as Id30,
    `t0`.`CreatedAt0` as CreatedAt031,
    `t0`.`Description` as Description32,
    `t0`.`Name` as Name33,
    `t0`.`OrderId0` as OrderId034,
    `t0`.`Price` as Price35,
    `t0`.`Status0` as Status036
FROM (
    SELECT `c`.`CustomerId`, `c`.`Address`, `c`.`Email`, `c`.`Mobile`, `c`.`Name`
    FROM `Customers` AS `c`
    LIMIT {Config.TestCount}
) AS `t`
LEFT JOIN `Groups` AS `g` ON `t`.`CustomerId` = `g`.`CustomerId`
LEFT JOIN `Comments` AS `c0` ON `g`.`GroupId` = `c0`.`GroupId`
LEFT JOIN (
    SELECT `o`.`OrderId`, `o`.`CreatedAt`, `o`.`CustomerId`, `o`.`Field1`, `o`.`Field2`, `o`.`Field3`, `o`.`Field4`, `o`.`Field5`, `o`.`Field6`, `o`.`Field7`, `o`.`Field8`, `o`.`OrderName`, `o`.`Status`, `o`.`UpdatedAt`, `o0`.`Id`, `o0`.`CreatedAt` AS `CreatedAt0`, `o0`.`Description`, `o0`.`Name`, `o0`.`OrderId` AS `OrderId0`, `o0`.`Price`, `o0`.`Status` AS `Status0`
    FROM `Orders` AS `o`
    LEFT JOIN `OrderItems` AS `o0` ON `o`.`OrderId` = `o0`.`OrderId`
) AS `t0` ON `t`.`CustomerId` = `t0`.`CustomerId`
ORDER BY `t`.`CustomerId`, `g`.`GroupId`, `c0`.`CommentId`, `t0`.`OrderId`
    ";

    public static void SetupDatabase()
    {
        var context = new EFContext();

        Console.WriteLine("database refreshing");

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("database refreshed");

        var customers = Enumerable.Range(1, Config.CustomerCount).Select(i => new Customer
        {
            Name = $"Customer {i}",
            Mobile = $"+1-800-{i}",
            Email = $"custom_email_{i}@mail.com",
            Address = $"country_province_city_street_{i}",
            Group = new()
            {
                GroupName = $"Group {i}",
                Comments = Enumerable.Range(1, Config.CommentPerGroup).Select(j => new Comment
                {
                    Content = $"Comment {j}",
                    CreatedBy = $"User {j}",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }).ToList()
            },
            Orders = Enumerable.Range(1, Config.OrderPerCustom).Select(j => new Order
            {
                OrderName = $"Order {j}",
                Status = 1,
                Field1 = $"Field1 {j}",
                Field2 = $"Field2 {j}",
                Field3 = $"Field3 {j}",
                Field4 = $"Field4 {j}",
                Field5 = $"Field5 {j}",
                Field6 = $"Field6 {j}",
                Field7 = $"Field7 {j}",
                Field8 = $"Field8 {j}",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Items = Enumerable.Range(1, Config.OrderItemPerOrder).Select(k => new OrderItem
                {
                    Name = $"Item {k}",
                    Description = $"Description {k}",
                    Price = k * 10,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                }).ToList()
            }).ToList()
        }).ToArray();

        foreach (var chunked in customers.Chunk(50))
        {
            context.AddRange(chunked);
            context.SaveChanges();
            Console.WriteLine($"Inserted customers {chunked.Last().Name}");
        }
    }

    static T Next<T>(this T[] source)
    {
        return source[Random.Shared.Next(0, source.Length)];
    }

    static T[] Next<T>(this T[] source, int count)
    {
        return source.OrderBy(item => Random.Shared.Next()).Take(count).ToArray();
    }

}
