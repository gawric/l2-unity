# L2-Unity

<p>This project aim is to create a basic playable demo of Lineage2 on Unity.</p>

This [video](https://www.youtube.com/watch?v=IEHY37bJ7nk) inspired me to start on this project.

<p>Preview of the current state of the project:</p>

<img src="https://i.imgur.com/HSXQLDF.png" alt="Preview0" style="max-width: 75%; height: auto;">
<img src="https://i.imgur.com/Dwrg15Y.png" alt="Preview" style="max-width: 75%; height: auto;">
<img src="https://i.imgur.com/OnWL7RX.png" alt="Preview3" style="max-width: 75%; height: auto;">
<img src="https://i.imgur.com/OqnzT1H.png" alt="Preview2" style="max-width: 75%; height: auto;">
<img src="https://i.imgur.com/hemt26R.png" alt="Preview4" style="max-width: 75%; height: auto;">

## What are the expected features?

For now the aim is to create a basic demo, therefore only basic features will be available:
- Client-side Pathfinding ✅
- Click to move and WASD movements ✅
- Camera collision ✅
- Basic UI
    - Status ✅
    - Chat ✅
    - Target ✅
    - Nameplates ✅
    - Skillbar ✅
	- Action ✅
	- Character info ✅
	- System menu ✅
	- Exit window ✅
- Basic combat ✅
- Basic RPG features 
    - HP Loss and regen ✅
    - Exp gain on kills
    - Leveling
- Small range of models
    - 2 races for players ✅ (FDarkElf, FDwarf)
	- A few armor sets for each race ✅ (naked set, starter set)
	- A few of weapons each type ✅
    - All Monsters of Talking island ✅
    - All NPCs of Talking island ✅
- Server/Client features (servers project [Gameserver](https://github.com/shnok/unity-mmo-gameserver) [Loginserver](https://github.com/shnok/unity-mmo-loginserver))
	- Login/Logout ✅
	- Server select ✅
	- Character select 🛠
    - Player position/rotation sync ✅
    - Animation sync ✅
    - Chat ✅
    - Server Ghosting/Grid system ✅
    - NPCs ✅
    - Monsters ✅
    - Monsters AI with Pathfinding ✅
    - Player actions ✅
    - Inventory system ✅
    - Skillbar system ✅
- Import Lineage2's world
    - Talking island region ✅
        - StaticMeshes ✅
        - Brushes ✅
        - Terrain ✅
        - DecoLayer ✅
- Day/Night cycle ✅
- Game sounds (FMOD project [here](https://github.com/shnok/l2-unity/tree/main/l2-unity-fmod))
    - Ambient sounds ✅
    - Step sounds (based on surface) ✅
	- Music ✅
    - UI sounds ✅
    - NPC sounds ✅

## How to run?

<ol> 
<li>Open the "Menu" scene and drag&drop the l2_lobby scenes into your scene<br><br>
<img src="https://i.imgur.com/aEBM3eJ.png" alt="Preview4" style="max-width: 50%; height: auto;"><br><br></li>
<li>Add all the scenes in the build settings (0. Menu 1. L2_lobby 2. Game then the remaining scenes)<br><br>
<img src="https://i.imgur.com/qMyP1vi.png" alt="Preview4" style="max-width: 50%; height: auto;"><br><br></li>
<li> Download and run the <a href="https://github.com/shnok/unity-mmo-loginserver">loginserver</a> project</li>
<li> Download and run the <a href="https://github.com/shnok/unity-mmo-gameserver">gameserver</a> project</li>
</ol>

## Contributing

Feel free to fork the repository and open any pull request.

## Discord

Project [discord](https://discord.gg/ra3BmraPKp).