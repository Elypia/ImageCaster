<div align="center">

# ImageCaster
[![Matrix]][matrix-community] [![Discord]][discord-guild] [![Docker]][docker-image] [![Build]][gitlab] [![Coverage]][gitlab] [![Donate]][elypia-donate]
</div>

## About
ImageCaster is a small CLI application that can be used to manage
exporting images. It's primary use case is for CI/CD where a
repository that has a set of images as a source and programmatically
produces images as a result of a build.

An example and motivation for this is the [Elypia Emotes] repository
where we have many images, which we'd like exported in many
sizes and colors, as well as to dynamically generate montages to
display on the project README.

This project is not an alternative to [Magick.NET] or [ImageMagick], it
is a different way of interfacing with them. If you're looking for a
programming interface then you may prefer [Magick.NET], or if you're
looking for a tool executed purely from CLI, please visit [ImageMagick].
ImageCaster is an abstraction of the two to provide the features though
a declarative configuration file instead for simpler CI/CD usage as
well as some management for consecutive builds and quality assurance.

## Download
There are 3 versions available of ImageCaster which correlate with the 3 [ImageMagick] versions.
Q16-HDRI is the default version and recommended for general use, however Q16 and Q8
are available for environments where limited resources are available, or minimal
resource usage and build times are critical so long as they are appropriate for your use case.
More information can be found here on the [ImageMagick website].

### ImageCaster Command Line Interface (CLI)
[![Download Q16-HDRI]][cli-q16-hdri-download] [![Download Q16]][cli-q16-download] [![Download Q8]][cli-q8-download]

### ImageCaster Server
[![Download Q16-HDRI]][api-q16-hdri-download] [![Download Q16]][api-q16-download] [![Download Q8]][api-q8-download]

## Features
* Instead of scripting, define a declarative configuration that
describes the output you want.
* Quicker consecutive builds, ImageCaster will manage if changes
occurred and if re-exported again is required.
* Set [Exif] data for output, for example the `Copyright` or `Artist`
tag.
* Define patterns (directory/[glob]/[regex]) to match images and create
montages to display to users.
* Define patterns (directory/[glob]/[regex]) to match images and
archive images for download.
* Define checks to ensure your repository standards are maintained and
to avoid mistakes like mismatching names.

## Example
### Simple
```yml
build:
  input:
    - twitch/emotes/
  sizes:
    dimensions: [112, 56, 28]
```
> Build all of the images made in the 3 sizes, 112px, 56px, and 28px.

### Advanced
```yml
build:
  input:
    - src/emotes/
  metadata:
    exif:
      - {tag: Artist, value: Elypia CIC and Contributors}
  resize:
    filter: Catrom
    geometries: [512, 258]
  recolor:
    mask:
      sources:
        - src/masks/
    modulation:
      - {name: blue, hue: 0}
      - {name: violet, saturation: 70, hue: 50}
checks:
  file-exists:
    source: regex:src/mask/(.+?)\\..+
    target: src/emotes/
    pattern: $1.png
```
> First we define a pattern which matches all of our input images. For
> each image we add the Exif tag, `Artist`, and export 6 versions of
> each image to accommodate original, blue, and violet colors, in 512px,
> and 128px sizes. Doing `imagecaster build` will do all of this for
> you. You may wish to use `imagecaster check` first to perform all
> checks, in this case only one check is defined which states if a mask
> exists, then an image must also exist with the respective name.
> By default the full filename is $0, the main name

## Open-Source
This project is open-source under the [Apache 2.0] license!
While not legal advice, you can find a [TL;DR] that sums up what
you're allowed and not allowed to do along with any requirements if you
want to use or derive work from this repository!

[matrix-community]: https://matrix.to/#/+elypia:matrix.org "Matrix Invite"
[discord-guild]: https://discord.com/invite/hprGMaM "Discord Invite"
[docker-image]: https://hub.docker.com/r/elypia/imagecaster "ImageCaster on Docker"
[gitlab]: https://gitlab.com/Elypia/imagecaster/commits/master "Repository on GitLab"
[elypia-donate]: https://elypia.org/donate "Donate to Elypia"
[Elypia Emotes]: https://gitlab.com/Elypia/elypia-emotes "Elypia Emotes"
[Magick.NET]: https://github.com/dlemstra/Magick.NET "Magick.NET on GitHub"
[ImageMagick]: https://github.com/ImageMagick "ImageMagick on GitHub"
[ImageMagick website]: https://imagemagick.org/ "ImageMagick Website"
[cli-q16-hdri-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-cli-q16-hdri "Download ImageCaster CLI with ImageMagick Q16-HDRI"
[cli-q16-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-cli-q16 "Download ImageCaster CLI with ImageMagick Q16"
[cli-q8-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-cli-q8 "Download ImageCaster CLI with ImageMagick Q8"
[api-q16-hdri-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-api-q16-hdri "Download ImageCaster API with ImageMagick Q16-HDRI"
[api-q16-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-api-q16 "Download ImageCaster API with ImageMagick Q16"
[api-q8-download]: https://gitlab.com/Elypia/imagecaster/-/jobs/artifacts/master/download?job=build-api-q8 "Download ImageCaster API with ImageMagick Q8"
[Exif]: https://en.wikipedia.org/wiki/Exif "Exif on Wikipedia"
[glob]: https://en.wikipedia.org/wiki/Glob_(programming) "Glob on Wikipedia"
[regex]: https://en.wikipedia.org/wiki/Regular_expression "Regular Expression on Wikipedia"
[Apache 2.0]: https://www.apache.org/licenses/LICENSE-2.0 "Apache 2.0 License"
[TL;DR]: https://tldrlegal.com/license/apache-license-2.0-(apache-2.0) "TL;DR of Apache 2.0"

[Matrix]: https://img.shields.io/matrix/elypia:matrix.org?logo=matrix "Matrix Shield"
[Discord]: https://discord.com/api/guilds/184657525990359041/widget.png "Discord Shield"
[Docker]: https://img.shields.io/docker/pulls/elypia/imagecaster?logo=docker "Docker Shield"
[Build]: https://gitlab.com/Elypia/imagecaster/badges/master/pipeline.svg "GitLab Build Shield"
[Coverage]: https://gitlab.com/Elypia/imagecaster/badges/master/coverage.svg "GitLab Coverage Shield"
[Donate]: https://img.shields.io/badge/elypia-donate-blueviolet "Donate Shield"
[Download Q16-HDRI]: https://img.shields.io/badge/Download-Q16--HDRI-blue "Download Q16-HDRI"
[Download Q16]: https://img.shields.io/badge/Download-Q16-blue "Download Q16"
[Download Q8]: https://img.shields.io/badge/Download-Q8-blue "Download Q8"
