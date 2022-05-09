FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY IdentityServer/FreeCourse.IdentityServer/*.csproj IdentityServer/FreeCourse.IdentityServer/
RUN dotnet restore IdentityServer/FreeCourse.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/FreeCourse.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runetime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.IdentityServer.dll" ]

