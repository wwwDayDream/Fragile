# Content Warning Harmony Template

Thank you for using the mod template! Here are a few tips to help you on your journey:

## Versioning

BepInEx uses [semantic versioning, or semver](https://semver.org/), for the mod's version info.
[MinVer](https://github.com/adamralph/minver?tab=readme-ov-file#usage) will automatically
version your mod based on the latest git tag, as well as the number of commits made since then.

To create a new git tag, you can either use the git cli, or a git client,
such as GitHub Desktop, GitKraken, or the one built into your IDE.
For command line use, you can simply type in the following commands in the terminal/shell:

```shell
git tag v1.2.3
git push --tags
```

This creates a new tag, `v1.2.3`, at the currently checked-out commit,
and pushes the tag to the git version-control system (vcs).
MinVer will then be able to use this when you build your project to set your mod's version.

> **Note:** You *must* have a `v` in front of the version number, otherwise MinVer will not recognize it.
>
> If you prefer not to have `v1.2.3` and instead `1.2.3`, you can remove the `<MinVerTagPrefix>v</MinVerTagPrefix>` line in your `.csproj` file.

## Logging

A logger is provided to help with logging to the console.
You can access it by doing `Plugin.Logger` in any class outside the `Plugin` class.

***Please use*** `LogDebug()` ***whenever possible, as any other log method
will be displayed to the console and potentially cause performance issues for users.***

If you chose to do so, make sure you change the following line in the `BepInEx.cfg` file to see the Debug messages:

```toml
[Logging.Console]

# ... #

## Which log levels to show in the console output.
# Setting type: LogLevel
# Default value: Fatal, Error, Warning, Message, Info
# Acceptable values: None, Fatal, Error, Warning, Message, Info, Debug, All
# Multiple values can be set at the same time by separating them with , (e.g. Debug, Warning)
LogLevels = All
```

## Harmony

This template uses harmony. For more specifics on how to use it, look at
[the HarmonyX GitHub wiki](https://github.com/BepInEx/HarmonyX/wiki) and
[the Harmony docs](https://harmony.pardeike.net/).

To make a new harmony patch, just use `[HarmonyPatch]` before any class you make that has a patch in it.

Then in that class, you can use
`[HarmonyPatch(typeof(ClassToPatch), nameof(ClassToPatch.MethodToPatch))]`
where `ClassToPatch` is the class you're patching (ie `ShoppingCart`), and `MethodToPatch` is the method you're patching (ie `AddItemToCart`).

Then you can use
[the appropriate prefix, postfix, transpiler, or finalizer](https://harmony.pardeike.net/articles/patching.html) attribute.

_While you can use_ `return false;` _in a prefix patch,
it is **HIGHLY DISCOURAGED** as it can **AND WILL** cause compatibility issues with other mods._

For example, we want to add a patch that will add a random amount to the total value of items in the cart upon adding an item.
We have the following postfix patch patching the `AddItemToCart` method
in `ShoppingCart`:

```csharp
using System;
using System.Reflection;
using HarmonyLib;

namespace Fragile.Patches;

[HarmonyPatch(typeof(ShoppingCart))]
public class ExampleShoppingCartPatch
{
    [HarmonyPatch(nameof(ShoppingCart.AddItemToCart))]
    [HarmonyPostfix]
    private static void AddItemToCartPostfix(ShoppingCart __instance)
    {
        __instance.CartValue += new Random().Next(0, 100);
    }
}

```

In this case we include the type of the class we're patching in the attribute
before our `ExampleShoppingCartPatch` class,
as our class will only patch the `ShoppingCart` class.
