Creates packed dirt paths when colonists frequently take the same path.

[img]https://banners.karel-kroeze.nl/title/Features.png[/img]Packed dirt has a higher move speed than most other 'natural' terrains, but it does track a lot of dirt around and has no fertility. If your colonists frequently walk through your farms, they will eventually trample the fields.

Packed dirt paths will form when colonists frequently take the same path over grass, soil, dirt and other natural terrain. The packed dirt paths will eventually degrade back into the original terrain if not walked on. In the real world, such paths are called [url=https://en.wikipedia.org/wiki/Desire_path]desire paths[/url], Elephant paths, pirate paths, or just simply shortcuts.

[img]https://i.ibb.co/gF6Sw3t/image.png[/img]

[img]https://banners.karel-kroeze.nl/title/Recommended.png[/img]If you find that your colonists keep walking through your fields, I recommend using this together with
[url=https://steamcommunity.com/sharedfiles/filedetails/?id=1180719857]Path Avoid[/url], so that you can better steer where your colonists may and may not walk.

For problematic trade caravans, you may want to use [url=https://steamcommunity.com/sharedfiles/filedetails/?id=1180719658]KV's Trading Spot[/url].

That said, if such desire paths form where you don't want them, it's really a sign that you need to change your base design!

[img]https://banners.karel-kroeze.nl/title/Known%20Issues.png[/img]None

[img]https://banners.karel-kroeze.nl/title/For%20Modders.png[/img]By default, any terrain that has the XML tag TakeFootprints' set to true will be 'packable', and given enough traffic, packed dirt paths will appear. As of version 0.7.97, the mod reads a DefModExtension on terrain defs. This extension has two fields, 'disabled' and 'packedTerrain'. These allow you to disable desire paths' behaviour for a specific terrain, or specify which terrain def should be used for the created path.

This extension can either be added directly on modded terrainDefs, or be injected with a patch. Desire Paths itself uses such a patch to disable its' behaviour for Ice terrain, so that might be a good place to start.

[img]https://banners.karel-kroeze.nl/title/Contributors.png[/img]

[list][*]Juijote: Add Simplified Chinese translation[*]Proxyer: Japanese Translation[/list]

[img]https://banners.karel-kroeze.nl/title/Think%20you%20found%20a%20bug%3F.png[/img]

Please read [url=http://steamcommunity.com/sharedfiles/filedetails/?id=725234314]this guide[/url] before creating a bug report,
and then create a bug report [url=https://github.com/fluffy-mods/DesirePaths/issues]here[/url]

[img]https://banners.karel-kroeze.nl/title/Older%20versions.png[/img]

All current and past versions of this mod can be downloaded from [url=https://github.com/fluffy-mods/DesirePaths/releases]GitHub[/url].

[img]https://banners.karel-kroeze.nl/title/License.png[/img]

All original code in this mod is licensed under the [url=https://opensource.org/licenses/MIT]MIT license[/url]. Do what you want, but give me credit.
All original content (e.g. text, imagery, sounds) in this mod is licensed under the [url=http://creativecommons.org/licenses/by-sa/4.0/]CC-BY-SA 4.0 license[/url].

Parts of the code in this mod, and some of the content may be licensed by their original authors. If this is the case, the original author & license will either be given in the source code, or be in a LICENSE file next to the content. Please do not decompile my mods, but use the original source code available on [url=https://github.com/fluffy-mods/DesirePaths/]GitHub[/url], so license information in the source code is preserved.

[img]https://banners.karel-kroeze.nl/title/Are%20you%20enjoying%20my%20mods%3F.png[/img]

Become a supporter and show your appreciation by buying me a coffee (or contribute towards a nice single malt).

[url=https://ko-fi.com/fluffymods][img]https://banners.karel-kroeze.nl/donations.png[/img][/url]

[url=https://ko-fi.com/fluffymods][img]https://i.imgur.com/6P7Ap79.gif[/img][/url]

[url=https://www.youtube.com/watch?v=XiCrniLQGYc][img]https://i.ibb.co/ss59Rwy/New-Project-2.png[/img][/url]