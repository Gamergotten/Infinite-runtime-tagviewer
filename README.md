# Infinite-runtime-tagviewer

# Download

In-development build (Click to download):

[![Download latest build](https://github.com/Gamergotten/Infinite-runtime-tagviewer/actions/workflows/dotnet.yml/badge.svg)](https://nightly.link/Gamergotten/Infinite-runtime-tagviewer/workflows/dotnet/master/Binaries.zip)

Stable (maybe) Releases:

https://github.com/Gamergotten/Infinite-runtime-tagviewer/releases

# Read me

figured i'd give this a go, seeing as everyone and their mothers are making Assembly type tools for halo infinite


the point of this tool is to lay-out all of the related *runtime* tag info into a readable format. It also supports writing because thats even cooler

this tool does not rely on module files to read tags because we read everything from the games memory.
however it does partly require the TagID's and their respective Tagnames from the modules, as they do not appear to be in Halo Infinite's runtime memory.

List of tags that are kinda mapped out so far (DM me or something if whatever's not here that you want supported)
- vehi 
- weap 
- proj 
- hlmt (swapping turrets on vehicles and whatever)
- sddt
- levl (for soft&kill barriers)
- effe (tried mapping out AI spawning stuff)
- matg
- pmcg
- glpa (player representation)


enormous thanks to Krevil for his tool (https://github.com/Krevil/InfiniteModuleReader) that allows us to dump the tag names&IDs, so we can see the correct names of each tag and easily redump the info anytime theres a change in the module information

also pretty huge thanks to Callum Carmicheal for the various contributions and the UI (which now doesn't look terrible lol, nice work!), 
as well as the rest of the halo mods crew for being awesome

https://nightly.link/Gamergotten/Infinite-runtime-tagviewer/workflows/dotnet/master/Binaries.zip
