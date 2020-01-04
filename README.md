# ImageCaster [![discord-members]][discord] [![gitlab-build]][gitlab] [![gitlab-coverage]][gitlab] [![donate-shield]][elypia-donate]
## About
ImageCaster is a small CLI application that can be used to manage exporting images.
It's primary use case is for CI/CD where a repository that has a set of images as a source
and programatically produces images as a result of a build.

An example, and motivation for this is the Elypia Emotes repository where we have
many images, which we'd like exported in many different sizes and colors, as well as
to dynamically generate images to display on the project README.

## Features
* Instead of scripting, define a declarative configuration that describes the output you want.
* Quicker consecutive builds, ImageCaster will manage if changes occured and if re-exported again is required.
* Set EXIF data for output, for example the `Copyright` or `Artist` tag.
* Define globs to create montages to display to users.
* Define globs to archive images for download.
* Define checks to ensure your repository standards are maintained and to avoid mistakes like mismatching names.

## Example
```yml
export:
  input: "src/static/panda*.png"
  exif:
    - tag: "Artist"
      value: "Elypia CIC and Contributors"
  sizes:
    units: "px"
    dimensions:
      - height: 512
      - height: 128
  color:
    mask: "src/static/masks/panda$1.mask.png"
    colors:
      - name: "blue"
        modulate: "100,100,0"
      - name: "violet"
        modulate: "100,70,50"
checks:
  - name: "file-exists"
    source: "src/static/mask/panda*.mask.png"
    target: "src/static/panda$1.png"
```
> First we define a glob which matches all of our input images.
> For each image we add the EXIF tag, `Artist`, and export 6 versions of each image to 
> accomodate original, blue, and violet colors, in 512px, and 128px sizes.
> Doing `imagecaster build` will do all of this for you.  
> You may wish to use `imagecaster test` first to perform all checks, in this case
> only one check is defined which states if a mask exists, then an image must also exist
> with the respective name.

## Open-Source
This project is open-source under the [GNU General Public License]!  
While not legal advice, you can find a [TL;DR] that sums up what
you're allowed and not allowed to do along with any requirements if you want to 
use or derive work from this source code!  

**All non-code files including videos, models, audio, bitmaps, vectors, and 
animations such as gifs, are not under the aforementioned license; all rights
are reserved by Elypia CIC.** 

[discord]: https://discordapp.com/invite/hprGMaM "Discord Invite"
[gitlab]: https://gitlab.com/Elypia/imagecaster/commits/master "Repository on GitLab"
[elypia-donate]: https://elypia.org/donate "Donate to Elypia"
[GNU General Public License]: https://www.gnu.org/licenses/gpl-3.0.en.html "AGPL"
[TL;DR]: https://tldrlegal.com/license/gnu-general-public-license-v3-(gpl-3) "TLDR of AGPL"

[discord-members]: https://discordapp.com/api/guilds/184657525990359041/widget.png "Discord Shield"
[gitlab-build]: https://gitlab.com/Elypia/imagecaster/badges/master/pipeline.svg "GitLab Build Shield"
[gitlab-coverage]: https://gitlab.com/Elypia/imagecaster/badges/master/coverage.svg "GitLab Coverage Shield"
[donate-shield]: https://img.shields.io/badge/Elypia-Donate-blueviolet "Donate Shield"
