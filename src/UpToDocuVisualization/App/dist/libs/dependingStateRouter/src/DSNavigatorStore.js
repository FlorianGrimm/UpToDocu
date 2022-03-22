import { DSObjectStore, catchLog, deepEquals, dsLog, getPropertiesChanged, } from "dependingState";
import matchPath from './matchPath';
import { navigatorBuilder, navigatorSetLocation } from "./DSNavigatorActions";
export class DSNavigatorStore extends DSObjectStore {
    constructor(storeName, stateValue, configuration) {
        super(storeName, stateValue, configuration);
        navigatorBuilder.bindValueStore(this);
    }
    setRouter(routerStore) {
        this.routerStore = routerStore;
    }
    matchPath(pathname, options) {
        const result = matchPath(pathname, options);
        if (result !== null) {
            dsLog.debugACME("DS", "DSNavigatorStore", "matchPath", (pathname || ""));
        }
        return result;
    }
    initializeStore() {
        super.initializeStore();
        if (this.routerStore === undefined) {
            this.routerStore = this.storeManager.getValueStore("router");
        }
        if (this.routerStore === undefined) {
            throw new Error("router store not found");
        }
        else {
            this.routerStore.listenEventLocationChanged(this.storeName, (e) => {
                try {
                    dsLog.infoACME("DS", "DSNavigatorStore", "handleLocationChanged", e.payload.location.pathname);
                    const p = this.handleLocationChanged(e.payload);
                    if (p && typeof p.then === "function") {
                        return catchLog("handleLocationChanged", p);
                    }
                }
                catch (err) {
                    dsLog.errorACME("DS", "DSNavigatorStore", "handleLocationChanged-failed", err);
                }
            });
            navigatorSetLocation.listenEvent("setLocation", (e) => {
                try {
                    dsLog.infoACME("DS", "DSNavigatorStore", "handleSetLocation", e.payload.page);
                    const p = this.handleSetLocation(e.payload);
                    if (p && typeof p.then === "function") {
                        return catchLog("handleSetLocation", p);
                    }
                }
                catch (err) {
                    dsLog.errorACME("DS", "DSNavigatorStore", "handleSetLocation-failed", err);
                }
            });
        }
    }
    /**
     * called if push or replace was called
     * @param payload
     * @example
     *  const {action ,location, updateMode } = payload;
     *  {
     *      const m = this.matchPath(path:string, route:RouteDefinition);
     *      if (m) {
     *          m.params["dtStart"]
     *      }
     *  }
    */
    handleLocationChanged(payload) {
        /*
        const {action ,location, updateMode } = payload;
        {
            const m = this.matchPath(path:string, route:RouteDefinition);
            if (m) {
                m.params["dtStart"]
            }
        }
        */
    }
    /**
     * called from the code to navigate
     * @param payload
     */
    handleSetLocation(payload) {
        if (payload.to === undefined) {
            payload = this.convertTo(payload);
        }
        //payload.page
        //payload.pathArguments
        const stateValuePC = getPropertiesChanged(this.stateValue);
        stateValuePC.setIf("page", payload.page);
        stateValuePC.setIf("pathArguments", payload.pathArguments, deepEquals);
        // // TODO thinkof can we get rid of this?
        // stateValuePC.setIf("to", payload.to, deepEquals);
        // stateValuePC.setIf("pathPattern", payload.pathPattern || "");
        // stateValuePC.setIf("isExact", payload.isExact || false);
        stateValuePC.valueChangedIfNeeded("handleSetLocation");
        if (payload.to !== undefined) {
            this.routerStore.setLocationFromNavigator(payload.to);
        }
        if (payload.eventToProcess) {
            return this.storeManager.emitEvent(payload.eventToProcess);
        }
    }
    /**
     * converts pathName and pathArguments to to
     * @param payload
     * @returns modified payload or a new copy
     */
    convertTo(payload) {
        return payload;
    }
}
