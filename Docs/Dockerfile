FROM nginx:alpine AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install docfx
RUN dotnet tool install --global docfx

# Copy over content
COPY . /src/

# Build project
WORKDIR /src
RUN dotnet build MoonCore.sln

# Gather metadata
WORKDIR /src/Docs
RUN ~/.dotnet/tools/docfx metadata

# Build application data
RUN ~/.dotnet/tools/docfx build

FROM base AS final

COPY --from=build /src/Docs/_site/ /usr/share/nginx/html