# Survival Island

## How to Run This Project
1. Download the ZIP and extract it.
2. Navigate to `Survivalisland-main\Survivalisland-main\Assets\Scenes` and open the **SampleScene** file — this will launch the project in Unity (version **6000.0.77f1**).
3. Once the scene finishes loading, press **Play** to start a run — the game drops you straight into gameplay (no main menu).

## Game Concept
Survival Island is a third-person survival game where the player explores a large open island, gathering resources by day and fending off enemies that emerge at night. The goal is simple but tense: survive until the timer runs out. As night falls, skeleton enemies spawn around the player, wander the island, and attack on sight — forcing the player to balance resource gathering against combat readiness.

## Objective
**Survive until the in-game timer reaches zero.** If the player's health drops to 0 before the timer runs out, the game ends in a loss. Reaching the timer goal while still alive triggers a win.

## Controls
| Action | Input |
|---|---|
| Move | WASD |
| Look | Mouse |
| Jump | Space |
| Sprint | (StarterAssets default — Left Shift) |
| Interact / Harvest Resource | E |
| Attack (Axe) | Left Mouse Button |
| Toggle Inventory | I |
| Restart (on Win/Lose screen) | Click "Restart" button |

## Gameplay Systems

**Day/Night Cycle**
A continuously running cycle shifts lighting, ambient color, and skybox tint between day and night, and drives when enemies are allowed to spawn.

**Resource Gathering**
Walking up to an apple tree, ore rock, or dead tree and pressing E harvests it into the player's inventory. Each resource node has limited uses before it depletes.

**Inventory System**
Collected resources (apples, ore, wood from dead trees) are tracked in a slot-based inventory with stacking, viewable/toggleable through a dedicated UI panel.

**Enemy AI (Skeletons)**
Skeleton enemies spawn only at night, in a ring around the player's current position (not fixed locations, since the map is large). Each enemy wanders near its spawn point until the player enters detection range, then chases and attacks on a cooldown. A one-shot "spotted" sound plays the moment an enemy first notices the player.

**Combat**
The player can swing an axe (Left Mouse Button) to damage nearby enemies. A hit only registers — and only plays the attack sound — if the swing actually connects with an enemy's hitbox. Enemies have their own health pool and play a death animation/sound before being removed.

**Player Health**
The player has a health pool that decreases when hit by enemies, displayed live on the HUD. Reaching 0 health ends the game in a loss.

**Survival Timer & Win/Lose Flow**
A countdown timer tracks progress toward the win condition. Reaching 0 remaining time triggers a Win screen; player health reaching 0 triggers a Lose screen. Both screens pause the game and offer a Restart button that reloads the scene cleanly.

**Main Menu**
A dedicated menu scene loads first, offering Play, Controls, and Quit. The Controls screen lists the full control scheme before starting a run, and menu music plays for as long as the menu is active.

**Audio**
Includes day/night ambient music that crossfades based on time of day, background ambience, and dedicated sound effects for player attacks, enemy detection, and enemy death.

**Visual Polish**
Dynamic day/night lighting transitions, low-poly environment props (trees, grass, rocks), a minimap with compass heading display, and scattered resource nodes across the terrain.

## Known Issues
- Skeleton enemies occasionally animate erratically — likely an Animator transition/state timing issue that hasn't been fully isolated yet.
- Enemy movement (both wandering and chasing) does not continuously resample terrain height while moving between points, so on steep slopes an enemy can occasionally appear slightly above or below the actual ground surface. Not currently a major visibility issue during normal play, but worth polishing later.

## External Assets & Resources Used

**Models & Environment**
- [StarterAssets ThirdPerson (URP)](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-urp-196526) (Unity) — player character rig, movement/camera controller, and base input setup.
- [Low Poly Environment - Nature Free (Lowpoly Medieval Fantasy Series)](https://assetstore.unity.com/packages/3d/environments/low-poly-environment-nature-free-lowpoly-medieval-fantasy-series-187052) (Polytope Studio) — low-poly terrain props (grass, rocks, foliage).
- [Mid Poly Axes Collection](https://assetstore.unity.com/packages/3d/props/weapons/mid-poly-axes-collection-326804) (Skyden Games) — axe model used for the player's melee weapon.
- [Stylized Low Poly Skeleton](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/stylized-low-poly-skeleton-306857) (SazenGames) — skeleton enemy model, rig, and animation clips.
- [2D Strategy Game Icons](https://assetstore.unity.com/packages/2d/gui/icons/2d-strategy-game-icons-281449) — UI icon set used for resource/inventory display.
- **TextMesh Pro** (Unity) — all in-game UI text rendering.

**Audio**
- [Nature Night Forest with Frogs and Crickets](https://pixabay.com/sound-effects/nature-night-forest-with-frogs-and-crickets-for-sleep-451153/) (Pixabay) — nighttime ambience.
- [Nature Forest Daytime](https://pixabay.com/sound-effects/nature-forest-daytime-446356/) (Pixabay) — daytime ambience.
- [Meditation/Spiritual Atmospheric Documentary](https://pixabay.com/music/meditationspiritual-atmospheric-documentary-509386/) (Pixabay) — background music track.
- [Musical Menu](https://pixabay.com/sound-effects/musical-menu-53679/) (Pixabay) — main menu background music.
- [Weapon Axe Hit 01](https://pixabay.com/sound-effects/film-special-effects-weapon-axe-hit-01-153372/) (Pixabay) — player attack sound effect.
- [Zombie Sound](https://pixabay.com/sound-effects/film-special-effects-zombie-sound-389279/) (Pixabay) — enemy spot/detection sound effect.
- [Death 2](https://pixabay.com/sound-effects/film-special-effects-death2-340040/) (Pixabay) — enemy death sound effect.

---
*Checkpoint 2 — main gameplay loop complete: movement, resource gathering, inventory, day/night-driven enemy combat, survival timer, and win/lose/restart flow are all implemented and tested.*
