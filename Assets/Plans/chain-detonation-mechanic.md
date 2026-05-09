# Project Overview
- **Game Title:** Cartoon Survivors
- **High-Level Concept:** A 2D Bullet Heaven survival game where players fight massive waves of enemies. Performance and satisfying combat feedback (cascades) are priorities.
- **Players:** Single player vs AI swarms.
- **Inspiration:** Vampire Survivors, Brotato.
- **Target Platform:** WebGL (Performance is extra critical).
- **Render Pipeline:** UniversalRP.
- **Input System:** New Input System.

# Game Mechanics
## Core Gameplay Loop
The player survives waves of enemies, collecting experience gems to level up and gain new abilities. The "Chain Detonation Mark" is a strategic combat mechanic that turns high-density enemy clusters into explosive chain reactions.

## Chain Detonation Mark Mechanic
1. **Application:** A "Mark" is applied to an enemy via player weapons or abilities.
2. **Trigger:** When a marked enemy's health reaches 0, it triggers an explosion request.
3. **Explosion Logic:**
    - Scans a circular area for enemies.
    - For each hit enemy:
        - If the explosion's `generationDepth` > 0, apply the "Mark" status and set its depth to `currentDepth - 1`.
        - Deal damage.
4. **Staggered Execution:** To prevent frame spikes and infinite recursion, explosions are queued and processed at a limited rate per frame.

# UI
- **Placeholder VFX:** A simple circular sprite/particle system for the explosion.
- **Mark Indicator (Optional):** A small visual cue (recolor or icon) on marked enemies to provide player feedback.

# Key Asset & Context
- **Scripts:**
    - `Enemy.cs`: Existing script to be updated with `Mark` state.
    - `ChainExplosion.cs`: New script for the explosion logic.
    - `ExplosionManager.cs`: New singleton to manage the explosion queue and throttling.
- **Prefabs:**
    - `ChainExplosionPrefab`: An empty GameObject with a `ChainExplosion` script and visual placeholders.

# Implementation Steps

## 1. Update Enemy State & Lifecycle
**File:** `Assets/Scripts/Enemy/Enemy.cs`
- Add `public bool IsMarked { get; private set; }`.
- Add `private int currentMarkDepth;`.
- Implement `public void ApplyMark(int depth)` to set the state.
- Update `OnEnable` (or `Setup`) to reset `IsMarked = false` and `currentMarkDepth = 0`.
- Modify `Die()`:
    - If `IsMarked` is true, call `ExplosionManager.Instance.QueueExplosion(transform.position, currentMarkDepth)`.

## 2. Implement the Explosion Manager
**File:** `Assets/Scripts/Managers/ExplosionManager.cs` (New)
- Create a Singleton to handle global explosion logic.
- Define `struct ExplosionRequest { Vector3 position; int depth; }`.
- Maintain a `Queue<ExplosionRequest>`.
- **Throttling Logic:**
    - In `Update()`, process a `maxExplosionsPerFrame` (e.g., 3-5).
    - For each request, pull a `ChainExplosion` from `PoolManager`.
    - Pass the `depth` and `position` to the explosion script.

## 3. Implement the Chain Explosion Logic
**File:** `Assets/Scripts/ChainExplosion.cs` (New)
- Attached to the explosion prefab.
- **Logic Flow in `Setup(int depth)`:**
    - Define `radius` and `damage` (could be passed from manager or tuned in inspector).
    - Use `Physics2D.OverlapCircleAll` to find nearby enemies.
    - Iterate through results:
        - Ensure hit object is an `Enemy` and alive.
        - **Step A (Mark):** If `depth > 0`, call `enemy.ApplyMark(depth - 1)`.
        - **Step B (Damage):** Call `enemy.TakeDamage(damage)`.
    - Play visual/audio effects.
    - Return itself to `PoolManager` after the effect duration.

## 4. Pool Integration
**File:** `Assets/Scripts/Managers/PoolManager.cs`
- Add a new `Pool` entry for "ChainExplosion".
- Ensure the `size` is sufficient (e.g., 20-30) to handle the per-frame processing.

# Verification & Testing
1. **Unit Test - Mark Reset:** Spawn an enemy, mark it, kill it, and verify that when it is spawned again from the pool, `IsMarked` is false.
2. **Cascade Test:** Place 3 enemies close together. Mark the first one and kill it with a high-damage single hit. Verify that all 3 explode in a staggered sequence (if damage is enough to kill them).
3. **Infinite Loop Failsafe:** Place two enemies on top of each other and set `generationDepth` to a high value. Verify the chain stops exactly when depth reaches 0 or if the max-per-frame limit is reached.
4. **Performance Check:** Use the Profiler to ensure `OverlapCircleAll` and `TakeDamage` calls do not cause spikes during heavy chain reactions.
