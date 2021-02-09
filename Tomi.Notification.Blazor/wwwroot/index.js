export function notificationPermissions() {
    return Notification.permission;
}

export function askForApproval() {
    Notification.requestPermission(permission => {
        if (permission === 'granted') {
            createNotification('Tomi.Calendar', 'Notifications Enabled', '');
        }
    });
}

export function createNotification(title, body, icon) {
    if (Notification.permission === "granted") {
        const noti = new Notification(title, {
            body: body,
            icon
        });
    }
}