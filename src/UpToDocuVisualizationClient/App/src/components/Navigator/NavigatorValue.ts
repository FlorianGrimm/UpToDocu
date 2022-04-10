import { IDSNavigatorValue, RouteDefinition } from "dependingStateRouter";

export type NavigatorPaths = {
    home: string;
    graph: string;
    // pageA:string;
    // pageB:string[];
};

export type NavigatorPathName = (keyof NavigatorPaths);
export type NavigatorPageName = "home" | "graph" | "pageError";

export type NavigatorPages = {
    [K in NavigatorPathName]: NavigatorPageName;
};
export type NavigatorRoutes = {
    [K in NavigatorPathName]: RouteDefinition;
};

export type NavigatorPatterns = {
    [K in NavigatorPathName]: string;
};

export type NavigatorPathArguments = {};
export type NavigatorValue = IDSNavigatorValue<NavigatorPageName, NavigatorPathArguments>;