# L2-Unity

<p>This project aim is to create a basic playable demo of Lineage2 on Unity.</p>

This [video](https://www.youtube.com/watch?v=IEHY37bJ7nk) inspired me to start on this project.

<p>Preview of the current state of the project:</p>

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
    - Status window ✅
    - Chat window ✅
    - Target window ✅
    - Nameplates ✅
    - Skillbar
- Basic combat ✅
- Basic RPG features 
    - HP Loss and regen 🛠️ (Players can fight mobs but wont regen)
    - Exp gain on kills
    - Leveling
- Small range of models
    - 2 races for players ✅ (FDarkElf, FDwarf)
	- A few armor sets for each race ✅ (naked set, starter set)
	- A few of weapons each type ✅
    - All Monsters of Talking island ✅
    - All NPCs of Talking island ✅
- Server/Client features (server project [here](https://gitlab.com/shnok/unity-mmo-server))
    - Player position/rotation sync ✅
    - Animation sync ✅
    - Chat ✅
    - Server Ghosting/Grid system ✅
    - NPCs ✅
    - Monsters ✅
    - Monsters AI with Pathfinding ✅
- Import Lineage2's world
    - Talking island region ✅
        - StaticMeshes ✅
        - Brushes ✅
        - Terrain ✅
        - DecoLayer ✅
- Day/Night cycle ✅
- Game sounds (FMOD project [here](https://gitlab.com/shnok/l2-unity/-/tree/main/l2-unity-fmod/))
    - Ambient sounds ✅
    - Step sounds (based on surface) ✅
	- Music ✅
    - UI sounds ✅
    - NPC sounds ✅

## How to run?

<p>Open the "Game" scene and drag&drop the 1x_1x scenes into your scene.</p>

<img src="https://i.imgur.com/FDleY0u.png" alt="Preview4" style="max-width: 75%; height: auto;">

Download and run the [server] project (https://gitlab.com/shnok/unity-mmo-server).

## Contributing

Feel free to fork the repository and open any pull request.
