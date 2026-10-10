FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DenunciaPeAPI.sln ./
COPY DenunciaPeAPI/DenunciaPeAPI.csproj DenunciaPeAPI/
COPY DenunciaPeAPI.Application/DenunciaPeAPI.Application.csproj DenunciaPeAPI.Application/
COPY DenunciaPeAPI.Domain/DenunciaPeAPI.Domain.csproj DenunciaPeAPI.Domain/
COPY DenunciaPeAPI.Infrastructure/DenunciaPeAPI.Infrastructure.csproj DenunciaPeAPI.Infrastructure/
RUN dotnet restore DenunciaPeAPI/DenunciaPeAPI.csproj

COPY . .
RUN dotnet publish DenunciaPeAPI/DenunciaPeAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production
# "localhost" apuntaría al propio contenedor; se usa el nombre del contenedor SQL Server. "\$" escapa el $ en el Dockerfile.
ENV ConnectionStrings__DefaultConnection="Server=sqlserverUPC,1433;Database=dbDenunciaPe;User Id=sa;Password=P@\$\$w0rD;TrustServerCertificate=True;Encrypt=True;"
EXPOSE 8081

USER $APP_UID
ENTRYPOINT ["dotnet", "DenunciaPeAPI.dll"]
