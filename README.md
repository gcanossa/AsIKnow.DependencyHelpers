# AsIKnow.DependencyHelpers

Library intended to check for service dependencies at runtime.

## Usage ##

In order to configure a blocking check it is necessary to configure a DependencyChecker.

<pre>
	DependencyCheckerBuilder builder = new DependencyCheckerBuilder(ServiceProvider, Options.DependencyCheckOptions);

	//configuration
	builder.AddDependencyCheck(new FileExistsDependencyCheck("./pidfile", "pid file exists", TimeSpan.FromSeconds(20)));

	DependencyChecker = builder.Build();
</pre>

Once a _DependencyChecker_ is available it can be awaited for.

<pre>
	DependencyChecker.WaitForDependencies();
</pre>

The various packages provides various convenience extensions method to the _DependencyCheckerBuilder_ class.
It can also be used on a _IWebHost_ instance, to simplify aspent core application depedency checks.

<pre>
    public class Program
    {
        public static void Main(string[] args)
        {
            Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();

            BuildWebHost(args)
                .CheckDependencies(builder => builder
                        .AddEntityFramewrokDbContext<ApplicationDbContext>("database",true)
                            .WithoutEntityFrameworkPostCheckOperation()
                        .AddDistributedCache<RedisCache>("cache")
                    )
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
</pre>