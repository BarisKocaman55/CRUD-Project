# Serenity
Serenity Test Case

# Project References for Layers

1. Entities Layer => Core Layer
2. DataAccess Layer => Core and Entities Layers
3. Core Layer => 
4. Business Layer => DataAccess and Entities Layers
5. Web API => Business and Entities Layers

#Project Packages and Versions
**DataAccess**
- Microsoft.EntityFrameworkCore.SqlServer Version:2.2.6
- Newtonsoft.Json Version:13.0.1
**Business**
- Autofac Version:4.9.4
- Autofac.Extras.DynamicProxy Version:4.5.0
- FluentValidation Version:8.5.0
- Microsoft.AspNetCore.Http Version:2.2.2
**Core**
- Autofac.Extensions.DependencyInjection Version:4.4.0
- Autofac.Extras.DynamicProxy Version:4.5.0
- FluentValidation Version:8.5.0
- Microsoft.AspNetCore.Http Version:2.2.2
- Microsoft.EntityFrameworkCore Version:2.2.6
- Microsoft.EntityFrameworkCore.SqlServer Version:2.2.6
- Microsoft.EntityFrameworkCore.Tools Version:2.2.6
- Microsoft.Extensions.Configuration Version:2.2.0
