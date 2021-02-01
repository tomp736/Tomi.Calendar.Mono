export function notificationPermissions() {
    return Notification.permission;
}

export function askForApproval() {
    Notification.requestPermission(permission => {
        if (permission === 'granted') {
            createNotification('Wow! This is great', 'created by @study.tonight', '');
        }
    });
}

export function createNotification(id, title, body, icon) {
    console.log("createNotification: " + title)
    if (Notification.permission === "granted") {
        const noti = new Notification(title, {
            body: body,
            icon
        });
    }
}

export function scheduleNotification(dotNetObject, milliseconds, id, title, body, icon) {
    var handle = setTimeout(createNotification, milliseconds, id, title, body, icon);
    console.log("scheduleNotification: handle:" + handle + "title:" + title + " in " + milliseconds + " milliseconds");
    dotNetObject.invokeMethodAsync('NotificationScheduled', handle);
    return handle;
}

export function cancelNotification(dotNetObject, handle) {
    console.log("cancelNotification: " + handle)
    clearTimeout(handle);
    dotNetObject.invokeMethodAsync('NotificationCancelled');
}
