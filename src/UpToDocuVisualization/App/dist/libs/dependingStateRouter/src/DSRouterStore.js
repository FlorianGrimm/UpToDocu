import { UpdateMode } from './history';
import { DSObjectStore, getPropertiesChanged, dsLog } from 'dependingState';
import { injectQuery } from './injectQuery';
import { routerLocationChanged, routerPush, routerReplace, dsRouterBuilder } from './DSRouterAction';
import { catchLog } from 'dependingState';
function noop() { }
// Define the initial state using that type
export function getRouterValueInitial() {
    const result = {
        location: {
            pathname: window.location.pathname,
            search: window.location.search,
            state: null,
            hash: window.location.hash,
            query: {},
            key: ""
        },
        action: "PUSH",
        updateMode: UpdateMode.Initialization
    };
    return result;
}
export class DSRouterStore extends DSObjectStore {
    constructor(history, stateValue, configuration) {
        super("router", stateValue, configuration);
        this.history = history;
        this.historyUnlisten = noop;
        this.suspressNavigator = false;
        dsRouterBuilder.bindValueStore(this);
    }
    initializeStore() {
        routerPush.listenEvent("router/push", (e) => {
            const location = e.payload;
            if (e.payload.noListener === true) {
                this.suspressNavigator = true;
            }
            this.history.push(location.to, location.state, location.updateMode ?? UpdateMode.FromCode, false);
        });
        routerReplace.listenEvent("router/replace", (e) => {
            const location = e.payload;
            if (e.payload.noListener === true) {
                this.suspressNavigator = true;
            }
            this.history.replace(location.to, location.state, location.updateMode ?? UpdateMode.FromCode, false);
        });
    }
    initializeBoot() {
        this.subscribe();
    }
    setLocationFromNavigator(to) {
        let locationTo;
        if (typeof to === "string") {
            locationTo = new URL(to, window.location.href);
        }
        else if (typeof to === "object") {
            locationTo = {
                pathname: "",
                search: "",
                hash: "",
                ...to
            };
        }
        else {
            // don't know what to do
            return;
        }
        const currentLocation = this.stateValue.value.location;
        if (((currentLocation.pathname || "") === (locationTo.pathname || ""))
            && ((currentLocation.search || "") === (locationTo.search || ""))
            && ((currentLocation.hash || "") === (locationTo.hash || ""))) {
            // skip href modification
        }
        else {
            this.suspressNavigator = true;
            this.history.push(to, undefined, UpdateMode.FromCode, false);
        }
    }
    listenEventLocationChanged(msg, callback) {
        return this.listenEvent(msg, "locationChanged", callback);
    }
    historyListener(update) {
        //console.warn("historyListener", this.suspressNavigator, update);
        const suspressNavigator = this.suspressNavigator;
        this.suspressNavigator = false;
        const locationPC = getPropertiesChanged(this.stateValue);
        locationPC.setIf("action", update.action || "PUSH");
        locationPC.setIf("location", injectQuery(update.location));
        locationPC.setIf("updateMode", update.updateMode);
        locationPC.valueChangedIfNeeded("historyListener");
        if (suspressNavigator) {
            // may be changed but ignore it
            dsLog.debugACME("DS", "DSRouterStore", "historyListener", update.location.pathname, "suspressNavigator");
        }
        else {
            const p = routerLocationChanged.emitEvent(this.stateValue.value);
            if (p && typeof p.then === "function") {
                return catchLog("routerLocationChanged", p);
            }
        }
    }
    subscribe() {
        this.unsubscribe();
        this.historyUnlisten = this.history.listen(this.historyListener.bind(this));
        this.historyListener({ location: this.history.location, action: this.history.action, updateMode: UpdateMode.Initialization });
    }
    unsubscribe() {
        this.historyUnlisten();
        this.historyUnlisten = () => { };
    }
}
