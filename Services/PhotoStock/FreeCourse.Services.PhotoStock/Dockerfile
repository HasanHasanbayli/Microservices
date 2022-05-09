FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/PhotoStock/FreeCourse.Services.PhotoStock/*.csproj Services/PhotoStock/FreeCourse.Services.PhotoStock/
RUN dotnet restore Services/PhotoStock/FreeCourse.Services.PhotoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/FreeCourse.Services.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runetime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.Services.PhotoStock.dll" ]

