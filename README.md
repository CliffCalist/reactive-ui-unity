# ReactiveUI for Unity

ReactiveUI is a lightweight, modular UI framework for Unity, built around reactive programming and a simple lifecycle abstraction. It allows you to cleanly separate UI from game logic, while still enabling fully dynamic behavior.

## Features

1. Subscribe-and-react approach instead of imperative control
2. One base class — `UIView` — for all visual components
3. Self-managed lifecycle for reliable subscribe/unsubscribe
4. Built-in animation pipeline supporting Animator, DoTween, and custom solutions
5. Built-in UI elements like reactive buttons, selectors, menu bars, etc.
6. Scales well from quick prototyping to complex applications

## Installing

This package requires **[R3](https://github.com/Cysharp/R3?tab=readme-ov-file#unity)** to work.  
Make sure to install it in your project before using ReactiveUI.

---

To install via UPM, use **"Install package from git URL"** and add the following:

```text
1. https://github.com/CliffCalist/reactive-ui-unity.git
```

## Usage

### UIView Basics

- `UIView` is not abstract and not generic — you can add it directly to any `GameObject` without inheritance.
- If the `UIView` doesn’t bind to game logic and just needs show/hide behavior (with optional animation), this is the quickest way to get started.
- Optionally assign `_btnHide` in the inspector to allow the `UIView` to close itself via a button.
- If your `UIView` needs initialization (e.g. resolving components, caching references), override `InitCore()`.  
  It is guaranteed to be called before any logic is executed — even if the view starts inactive.
- If you need to ensure initialization in an edge case, you can call `InitIfFalse()` manually.

Example:
```csharp
_myUI.Show();
_myUI.Hide();

_myUI.ShowInHierarchyState
    .Where(state => state == UIShowState.AnimationEnded)
    .Subscribe(_ =>
    {
        // Fully visible
    });
```

See the full list of `UIView` properties in the section below.

---

### Binding to State

When your UI reflects some external state, inherit from `UIView` and define your binding logic.

- Use a method called `Bind(...)` to accept and cache the binding context. This does **not** bind immediately — it's only a caching step. Always call `RecreateSubscriptionsIfVisible()` after assigning new data. It will create subscriptions now if visible, or wait until shown.
- Do actual subscription logic in `CreateSubscriptionsCore()`.
This method may return an `IDisposable` representing all subscriptions created inside it. R3 provides a `DisposablesBuilder`, which lets you combine multiple subscriptions into one disposable object. `UIView` will automatically dispose that returned `IDisposable` when needed. If no subscriptions are created, you may return `null`.
- Subscriptions created **outside** of `CreateSubscriptionsCore()` (for example, inside helper methods, callbacks, or one-off interactions) must be managed manually. Usually, the most convenient approach is to cache such disposables and clean them up inside `DisposeSubscriptionsCore()`, which is invoked together with the main subscription disposal.


Example:
```csharp
public class MyUI : UIView
{
    private PlayerStats _playerStats;

    // Example of a subscription created outside CreateSubscriptionsCore
    private IDisposable _extraSubscription;



    protected override void InitCore()
    {
        // One-time setup
    }



    public void Bind(PlayerStats stats)
    {
        _playerStats = stats;
        RecreateSubscriptionsIfVisible();
    }

    protected override IDisposable CreateSubscriptionsCore()
    {
        if (_playerStats == null)
            return null;

        var builder = new DisposablesBuilder();

        // Example: build multiple subscriptions into one disposable
        _playerStats.HP
            .Subscribe(UpdateHPBar)
            .AddTo(ref builder);

        _playerStats.Mana
            .Subscribe(UpdateManaBar)
            .AddTo(ref builder);

        return builder.Build();
    }



    public void RegisterExtraListener(Inventory inventory)
    {
        // Subscription created outside CreateSubscriptionsCore
        _extraSubscription = inventory.Updated.Subscribe(_ => RefreshInventory());
    }

    protected override void DisposeSubscriptionsCore()
    {
        // Dispose manually-managed subscriptions
        _extraSubscription?.Dispose();
        _extraSubscription = null;
    }


    protected override void OnShowedCore()
    {
        // Runs after Show() completes (including animation)
    }

    protected override void OnHidedCore()
    {
        // Runs after Hide() completes
    }



    private void UpdateHPBar(int hp) { }
    private void UpdateManaBar(int mana) { }
    private void RefreshInventory() { }
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

You can attach animations to a `UIView` by either:
- Assigning it in code with `myUi.SetAnimations(...)`
- Attaching a `MonoBehaviour` component that implements `IUIAnimations` to the same GameObject

The system will:
- Automatically call `PlayShow()` / `PlayHide()` when appropriate
- Wait for animations to finish before updating state
- Deactivate the GameObject only after the hide animation completes

---

### Built-in Base Classes

ReactiveUI includes two helper bases:

| Class                | Use Case |
|---------------------|----------|
| `UIAnimations`     | Pure C# without MonoBehaviour |
| `MonoUIAnimations` | Unity components with inspector support |

Both provide:
- `Init()` lifecycle hook
- `ShowEnded`, `HideEnded` observables
- `PlayShowCore()`, `PlayHideCore()` for your logic
- `Dispose()` for cleanup

**Example:**
```csharp
public class ScalePop : MonoUIAnimations
{
    [SerializeField] private Transform _target;

    protected override void InitCore(UIView ui)
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

All `MonoUIAnimations` include inspector buttons for testing `Show()` and `Hide()` animations.

---

### Built-in Implementations

- `AnimatorUIAnimations` — integrates with Unity `Animator`
    - Uses `ObservableUIAnimatorTrigger`
    - You must place animation events in clips to call:
        - `OnAnimationShowEnded()`
        - `OnAnimationCloseEnded()`

- DoTween support via separate package:  
👉 [reactive-ui-dotween (GitHub)](https://github.com/CliffCalist/reactive-ui-dotween)

---

## Built-in Tools

1. `UIVisibilityTracker` — track show/hide status of multiple `UIView`
2. `UIUtilities` — static helper utilities for common UI operations  
   - **RebuildList** — efficiently creates, removes, and updates UI elements based on a data collection

## Built-in UI Elements

### Buttons
1. `UIButton` — reactive button with event streams  
2. `ShowUIButton / HideUIButton` — calls `Show()` / `Hide()` on the assigned `UIView`  
3. `SwitchUIButton` — hides one `UIView` and shows another  

---

### Selectors
1. `Selector<TData, TOption>` — base class for selectors with typed data and options  
2. `TabBar` — selector-based tab bar for switching between UI sections  

---

### Confirmation
1. `ConfirmationUIBase` — base class for confirmation UI logic  
2. `ConfirmationUI` — simple confirmation dialog using `Action<bool> onChoiceMade`  

---

### Authentication  
> These are abstract UI classes.  
> They do **not** enforce any backend (Firebase, PlayFab, custom API).  
> Your implementation should inherit from these classes and define the actual request logic (e.g., registration call, sign-in call, password reset API, etc.).  
> The UI handles validation, state, and interaction — you implement the backend calls.

#### General
1. `AuthFormUIBase** — base class for authentication-related forms  

#### Authorization
2. `RegistrationUIBase`
3. `SignInUIBase`
4. `ResetUserPasswordUIBase`
5. `CredentialConfirmUIBase` — base form for confirming account ownership (reauthentication / sensitive actions)  

#### User Data Modification
6. `ChangeUserEmailUIBase`
7. `ChangeUserPasswordUIBase`
8. `ChangeUserNameUIBase`

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
Observable.CombineLatest(_armor, _hp, (_armor, _hp) => _armor + _hp)
    .Subscribe(_ => UpdateHealthBar());
```

**Manual dispose via lifecycle:**
```csharp
_inventory.Updated
    .Subscribe(UpdateCounters)
    .AddTo(_disposables);
```

**From business logic event to observable:**
```csharp
Observable
    .FromEvent(
        h => _player.HeathChanged += h,
        h => _player.HeathChanged -= h
    )
    .Subscribe(_ => UpdateHealthBar());
```

---

## Roadmap

- [x] Automatic subscription disposal inside `UIView`
- [ ] Tools:
  - [ ] Optimized element spawner: reuse, create, destroy, and rebind efficiently
- [ ] UI Components:
  - [x] View-switcher button
