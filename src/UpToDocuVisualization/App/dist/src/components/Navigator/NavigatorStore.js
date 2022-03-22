import { DSNavigatorStore } from "dependingStateRouter";
export class NavigatorStore extends DSNavigatorStore {
    constructor(stateValue, configuration) {
        super("navigator", stateValue, configuration);
        this.pathNames = ["home", "pageA", "pageB"];
        const path = this.path = {
            home: "/",
            pageA: "/pageA",
            pageB: ["/pageB", "/pageB/:id"],
        };
        this.patterns = {
            home: path.home,
            pageA: path.pageA,
            pageB: path.pageB[1],
        };
        this.route = {
            home: {
                path: this.path.home,
                exact: true
            },
            pageA: {
                path: this.path.pageA
            },
            pageB: {
                path: this.path.pageB
            },
        };
        this.pages = {
            "home": "home",
            "pageA": "pageA",
            "pageB": "pageB",
        };
    }
    /** called if push or replace was called */
    handleLocationChanged(payload) {
        const { action, location, updateMode } = payload;
        {
            const result = this.matchPaths(location);
            if (result !== null) {
                const [pathName, m] = result;
                /*
                if (pathName==="pageB")
                */
                this.stateValue.value = {
                    page: this.pages[pathName],
                    pathArguments: m.params,
                    //isExact: m.isExact,
                    //pathName: pathName
                };
            }
            else {
                this.stateValue.value = {
                    page: "pageError",
                    pathArguments: {},
                    //isExact: false,
                    //pathName: ""
                };
            }
        }
    }
    matchPaths(location) {
        for (const pathName of this.pathNames) {
            const m = this.matchPath(location.pathname, this.route[pathName]);
            if (m) {
                return [pathName, m];
            }
        }
        return null;
    }
    convertTo(payload) {
        // payload.page
        return payload;
    }
}
