# About-the-years-gameoff-2026
A 2D pixel-art narrative puzzle game for Game Off 2026. Theme:SHIFT.
可以从这里拉取代码和资源，方便项目合并和协作
## Development Environment

- **Game Engine:** Unity 2022.3.62f3c1 LTS
- **Project Type:** 2D (Built-In Render Pipeline)
- **Version Control:** Git + GitHub Desktop
- **Large File Storage:** Git LFS

## Project Structure

```text
About-the-years-gameoff-2026/
├── UnityProject/     # Unity 游戏工程
├── ArtSource/        # 美术源文件
├── Docs/             # 策划、美术与技术文档
├── README.md         # 项目说明
└── .gitignore        # Git 忽略规则
```

### Unity Assets

Unity 游戏资源统一存放在：

`UnityProject/Assets/_Game/`

主要目录：

- `Animations/`：角色与环境动画
- `Art/`：游戏内使用的 PNG 图片及美术资源
- `Audio/`：背景音乐、环境音效
- `Fonts/`：字体资源
- `Prefabs/`：预制体
- `Scenes/`：游戏场景
- `Scripts/`：C# 脚本
- `Settings/`：游戏配置
- `UI/`：用户界面资源

## How to Open the Project

1. 克隆本 GitHub 仓库到本地。
2. 打开 Unity Hub。
3. 点击 **Add project from disk**。
4. 选择仓库中的 `UnityProject` 文件夹。
5. 使用 Unity `2022.3.62f3c1` 打开项目。

**注意：不要直接选择仓库根目录作为 Unity 工程。**

## Collaboration Guidelines

### Git Workflow

1. 开始工作前，先从远程仓库同步最新内容。
2. `main` 分支用于保存稳定的游戏版本。
3. 开发新功能时，创建 `feature/功能名称` 分支。
4. 提交美术资源时，可以创建 `art/资源名称` 分支。
5. 完成修改后，通过 Pull Request 请求合并。
6. 合并前检查 Unity 工程是否能够正常打开和运行。

### Unity Collaboration Rules

- 不要提交 `Library/`、`Temp/`、`Obj/` 等 Unity 缓存目录。
- 不要删除或忽略 Unity 自动生成的 `.meta` 文件。
- 不要在 Unity 外随意移动已经导入的资源。
- 尽量避免多人同时修改同一个 Scene 或 Prefab。
- 修改公共场景前，先与负责整合的程序员沟通。
- 提交修改时，使用清晰的 Commit message。


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
- `PROP_Door_2014_Closed.png`### Asset Naming Conventions

| Asset Type | Example |
|---|---|
| C# Script | `PlayerController.cs` |
| Scene | `MainMenu.unity` |
| Prefab | `PF_Player.prefab` |
| Background | `BG_Room_2014.png` |
| Character Sprite | `CHAR_Player_Idle.png` |
| Animation | `AN_Player_Walk.anim` |
| UI Sprite | `UI_Calendar_Icon.png` |
| Sound Effect | `SFX_DoorOpen.wav` |
| Background Music | `BGM_MainTheme.wav` |

### Art Asset Workflow

- 美术源文件（PSD、Aseprite）放在仓库根目录的 `ArtSource/`。
- 导出的 PNG 游戏资源放在 `UnityProject/Assets/_Game/Art/` 对应目录。
- 使用 Git LFS 管理大型美术源文件及已配置的音频格式。
- 导出像素素材时保持原始像素尺寸，不使用模糊缩放。
- 导入 Unity 后，确保资源对应的 `.meta` 文件一并提交。
[Project Naming Conventions](Docs/Art/NamingConventions.md)
