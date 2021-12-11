# Infinite-runtime-tagviewer

# Download

In-development build (Click to download):

[![Download latest build](https://github.com/Gamergotten/Infinite-runtime-tagviewer/actions/workflows/dotnet.yml/badge.svg)](https://nightly.link/Gamergotten/Infinite-runtime-tagviewer/workflows/dotnet/master/Binaries.zip)

Stable (maybe) Releases:

https://github.com/Gamergotten/Infinite-runtime-tagviewer/releases

# Read me

figured i'd give this a go, seeing as everyone and their mothers are making Assembly type tools for halo infinite


the point of this tool is to lay-out all of the relevant *runtime* tag info into a readable format.

this tool does not rely on module files to read tags because we read everything from the games memory.
however it does partly require the TagID's and their respective Tagnames from the modules, as they do not appear to be in Halo Infinite's runtime memory.

thats about it lol

enormous thanks to Krevil for his tool (https://github.com/Krevil/InfiniteModuleReader) that allows us to dump the tag names&IDs, so we can see the correct names of each tag and easily redump the info anytime theres a change in the module information


https://nightly.link/Gamergotten/Infinite-runtime-tagviewer/workflows/dotnet/master/Binaries.zip
