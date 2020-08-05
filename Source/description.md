Creates packed dirt paths when colonists frequently take the same path.

# Features

Packed dirt has a higher move speed than most other 'natural' terrains, but it does track a lot of dirt around and has no fertility. If your colonists frequently walk through your farms, they will eventually trample the fields.

Packed dirt paths will form when colonists frequently take the same path over grass, soil, dirt and other natural terrain. The packed dirt paths will eventually degrade back into the original terrain if not walked on. In the real world, such paths are called [desire paths](https://en.wikipedia.org/wiki/Desire_path), Elephant paths, pirate paths, or just simply shortcuts.

![Desire Path](https://i.ibb.co/gF6Sw3t/image.png)

# Recommended

If you find that your colonists keep walking through your fields, I recommend using this together with
[Path Avoid](https://steamcommunity.com/sharedfiles/filedetails/?id=1180719857), so that you can better steer where your colonists may and may not walk.

That said, if such desire paths form where you don't want them, it's really a sign that you need to change your base design!

# Known Issues

None

# For Modders

By default, any terrain that has the XML tag TakeFootprints' set to true will be 'packable', and given enough traffic, packed dirt paths will appear. As of version 0.7.97, the mod reads a DefModExtension on terrain defs. This extension has two fields, 'disabled' and 'packedTerrain'. These allow you to disable desire paths' behaviour for a specific terrain, or specify which terrain def should be used for the created path.

This extension can either be added directly on modded terrainDefs, or be injected with a patch. Desire Paths itself uses such a patch to disable its' behaviour for Ice terrain, so that might be a good place to start.
