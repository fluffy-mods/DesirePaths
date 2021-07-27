Creates packed dirt paths when colonists frequently take the same path.

![Features](https://banners.karel-kroeze.nl/title/Features.png)  
Packed dirt has a higher move speed than most other 'natural' terrains, but it does track a lot of dirt around and has no fertility. If your colonists frequently walk through your farms, they will eventually trample the fields.

Packed dirt paths will form when colonists frequently take the same path over grass, soil, dirt and other natural terrain. The packed dirt paths will eventually degrade back into the original terrain if not walked on. In the real world, such paths are called [desire paths](https://en.wikipedia.org/wiki/Desire_path), Elephant paths, pirate paths, or just simply shortcuts.

![Desire Path](https://i.ibb.co/gF6Sw3t/image.png)

![Recommended](https://banners.karel-kroeze.nl/title/Recommended.png)  
If you find that your colonists keep walking through your fields, I recommend using this together with
[Path Avoid](https://steamcommunity.com/sharedfiles/filedetails/?id=1180719857), so that you can better steer where your colonists may and may not walk.

For problematic trade caravans, you may want to use [KV's Trading Spot](https://steamcommunity.com/sharedfiles/filedetails/?id=1180719658).

That said, if such desire paths form where you don't want them, it's really a sign that you need to change your base design!

![Known Issues](https://banners.karel-kroeze.nl/title/Known%20Issues.png)  
None

![For Modders](https://banners.karel-kroeze.nl/title/For%20Modders.png)  
By default, any terrain that has the XML tag TakeFootprints' set to true will be 'packable', and given enough traffic, packed dirt paths will appear. As of version 0.7.97, the mod reads a DefModExtension on terrain defs. This extension has two fields, 'disabled' and 'packedTerrain'. These allow you to disable desire paths' behaviour for a specific terrain, or specify which terrain def should be used for the created path.

This extension can either be added directly on modded terrainDefs, or be injected with a patch. Desire Paths itself uses such a patch to disable its' behaviour for Ice terrain, so that might be a good place to start.

![Contributors](https://banners.karel-kroeze.nl/title/Contributors.png)  
Juijote: [Add Simplified Chinese translation](https://github.com/fluffy-mods/DesirePaths/commit/2db8525)
Proxyer: [Japanese Translation](https://github.com/fluffy-mods/DesirePaths/commit/9b5dcdd)


![Think you found a bug?](https://banners.karel-kroeze.nl/title/Think%20you%20found%20a%20bug%3F.png)  

Please read [this guide](http://steamcommunity.com/sharedfiles/filedetails/?id=725234314) before creating a bug report,
and then create a bug report [here](https://github.com/fluffy-mods/DesirePaths/issues)

![Older versions](https://banners.karel-kroeze.nl/title/Older%20versions.png)  

All current and past versions of this mod can be downloaded from [GitHub](https://github.com/fluffy-mods/DesirePaths/releases).

![License](https://banners.karel-kroeze.nl/title/License.png)  

All original code in this mod is licensed under the [MIT license](https://opensource.org/licenses/MIT). Do what you want, but give me credit.
All original content (e.g. text, imagery, sounds) in this mod is licensed under the [CC-BY-SA 4.0 license](http://creativecommons.org/licenses/by-sa/4.0/).

Parts of the code in this mod, and some of the content may be licensed by their original authors. If this is the case, the original author & license will either be given in the source code, or be in a LICENSE file next to the content. Please do not decompile my mods, but use the original source code available on [GitHub](https://github.com/fluffy-mods/DesirePaths/), so license information in the source code is preserved.



![Are you enjoying my mods?](https://banners.karel-kroeze.nl/title/Are%20you%20enjoying%20my%20mods%3F.png)  

Become a supporter and show your appreciation by buying me a coffee (or contribute towards a nice single malt).

[![Supporters](https://banners.karel-kroeze.nl/donations.png)](https://ko-fi.com/fluffymods)

[![Buy Me a Coffee](https://i.imgur.com/6P7Ap79.gif)](https://ko-fi.com/fluffymods)

[![I Have a Black Dog](https://i.ibb.co/ss59Rwy/New-Project-2.png)](https://www.youtube.com/watch?v=XiCrniLQGYc)
