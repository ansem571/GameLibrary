# CharacterLibrary
Notes are based on Version *0.1.0.1*
Date: 12/6/2017

## Introductions
This will be a basis for future Project Developments.
Any critic will be appreciated.
I developed this library in the hopes that I will be able to expand upon these concepts at another date. Although this library contains a Console App, the main purose of this library is to be an external library for future projects. The main puroses of this solution is the development of the Interface project as well as the library implementation. The CharacterLibrary implementation is based on a console/2D app. Although, this can be updated to include 3D (z-axis) points, I will need to add a source canvas to act as the UI, but more on that in a future update.

Now to get into the current code developed.

## Player, Enemies, Stats

#### Currently there are only 2 types of damage and defense.
* Physical
* Magic

#### Damage types will be reworked at a later date to the following
* Melee
* Ranged
* Magic

#### With any of the following Secondary types
* Fire
* Water
* Earth
* Wind
* Chaos
* Holy
* Physical

These scale at the same rate, but later in the game, although physical has more visible damage, enemies have less resistence to magic attacks. Enemies scale by your level to keep up with the game and to continue making it a challenge. Early game scaling is low to accommodate for later game fast scaling. This is to counteract the ease of gaining experience early in the game. Similar to Runescape, you gain experience for damaging enemies as well as a bonus for defeating them. For clearing a dungeon of finding Points of Interest (POI) on the map, you will gain additional experience. More on map information later. 
For ease of use, beginning development will only contain the origianl types of damage. Changes to damage types will be made at a future date/version before release.

## Combat Actions
Combat actions are Attack (with or without magic), Charge Mana, and Retreat. These will be updated to accommodate the updated damage types when those damage types are implemented. Damage Mitigation formulas will have to be reworked when damage types are updated.

## Map and Tiles
The map works on a 2D Grid system. Each location is given a tile. The current reader for tiles adds the base terrain tiles out first, then applies special tile types to replace the terrain type.
#### Terrain tiles
* Desert
* Forest
* Mountain
* Plains
* Seaside

#### Special tiles
* Dungeon
* Enemy
* Point of Interest (POI)
* Seaside

Once you have entered a tile, the grid will register that you have at least "visited" that location. Now when you open the map (will be opened/refreshed upon movement in console app), you will see which tiles have been visited as that tile is no longer dark/blacked out. In console app, image will be opened in Windows Photo Viewer. As this is a bit map, you will need to zoom in to see map. I will update the map to be more visible at a later date. If the tile entered is a special tile, an event will occur before you return to the main game menu. If you do not complete the event at that location, the tile will still be registered as "visited" to allow the user to be aware where that location is, in order to return at a later date.

As stated previously, thanks to the implementation of our interfaces, we have to change almost no code in neither the map nor the tiles themselves in order to integrate this implementation into Unity.
There might have to be changes to add a 3rd dimension (z-axis) for the map, but limited changes will need to be made to the entities containing those points themselves. 

> Thank you for reading, will attempt to update appropriately. > - Ansem571 12/6/17