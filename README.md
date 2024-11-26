![Nuget](https://img.shields.io/nuget/v/Unicorn.Taf.Api?style=plastic)
![Nuget](https://img.shields.io/nuget/dt/Unicorn.Taf.Api?style=plastic)

# Unicorn.Taf.Api

Common API of Unicorn test automation framework.

* Common interfaces

* AppDomain isolation for .NET Framework: `UnicornAppDomainIsolation`  
	> Example of use based on Unicorn test runner
	```csharp
	// Getting base directory for the AppDomain (here test assembly dir)
	string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

	// Create new AppDomain
	UnicornAppDomainIsolation<AppDomainRunner> runnerIsolation =
		new UnicornAppDomainIsolation<AppDomainRunner>(assemblyDirectory, "Unicorn.TestAdapter Runner AppDomain");

	// And execute tests
	return runnerIsolation.Instance.RunTests(assemblyPath, testsMasks, unicornConfig);
	```
	
* Custom AssemblyLoadContext for .NET Core and .NET: `UnicornAssemblyLoadContext`  
	> Example of use based on Unicorn test runner
	```csharp
	// Getting base directory for the context (here test assembly dir)
	string contextDirectory = Path.GetDirectoryName(assemblyPath);

	// Initialize AssemblyLoadContext
	UnicornAssemblyLoadContext runnerContext = new UnicornAssemblyLoadContext(contextDirectory);
	runnerContext.Initialize(typeof(ITestRunner));

	// Get test assembly
	AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
	Assembly testAssembly = runnerContext.GetAssembly(assemblyName);

	// Get type of test runner implementation from the context and create an istance
	Type runnerType = runnerContext.GetAssemblyContainingType(typeof(LoadContextRunner))
		.GetTypes()
		.First(t => t.Name.Equals(typeof(LoadContextRunner).Name));

	ITestRunner runner = Activator.CreateInstance(runnerType, testAssembly, testsMasks, unicornConfig) as ITestRunner;

	// And execute tests
	IOutcome ioutcome = runner.RunTests();
	```
