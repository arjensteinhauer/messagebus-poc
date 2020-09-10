@echo off
set /p name="Name: "
dotnet ef migrations add %name% --context TenantContext --project ".\MB.Access.Tenant.Database.csproj" --startup-project "..\..\..\Microservice\Message1\WebJob\MB.Microservice.Message1.WebJob.csproj"
