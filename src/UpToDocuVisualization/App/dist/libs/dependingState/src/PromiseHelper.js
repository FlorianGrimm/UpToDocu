import { dsLog } from ".";
export function catchLog(msg, promise) {
    return promise.then((v) => {
        return v;
    }, (reason) => {
        // console.error(msg, reason);
        dsLog.errorACME("DS", "handleDSEventHandlerResult", msg, reason);
        return undefined;
    });
}
export function handleDSEventHandlerResult(msg, p) {
    if (p && (typeof p.then === "function")) {
        return p.then((v) => {
            return v;
        }, (reason) => {
            // console.error(msg, reason);
            dsLog.errorACME("DS", "handleDSEventHandlerResult", msg, reason);
            return undefined;
        });
    }
}
