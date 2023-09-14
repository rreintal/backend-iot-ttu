FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

COPY App.BLL/*.csproj ./App.BLL/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY BLL.DTO/*.csproj ./BLL.DTO/
COPY Base.Contracts/*.csproj ./Base.Contracts/
COPY Base.DAL.EF.Contracts/*.csproj ./Base.DAL.EF.Contracts/
COPY DAL.DTO/*.csproj ./DAL.DTO/
COPY IDomainEntityId/*.csproj ./IDomainEntityId/
COPY WebApp/*.csproj ./WebApp/
COPY App.BLL.Contracts/*.csproj ./App.BLL.Contracts/
COPY App.DAL.EF.Contracts/*.csproj ./App.DAL.EF.Contracts/
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY PUBLIC.DTO/*.csproj ./PUBLIC.DTO/
COPY App.DAL.Contracts/*.csproj ./App.DAL.Contracts/
COPY App.Domain/*.csproj ./App.Domain/
COPY Base.BLL.Contracts/*.csproj ./Base.BLL.Contracts/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain.Contracts/*.csproj ./Base.Domain.Contracts/

RUN dotnet restore

COPY App.BLL/. ./App.BLL/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY BLL.DTO/. ./BLL.DTO/
COPY Base.Contracts/. ./Base.Contracts/
COPY Base.DAL.EF.Contracts/. ./Base.DAL.EF.Contracts/
COPY DAL.DTO/. ./DAL.DTO/
COPY IDomainEntityId/. ./IDomainEntityId/
COPY WebApp/. ./WebApp/
COPY App.BLL.Contracts/. ./App.BLL.Contracts/
COPY App.DAL.EF.Contracts/. ./App.DAL.EF.Contracts/
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.Domain/. ./Base.Domain/
COPY PUBLIC.DTO/. ./PUBLIC.DTO/
COPY App.DAL.Contracts/. ./App.DAL.Contracts/
COPY App.Domain/. ./App.Domain/
COPY Base.BLL.Contracts/. ./Base.BLL.Contracts/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain.Contracts/. ./Base.Domain.Contracts/


WORKDIR /src/WebApp

# this is for thumbnail generating, if this is not done then thumbnail generation will not work
RUN apt-get update && apt-get install -y libfontconfig1

RUN dotnet publish -c Release -o out



#FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64 AS runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /src/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]