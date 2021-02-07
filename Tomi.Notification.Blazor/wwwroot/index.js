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