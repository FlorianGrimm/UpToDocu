import { storeBuilder, } from 'dependingState';
export const dsRouterBuilder = storeBuilder("router");
export const routerPush = dsRouterBuilder.createAction("push");
export const routerReplace = dsRouterBuilder.createAction("replace");
export const routerLocationChanged = dsRouterBuilder.createAction("locationChanged");
