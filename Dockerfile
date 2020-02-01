# Build all files for deployment.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101-alpine3.10 AS build

# We'll move builds from here to the next image.
WORKDIR /home/dev/

# Copy source-code over to our container.
COPY ./ ./

# Build all docker releases.
RUN cd src/                                   && \
    dotnet restore                            && \
    cd ImageCaster/                           && \
    dotnet publish -c Release-Q8-Docker       && \
    dotnet publish -c Release-Q16-Docker      && \
    dotnet publish -c Release-Q16-HDRI-Docker

# Start building our final image.
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.1-bionic

LABEL maintainer="seth@elypia.org"

# Add imagecaster user, and install a generic font.
RUN useradd -ms /bin/bash imagecaster && \
    apt-get update                    && \
    apt-get install -y ttf-dejavu     && \
    rm -rf /var/lib/apt/lists/*

USER imagecaster

WORKDIR /home/imagecaster

COPY --from=build /home/dev/src/ImageCaster/bin/Release-Q8-Docker/netcoreapp3.1/linux-x64/publish/* /usr/local/bin/
