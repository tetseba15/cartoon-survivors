# Project Overview
- **Game Title:** Cartoon Survivors (Bullet Heaven Survival)
- **High-Level Concept:** A 2D survival game where performance is key, featuring hundreds of enemies and cascading combat mechanics.
- **Players:** Single player.
- **Render Pipeline:** UniversalRP (URP).
- **Target Platform:** WebGL.
- **Input System:** New Input System.

# Game Mechanics
## Core Gameplay Loop
The player survives waves of enemies, collecting experience gems and using various status effects to clear large groups. The "Chain Detonation Mark" is a high-reward mechanic designed to clear dense crowds through recursive explosions.

## Chain Detonation Mark Mechanic
1.  **Marking:** A player's attack applies a "Mark" status to an enemy.
2.  **Trigger:** When a marked enemy dies, it spawns a `ChainExplosion` effect.
3.  **Propagation:** The explosion marks all enemies in its radius and then deals damage.
4.  **Cascading:** If that damage kills a newly marked enemy, another explosion is triggered.
5.  **Failsafe:** A generation-based depth limit prevents infinite recursion and frame-rate crashes.

# UI
- **Mark Indicator (Optional/Future):** A visual sprite or particle effect above marked enemies. (Not part of the core logic plan but noted for integration).

# Key Asset & Context
- **Scripts:** `Entity.cs`, `Enemy.cs`, `PoolManager.cs`, `ChainExplosion.cs` (New).
- **Prefabs:** `ChainExplosionVFX` (Empty GameObject with the script and optional visual effects).
- **Layer Mask:** An `Enemy` layer for efficient physics scanning.

# Implementation Steps

## 1. Update Entity.cs (Base Class)
- **Add State:** Add `bool IsMarked` and `int ExplosionGeneration`.
- **Reset Logic:** Ensure `OnEnable` resets `IsMarked = false` and `ExplosionGeneration = 0`. This is critical for pool safety so recycled enemies aren't "pre-marked".
- **Marking Method:** Add `public void ApplyMark(int generation)` to set both the boolean and the generation.

## 2. Update Enemy.cs (Death Logic)
- **Modify Die():** Before returning to the pool, check `if (IsMarked)`.
- **Failsafe Check:** Only trigger the explosion if `ExplosionGeneration < MAX_CHAIN_DEPTH` (e.g., 10).
- **Spawn Explosion:** Call `PoolManager.Instance.SpawnFromPool("ChainExplosion", ...)` and pass the current `ExplosionGeneration + 1` to the spawned explosion object.

## 3. Create ChainExplosion.cs (Separated Logic)
- **Inheritance:** Should likely inherit from a base VFX or use `IPoolable` to return itself.
- **Initialization:** Implement an `Initialize(int generation, float damage, float radius)` method.
- **Overlap Detection:** Use `Physics2D.OverlapCircleNonAlloc` with a dedicated `Enemy` LayerMask to minimize garbage collection and maximize performance.
- **Sequence of Operations:**
    1.  Find all colliders in radius.
    2.  For each collider, get the `Enemy` component.
    3.  If enemy is alive:
        - Call `enemy.ApplyMark(generation)`.
        - Call `enemy.TakeDamage(damage)`.
- **Self-Cleanup:** Return the explosion object to the pool after the logic (and any visual duration) is complete.

## 4. Pool Management Configuration
- Register the new `ChainExplosion` prefab in the `PoolManager` inspector.
- Ensure the tag matches the string used in `Enemy.Die()`.

# Verification & Testing
- **Unit Test - Single Detonation:** Mark one enemy, kill it, and verify the explosion spawns and damages neighbors.
- **Unit Test - Chain Reaction:** Cluster 10 low-health enemies, mark one, and kill it. Verify all 10 explode in sequence.
- **Edge Case - Circular Recursion:** Place two high-health marked enemies next to each other. Ensure they don't infinitely trigger explosions if their health stays above 0 (the rule states they only explode on death, but we must verify the `ExplosionGeneration` increments correctly).
- **Performance Profiling:** Monitor the "Physics.Update" and "Script.Update" costs during a massive chain reaction using the Unity Profiler.
