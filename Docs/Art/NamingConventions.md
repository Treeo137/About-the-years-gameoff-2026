# Project Naming Conventions

**Project:** Game Off 2026  
**Version:** 1.0  
**Engine:** Unity 2022.3

## 1. General Rules

- Use English letters, numbers, and underscores for asset filenames.
- Use consistent prefixes for Unity assets.
- Use PascalCase for C# scripts and folders.
- Include the year in filenames when the asset has year-specific variants.
- Do not use suffixes such as `final`, `new`, `v2`, or `latest` for version management.
- Use Git history to track file revisions.
- Rename or move imported Unity assets within the Unity Editor whenever possible.
- Commit Unity `.meta` files together with their corresponding assets.

## 2. Asset Naming Reference

| Asset Type | Prefix | Example |
|---|---|---|
| Scene | SC_ | SC_Bedroom.unity |
| Prefab | PF_ | PF_Player.prefab |
| Background | BG_ | BG_Bedroom_2014.png |
| Character | CHR_ | CHR_Player_Idle.png |
| Prop | PROP_ | PROP_Door_2014_Open.png |
| Tile Asset | TILE_ | TILE_Bedroom.png |
| Animation Clip | AN_ | AN_Player_Walk.anim |
| Animator Controller | AC_ | AC_Player.controller |
| UI Asset | UI_ | UI_Clue_Icon.png |
| Sound Effect | SFX_ | SFX_DoorOpen.wav |
| Background Music | BGM_ | BGM_MainTheme.wav |
| Ambient Audio | AMB_ | AMB_Rain.wav |
| ScriptableObject Asset | SO_ | SO_Clue_Diary.asset |
| Material | MAT_ | MAT_SpriteOutline.mat |

## 3. Year-Specific Assets

Use the following naming pattern:

`PREFIX_Object_Year_State.extension`

Examples:

- `BG_Bedroom_2014.png`
- `BG_Bedroom_2017.png`
- `PROP_Door_2014_Closed.png`
- `PROP_Door_2014_Open.png`
- `PROP_Door_2017_Broken.png`

Omit the year or state when it is not relevant.

## 4. C# Scripts

C# scripts use PascalCase without asset prefixes.

Examples:

- `PlayerController.cs`
- `InteractionDetector.cs`
- `TimeShiftManager.cs`
- `ClueManager.cs`
- `PuzzleManager.cs`

The script filename should match its primary class name.

## 5. Logical IDs

Use lowercase snake_case for persistent game object, clue, and puzzle identifiers.

Examples:

- `clue_diary`
- `clue_photo`
- `puzzle_door_lock`
- `obj_bedroom_door`

IDs must be unique within their relevant data system and should remain stable after creation.

## 6. Art Source Files

Store editable art source files in the repository's `ArtSource/` directory.

Exported game-ready PNG files should be placed in the appropriate Unity project asset directory.

Use the same descriptive base name for source files and their exported assets when practical.

## 7. Version Control

- Use Git history instead of filename version suffixes.
- Avoid duplicate files such as `Player_final.png` or `Player_v2.png`.
- Use meaningful Git commit messages.
- Do not rename or delete shared assets without checking their references.
- Keep `.meta` files under version control.

---

These conventions apply to all team members contributing to the project.
