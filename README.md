# ReactiveUI for Unity

ReactiveUI is a lightweight, modular UI framework for Unity, built around reactive programming and a simple lifecycle abstraction.  
It allows you to cleanly separate UI from game logic, while still enabling fully dynamic behavior.

## Features

1. Subscribe-and-react approach instead of imperative control
2. One base class — `UIView` — for all visual components
3. Self-managed lifecycle for reliable binding/unbinding
4. Built-in animation pipeline supporting Animator, DoTween, and custom solutions
5. Built-in UI elements like reactive buttons, selectors, menu bars, etc.
6. Scales well from quick prototyping to complex applications

## Installing

Just add this line to your Unity project's `manifest.json`:

```json
"com.whitearrow.reactiveui": "https://github.com/white-arrow-dev/reactive-ui-unity.git"
```

## Usage

### UIView Basics

- `UIView` is not abstract and not generic — you can add it directly to any `GameObject` without inheritance.
- If the view doesn’t bind to game logic and just needs show/hide behavior (with optional animation), this is the quickest way to get started.
- Optionally assign `_btnHide` in the inspector to allow the view to close itself via a button.
- If your `UIView` needs initialization (e.g. resolving components, caching references), override `InitCore()`.  
  It is guaranteed to be called before any logic is executed — even if the view starts inactive.
- If you need to ensure initialization in an edge case, you can call `InitIfFalse()` manually.

Example:
```csharp
_myView.Show();
_myView.Hide();

_myView.ShowInHierarchyState
    .Where(state => state == UIViewShowState.AnimationEnded)
    .Subscribe(_ =>
    {
        // Fully visible
    });
```

See the full list of `UIView` properties in the section below.

---

### Binding to State

When your UI reflects some external state, inherit from `UIView` and define your binding logic.

- Use a method called `Bind(...)` to accept and cache the binding context.
- This does **not** bind immediately — it's only a caching step.
- Always call `RebindIfShowedInHierarchy()` after assigning new data. It will bind now if visible, or wait until shown.
- Do actual subscription logic in `BindFromCache()`.
- Cleanup subscriptions or listeners in `DisposeBinding()`. This method is called on hide, rebind, or destroy.

Example:
```csharp
public class MyView : UIView
{
    private IDisposable _hpSubscription;
    private PlayerStats _playerStats;

    protected override void InitCore()
    {
        // One-time setup logic
    }

    public void Bind(PlayerStats stats)
    {
        _playerStats = stats;
        RebindIfShowedInHierarchy();
    }

    protected override void BindFromCache()
    {
        if (_playerStats == null)
            return;

        _hpSubscription = _playerStats.HP
            .Subscribe(UpdateHPBar);
    }

    protected override void DisposeBinding()
    {
        _hpSubscription?.Dispose();
        _hpSubscription = null;
    }

    protected override void OnShowed()
    {
        // Runs after Show() completes (including animation)
    }

    protected override void OnHided()
    {
        // Runs after Hide() completes
    }
}
```

---

### UIView Properties

These properties help you reactively track the lifecycle:

**Show:**
- `IsSelfShowed` — was `Show()` called
- `IsInHierarchyShowed` — is view active in hierarchy
- `ShowInHierarchyState` — `None`, `Requested`, `Showed`, `AnimationEnded`

**Hide:**
- `HideInHierarchyState` — `None`, `Requested`, `Hided`

These are `ReactiveProperty<T>`, so you can `.Subscribe(...)` or `.Value`.

---

## Animations

You can attach animations to a view by either:
- Assigning it in code with `view.SetAnimations(...)`
- Attaching a `MonoBehaviour` component that implements `IViewAnimations` to the same GameObject

The system will:
- Automatically call `PlayShow()` / `PlayHide()` when appropriate
- Wait for animations to finish before updating state
- Deactivate the GameObject only after the hide animation completes

---

### Built-in Base Classes

ReactiveUI includes two helper bases:

| Class                | Use Case |
|---------------------|----------|
| `ViewAnimations`     | Pure C# without MonoBehaviour |
| `MonoViewAnimations` | Unity components with inspector support |

Both provide:
- `Init()` lifecycle hook
- `ShowEnded`, `HideEnded` observables
- `PlayShowCore()`, `PlayHideCore()` for your logic
- `Dispose()` for cleanup

**Example:**
```csharp
public class ScalePop : MonoViewAnimations
{
    [SerializeField] private Transform _target;

    protected override void InitCore(UIView view)
    {
        _target.localScale = Vector3.zero;
    }

    protected override void PlayShowCore()
    {
        _target.DOScale(1f, 0.2f)
            .OnComplete(() => _showEnded.OnNext(Unit.Default));
    }

    protected override void PlayHideCore()
    {
        _target.DOScale(0f, 0.2f)
            .OnComplete(() => _hideEnded.OnNext(Unit.Default));
    }
}
```

All `MonoViewAnimations` include inspector buttons for testing `Show()` and `Hide()` animations.

---

### Built-in Implementations

- `AnimatorViewAnimations` — integrates with Unity `Animator`
    - Uses `ObservableViewAnimatorTrigger`
    - You must place animation events in clips to call:
        - `OnAnimationShowEnded()`
        - `OnAnimationCloseEnded()`

- DoTween support via separate package:  
👉 [reactive-ui-dotween (GitHub)](https://github.com/CliffCalist/reactive-ui-dotween)

---

## Reactive Patterns

ReactiveUI is built on top of [R3](https://github.com/neuecc/R3) — a modern, fast reactive library for Unity.  
We don’t use R3 for multithreading — but rather for its safe, declarative, event-driven API.

It gives you powerful tools to react to data and manage subscriptions with minimal boilerplate.

### Examples

**Subscribe once:**
```csharp
_btn.OnClickAsObservable()
    .Take(1)
    .Subscribe(_ => StartTutorial());
```

**Combine multiple streams:**
```csharp
Observable.CombineLatest(_hp, _mana, (hp, mana) => hp + mana)
    .Subscribe(UpdateResourceBar);
```

**Manual dispose via lifecycle:**
```csharp
_inventory.Updated
    .Subscribe(UpdateView)
    .AddTo(_disposables);
```

**From business logic event to observable:**
```csharp
Observable
    .FromEvent(
        h => Test += h,
        h => Test -= h
    )
    .Subscribe(_ => UpdateHealthBar());
```

---

## Built-in Tools

1. `ViewVisibilityTracker` — track show/hide status of multiple views

## Built-in UI Elements

1. `ViewButton` — reactive button with event streams
2. `ShowViewButton` — triggers `Show()` on assigned view
3. `HideViewButton` — triggers `Hide()` on assigned view
4. `Selector` / `SelectorOption` — toggle group with single active option
5. `TabMenu` — selector for switching between views
6. `ConfirmationPopUp` — confirmation dialog pop-up

---

## Roadmap

- [ ] Automatic subscription disposal inside `UIView`
- [ ] Optional blocker (click-through prevention)
- [ ] Optional canvas splitting support
- [ ] Tools:
  - [ ] Optimized element spawner: reuse, create, destroy, and rebind efficiently
- [ ] UI Components:
  - [ ] View-switcher button
  - [ ] Reactive layout groups
  - [ ] Reactive toggles
  - [ ] Reactive sliders
  - [ ] Optimized `ScrollRect` for large lists
