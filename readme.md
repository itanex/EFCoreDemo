# Entity Framework Core Demo

This Project demonstrates the use of EF Core with ASP.NET Core.

	// Required because NewtonSoft library does not handle cyclilcal references
	// MVC by default supresses the exception and returns what it had serialized up to the exception
	.AddJsonOptions(options => 
		options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
	);