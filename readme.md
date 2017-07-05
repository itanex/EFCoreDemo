# Entity Framework Core Demo

This Project demonstrates the use of EF Core with ASP.NET Core.

The Master branch is updated with the latest Visual Studio solution and project formats. If you are looking at previous versions of Visual Studio there are branches that are holding old references. Each branch is ensured to have a working solution so that you do not have to worry about getting it up and running.


	// Required because NewtonSoft library does not handle cyclical references
	// MVC by default suppresses the exception and returns what it had serialized up to the exception
	.AddJsonOptions(options => 
		options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
	);