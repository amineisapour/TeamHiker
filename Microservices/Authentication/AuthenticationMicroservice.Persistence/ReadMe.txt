﻿Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools

in PM: (AuthenticationMicroservice.Persistence)
Add-Migration InitialMigration -context AuthenticationMicroservice.Persistence.DatabaseContext
Update-Database -Project AuthenticationMicroservice.Persistence -context AuthenticationMicroservice.Persistence.DatabaseContext

OR

PM> Add-Migration mig333 -Project AuthenticationMicroservice.Persistence
PM> Add-Migration mig333 -Project AuthenticationMicroservice.Persistence -context AuthenticationMicroservice.Persistence.DatabaseContext

PM> Add-Migration mig20250130 -Project AuthenticationMicroservice.Persistence -StartupProject AuthenticationMicroservice.Api -context AuthenticationMicroservice.Persistence.DatabaseContext

Update-Database -Project AuthenticationMicroservice.Persistence -StartupProject AuthenticationMicroservice.Api -context AuthenticationMicroservice.Persistence.DatabaseContext