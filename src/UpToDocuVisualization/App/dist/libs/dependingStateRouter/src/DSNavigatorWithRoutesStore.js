import { deepEquals, getPropertiesChanged, } from "dependingState";
import { DSNavigatorStore } from "./DSNavigatorStore";
export class DSNavigatorWithRoutesStore extends DSNavigatorStore {
    constructor(pageNames, pageRouteNotFound, routes, storeName, stateValue, configuration) {
        super(storeName, stateValue, configuration);
        // already done in super: navigatorWithRoutesBuilder.bindValueStore(this);
        this.pageNames = pageNames;
        this.pageRouteNotFound = pageRouteNotFound;
        this.routes = routes;
    }
    matchPaths(location) {
        for (const pageName of this.pageNames) {
            const m = this.matchPath(location.pathname, this.routes[pageName]);
            if (m) {
                return [pageName, m];
            }
        }
        return null;
    }
    /** called if push or replace was called */
    handleLocationChanged(payload) {
        const { action, location, updateMode } = payload;
        {
            const result = this.matchPaths(location);
            if (result !== null) {
                const { pageName, pathArguments } = this.convertPageParameters(result[0], result[1], action, location, updateMode);
                this.setPagePathArguments(pageName, pathArguments);
            }
            else {
                this.setPagePathArguments(this.pageRouteNotFound, {});
            }
        }
    }
    setPagePathArguments(pageName, pathArguments) {
        const pc = getPropertiesChanged(this.stateValue);
        this.stateValue.value.pathArguments;
        pc.setIf("page", pageName);
        pc.setIf("pathArguments", pathArguments, deepEquals);
        pc.valueChangedIfNeeded("handleLocationChanged");
    }
    convertPageParameters(pageName, m, action, location, updateMode) {
        return { pageName, pathArguments: m.params };
    }
}
