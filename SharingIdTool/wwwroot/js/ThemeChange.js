export function onTeamsThemeChange(dotNetHelper, methodName) {
    microsoftTeams.app.registerOnThemeChangeHandler(function (theme) {
        dotNetHelper.invokeMethodAsync(methodName, theme);
    });
}

export function updateBodyTheme(theme) {
    document.documentElement.setAttribute('data-theme', theme);
}